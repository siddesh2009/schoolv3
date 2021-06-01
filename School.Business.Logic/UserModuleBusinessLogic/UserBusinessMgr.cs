using AutoMapper;
using Core.Common.Enum;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using School.Business.Contract;
using School.Data.Contract;
using School.ViewModel;
using SchoolModel.Model;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace School.Business.Logic
{
    public class UserBusinessMgr : IUserBusinessMgr
    {
        private readonly IUserDataMgr _userDataMgr;
        private readonly AppSettingsViewModel _appSettings;

        public UserBusinessMgr(IUserDataMgr userDataMgr, IOptions<AppSettingsViewModel> appSettings)
        {
            _userDataMgr = userDataMgr;
            _appSettings = appSettings.Value;
        }

        public AuthenticateResponseViewModel Authenticate(AuthenticateRequestViewModel model, string ipAddress)
        {
            var user = _userDataMgr.Authenticate(model, ipAddress);

            if (user == null) return null;

            var jwtToken = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken(ipAddress, "");
            if (user.RefreshTokenId != null)
            {
                var userRefreshToken = _userDataMgr.GetRefreshTokenById(Convert.ToInt32(user.RefreshTokenId));
                if (userRefreshToken.Expires < DateTime.UtcNow)
                {
                    userRefreshToken.IsActive = 1;
                    userRefreshToken.IsExpired = 0;
                    userRefreshToken.Token = refreshToken.Token;
                    userRefreshToken.Expires = DateTime.UtcNow;
                    int refreshTokenSave = _userDataMgr.SaveRefreshToken(userRefreshToken);
                }
                else {
                    user.refreshtoken = userRefreshToken;
                }
            }
            else
            {
                user.refreshtoken = refreshToken;
            }
            _userDataMgr.UpdateUser(user);
            return new AuthenticateResponseViewModel(user, jwtToken, refreshToken.Token);
        }

        private string GenerateJwtToken(schooluser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("id", user.SchoolUserId.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private refreshtoken GenerateRefreshToken(string ipAddress, string oldToken)
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return new refreshtoken
                {
                    Token = Convert.ToBase64String(randomBytes),
                    Expires = DateTime.UtcNow.AddMinutes(15),
                    IsExpired = 0,
                    CreatedDateTime = DateTime.UtcNow,
                    CreatedByIp = ipAddress,
                    ReplacedByToken = oldToken.Length > 0 ? oldToken : "",
                    IsActive = 1
                };
            }
        }

        public AuthenticateResponseViewModel RefreshToken(string token, string ipAddress)
        {
            var user = _userDataMgr.GetRefreshTokens(token);

            // return null if no user found with token
            if (user == null) return null;

            var refreshToken = user.refreshtoken.Token;

            if (user.refreshtoken.IsActive != 1) return null;

            var newRefreshToken = GenerateRefreshToken(ipAddress, token);
            user.refreshtoken.RevokedDateTime = DateTime.UtcNow;
            user.refreshtoken.RevokedByIp = ipAddress;
            user.refreshtoken.ReplacedByToken = newRefreshToken.Token;

            _userDataMgr.UpdateUser(user);
            var jwtToken = GenerateJwtToken(user);
            return new AuthenticateResponseViewModel(user, jwtToken, newRefreshToken.Token);
        }

        public bool RevokeToken(string token, string ipAddress)
        {
            var user = _userDataMgr.GetRefreshTokens(token);

            if (user == null) return false;
            user.RefreshTokenId = null;

            _userDataMgr.UpdateUser(user);

            _userDataMgr.RevokeRefreshToken(token);
            return true;
        }

        public List<UserDetailViewModel> GetAllUsers(int? roleId)
        {
            List<userdetail> modelUsersList = _userDataMgr.GetAllUsers(roleId);
            List<UserDetailViewModel> uiUserList = new List<UserDetailViewModel>();
            for (int i = 0; i < modelUsersList.Count; i++)
            {
                UserDetailViewModel userDetailViewModel = new UserDetailViewModel();
                userDetailViewModel.FirstName = modelUsersList[i].FirstName;
                userDetailViewModel.MiddleName = modelUsersList[i].MiddleName;
                userDetailViewModel.LastName = modelUsersList[i].LastName;
                userDetailViewModel.MobileContact = Convert.ToInt32(modelUsersList[i].MobileContact);
                userDetailViewModel.PassionateTopic = modelUsersList[i].PassionateTopic;
                userDetailViewModel.ProfessionalCertificates = modelUsersList[i].ProfessionalCertificates;
                userDetailViewModel.ProfessionsBkg = modelUsersList[i].ProfessionsBkg;
                userDetailViewModel.UserId = modelUsersList[i].SchoolUserId;
                userDetailViewModel.CountryCode = _userDataMgr.GetCountry(modelUsersList[i].CountryCode).CountryName;
                userDetailViewModel.StateCode = _userDataMgr.GetState(modelUsersList[i].CountryCode, modelUsersList[i].StateCode).StateName;
                userDetailViewModel.CityCode = _userDataMgr.GetCity(modelUsersList[i].StateCode, modelUsersList[i].CityCode).CityName;
                userDetailViewModel.UserRoleId = Convert.ToInt32(roleId != null ? roleId : 1);
                uiUserList.Add(userDetailViewModel);
            }
            return uiUserList;
        }

        public UserViewModel GetUserById(int id)
        {
            var user = _userDataMgr.GetUserById(id);
            var userDetail = user.userdetails.First();
            UserViewModel userViewModel = new UserViewModel();
            userViewModel.FirstName = userDetail.FirstName;
            userViewModel.MiddleName = userDetail.MiddleName;
            userViewModel.LastName = userDetail.LastName;
            userViewModel.MobileContact = userDetail.MobileContact;
            userViewModel.SchoolUserId = userDetail.SchoolUserId;
            userViewModel.ProfessionsBkg = userDetail.ProfessionsBkg;
            userViewModel.PassionateTopic = userDetail.PassionateTopic;
            userViewModel.ProfessionalCertificates = userDetail.ProfessionalCertificates;
            userViewModel.MobileContact = userDetail.MobileContact;
            userViewModel.Country = _userDataMgr.GetCountry(userDetail.CountryCode).CountryName;
            userViewModel.State = _userDataMgr.GetState(userDetail.CountryCode, userDetail.StateCode).StateName;
            userViewModel.City = _userDataMgr.GetCity(userDetail.StateCode, userDetail.CityCode).CityName;

            List<RoleMasterViewModel> userRoleViewModelList = new List<RoleMasterViewModel>();
            foreach (var item in user.userroleassocs)
            {
                RoleMasterViewModel userRoleViewModel = new RoleMasterViewModel();
                var roleMasterObj = _userDataMgr.GetRole(item.RoleId);
                userRoleViewModel.RoleId = item.RoleId;
                userRoleViewModel.RoleName = roleMasterObj.RoleName;
                userRoleViewModelList.Add(userRoleViewModel);
            }
            userViewModel.userRoleViewModel = userRoleViewModelList;

            return userViewModel;
        }

        public int RegisterUserDetail(UserDetailViewModel userDetailViewModel)
        {
            int userId = _userDataMgr.RegisterUserDetail(userDetailViewModel);
            return userId;
        }

        public List<RoleMasterViewModel> GetAllRoles()
        {
            List<rolemaster> rolemastersList = _userDataMgr.GetAllRoles();
            List<RoleMasterViewModel> roleMasterViewModelList = new List<RoleMasterViewModel>();
            foreach (var item in rolemastersList)
            {
                RoleMasterViewModel roleMasterViewModel = new RoleMasterViewModel();
                roleMasterViewModel.RoleId = item.RoleId;
                roleMasterViewModel.RoleName = item.RoleName;
                roleMasterViewModelList.Add(roleMasterViewModel);
            }
            return roleMasterViewModelList;
        }

        public List<CountryMasterViewModel> GetAllCountries()
        {
            List<countrymaster> countrymastersList = _userDataMgr.GetAllCountry();
            List<CountryMasterViewModel> countryMasterViewModelList = new List<CountryMasterViewModel>();
            foreach (var item in countrymastersList)
            {
                CountryMasterViewModel countryMasterViewModel = new CountryMasterViewModel();
                countryMasterViewModel.CountryCode = item.CountryCode;
                countryMasterViewModel.CountryName = item.CountryName;
                countryMasterViewModelList.Add(countryMasterViewModel);
            }
            return countryMasterViewModelList;
        }

        public List<StateMasterViewModel> GetAllStates()
        {
            List<statesmaster> statemastersList = _userDataMgr.GetAllState();
            List<StateMasterViewModel> stateMasterViewModelList = new List<StateMasterViewModel>();
            foreach (var item in statemastersList)
            {
                StateMasterViewModel stateMasterViewModel = new StateMasterViewModel();
                stateMasterViewModel.StateCode = item.StateCode;
                stateMasterViewModel.StateName = item.StateName;
                stateMasterViewModel.CountryCode = item.CountryCode;
                stateMasterViewModelList.Add(stateMasterViewModel);
            }
            return stateMasterViewModelList;
        }

        public List<CityMasterViewModel> GetAllCities()
        {
            List<citiesmaster> citymastersList = _userDataMgr.GetAllCities();
            List<CityMasterViewModel> cityMasterViewModelList = new List<CityMasterViewModel>();
            foreach (var item in citymastersList)
            {
                CityMasterViewModel cityMasterViewModel = new CityMasterViewModel();
                cityMasterViewModel.CityCode = item.CityCode;
                cityMasterViewModel.CityName = item.CityName;
                cityMasterViewModel.StateCode = item.StateCode;
                cityMasterViewModelList.Add(cityMasterViewModel);
            }
            return cityMasterViewModelList;
        }

        public List<BoardMasterViewModel> GetAllBoards()
        {
            List<boardmaster> boardmastersList = _userDataMgr.GetAllBoards();
            List<BoardMasterViewModel> boardMasterViewModelList = new List<BoardMasterViewModel>();
            foreach (var item in boardmastersList)
            {
                BoardMasterViewModel boardMasterViewModel = new BoardMasterViewModel();
                boardMasterViewModel.BoardId = item.BoardId;
                boardMasterViewModel.BoardName = item.BoardName;
                boardMasterViewModelList.Add(boardMasterViewModel);
            }
            return boardMasterViewModelList;
        }

        public List<GradeMasterViewModel> GetAllGrades()
        {
            List<grademaster> grademastersList = _userDataMgr.GetAllGrades();
            List<GradeMasterViewModel> gradeMasterViewModelList = new List<GradeMasterViewModel>();
            foreach (var item in grademastersList)
            {
                GradeMasterViewModel gradeMasterViewModel = new GradeMasterViewModel();
                gradeMasterViewModel.GradeId = item.GradeId;
                gradeMasterViewModel.GradeName = item.GradeName;
                gradeMasterViewModelList.Add(gradeMasterViewModel);
            }
            return gradeMasterViewModelList;
        }

        public List<SubjectMasterViewModel> GetAllSubjects()
        {
            List<subjectmaster> subjectmastersList = _userDataMgr.GetAllSubjects();
            List<SubjectMasterViewModel> subjectMasterViewModelList = new List<SubjectMasterViewModel>();
            foreach (var item in subjectmastersList)
            {
                SubjectMasterViewModel subjectMasterViewModel = new SubjectMasterViewModel();
                subjectMasterViewModel.SubjectId = item.SubjectId;
                subjectMasterViewModel.SubjectName = item.SubjectName;
                subjectMasterViewModelList.Add(subjectMasterViewModel);
            }
            return subjectMasterViewModelList;
        }

        public List<int> CreateUserRoleSchedule(List<UserRoleScheduleViewModel> userRoleScheduleViewModel)
        {
            return _userDataMgr.CreateUserRoleSchedule(userRoleScheduleViewModel);
        }

        public List<int> UpdateUserRoleSchedule(List<UserRoleScheduleViewModel> userRoleScheduleViewModel)
        {
            return _userDataMgr.UpdateUserRoleSchedule(userRoleScheduleViewModel);
        }

        public List<UserRoleScheduleViewModel> GetUserRoleSchedule(UserRoleScheduleRequestViewModel userRoleScheduleRequestViewModel)
        {
            List<UserRoleScheduleViewModel> userRoleScheduleViewModelsList = new List<UserRoleScheduleViewModel>();
            List<userroleschedule> userroleschedules = _userDataMgr.GetUserRoleSchedule(userRoleScheduleRequestViewModel);
            List<boardmaster> boardmastersList = _userDataMgr.GetAllBoards();
            List<grademaster> grademasterList = _userDataMgr.GetAllGrades();
            List<subjectmaster> subjectmasterList = _userDataMgr.GetAllSubjects();

            foreach (var item in userroleschedules)
            {
                boardgradesubassoc boardgradesubassocObj = _userDataMgr.GetUserRoleAssocBoard(item.BoardGradeSubAssocId);

                UserRoleScheduleViewModel userRoleScheduleViewModel = new UserRoleScheduleViewModel();
                userRoleScheduleViewModel.UserRoleScheduleId = item.UserRoleScheduleId;
                userRoleScheduleViewModel.UserRoleAssocId = item.UserRoleAssocId;
                userRoleScheduleViewModel.ScheduleTopic = item.ScheduleTopic;
                userRoleScheduleViewModel.TopicDesc = item.TopicDesc;
                userRoleScheduleViewModel.BoardGradeSubAssocId = item.BoardGradeSubAssocId;
                userRoleScheduleViewModel.BoardId = boardmastersList.Where(a => a.BoardId == boardgradesubassocObj.BoardId).FirstOrDefault().BoardId;
                userRoleScheduleViewModel.BoardName = boardmastersList.Where(a => a.BoardId == boardgradesubassocObj.BoardId).FirstOrDefault().BoardName;
                userRoleScheduleViewModel.GradeId = grademasterList.Where(a => a.GradeId == boardgradesubassocObj.GradeId).FirstOrDefault().GradeId;
                userRoleScheduleViewModel.GradeName = grademasterList.Where(a => a.GradeId == boardgradesubassocObj.GradeId).FirstOrDefault().GradeName;
                userRoleScheduleViewModel.SubjectId = subjectmasterList.Where(a => a.SubjectId == boardgradesubassocObj.SubjectId).FirstOrDefault().SubjectId;
                userRoleScheduleViewModel.SubjectName = subjectmasterList.Where(a => a.SubjectId == boardgradesubassocObj.SubjectId).FirstOrDefault().SubjectName;
                userRoleScheduleViewModel.ScheduledDateTime = item.ScheduledDateTime;
                userRoleScheduleViewModel.ScheduledDuration = item.ScheduledDuration;
                userRoleScheduleViewModel.MinCandidateSize = item.MinCandidateSize;
                userRoleScheduleViewModel.MaxCandidateSize = item.MaxCandidateSize;
                userRoleScheduleViewModel.TopicRefMaterial = item.TopicRefMaterial;
                userRoleScheduleViewModel.ScheduleCost = item.ScheduleCost;
                userRoleScheduleViewModelsList.Add(userRoleScheduleViewModel);
            }

            return userRoleScheduleViewModelsList;
        }

        public void UpdateDbErrorLog(string message)
        {
            _userDataMgr.UpdateDbErrorLog(message);
        }
    }
}
