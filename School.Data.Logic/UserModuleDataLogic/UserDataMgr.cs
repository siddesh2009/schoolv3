using Core.Common.Enum;
using School.Data.Contract;
using School.ViewModel;
using SchoolModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace School.Data.Logic
{
    public class UserDataMgr : IUserDataMgr
    {
        private readonly SchoolDbContext _dbContext;

        public UserDataMgr(SchoolDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public schooluser Authenticate(AuthenticateRequestViewModel model, string ipAddress)
        {
            return _dbContext.schoolusers.Where(x => x.EmailId == model.EmailId && x.Password == model.Password).FirstOrDefault();
        }

        public int UpdateUser(schooluser user)
        {
            using (var dbTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    _dbContext.Update(user);
                    int schoolUserId = _dbContext.SaveChanges();
                    dbTransaction.Commit();
                    return schoolUserId;
                }
                catch (Exception ex)
                {
                    dbTransaction.Rollback();
                    return 0;
                }
            }
        }

        public int SaveRefreshToken(refreshtoken refreshToken)
        {
            using (var dbTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    _dbContext.Update(refreshToken);
                    int refreshId = _dbContext.SaveChanges();
                    dbTransaction.Commit();
                    return refreshId;
                }
                catch (Exception ex)
                {
                    dbTransaction.Rollback();
                    return 0;
                }
            }
        }

        public int AddUser(schooluser user)
        {
            _dbContext.schoolusers.Add(user);
            return _dbContext.SaveChanges();
        }

        public schooluser GetRefreshTokens(string token)
        {
            schooluser user = new schooluser();
            refreshtoken refreshToken = new refreshtoken();
            refreshToken = _dbContext.refreshtokens.FirstOrDefault(a => a.Token == token);
            user = refreshToken.schoolusers.FirstOrDefault();
            user.refreshtoken = refreshToken;
            return user;
        }

        public void RevokeRefreshToken(string revokeToken)
        {
            refreshtoken refreshToken = _dbContext.refreshtokens.FirstOrDefault(a => a.Token == revokeToken);
            _dbContext.refreshtokens.Remove(refreshToken);
            _dbContext.SaveChanges();
        }

        public List<userdetail> GetAllUsers(int? roleId)
        {
            List<userdetail> userDetailsList = new List<userdetail>();
            if (roleId != null)
            {
                userDetailsList = (from usd in _dbContext.userdetails
                                   join ura in _dbContext.userroleassocs
                                   on new { usd.SchoolUserId } equals new { ura.SchoolUserId }
                                   where ura.RoleId == roleId
                                   select usd
                          ).ToList();
            }
            else
            {
                userDetailsList = _dbContext.userdetails.ToList();
            }
            return userDetailsList;
        }

        public schooluser GetUserById(int id)
        {
            schooluser user = new schooluser();
            user = _dbContext.schoolusers.Where(a => a.SchoolUserId == id).FirstOrDefault();
            user.refreshtoken = GetRefreshTokenById(Convert.ToInt32(user.RefreshTokenId));
            user.userdetails.Add(GetUserDetail(id));
            user.userroleassocs = GetUserRole(id);
            return user;
        }

        public List<countrymaster> GetAllCountry()
        {
            return _dbContext.countrymasters.ToList();
        }

        public countrymaster GetCountry(string countryCode)
        {
            return _dbContext.countrymasters.Where(a => a.CountryCode == countryCode).First();
        }

        public List<statesmaster> GetAllState()
        {
            return _dbContext.statesmasters.ToList();
        }

        public statesmaster GetState(string countryCode, string stateCode)
        {
            return _dbContext.statesmasters.Where(a => a.CountryCode == countryCode && a.StateCode == stateCode).First();
        }

        public List<citiesmaster> GetAllCities()
        {
            return _dbContext.citiesmasters.ToList();
        }

        public citiesmaster GetCity(string stateCode, string cityCode)
        {
            return _dbContext.citiesmasters.Where(a => a.StateCode == stateCode && a.CityCode == cityCode).First();
        }

        public userdetail GetUserDetail(int userId)
        {
            return _dbContext.userdetails.Where(a => a.SchoolUserId == userId).First();
        }

        public refreshtoken GetRefreshTokenById(int userRefreshTokenId)
        {
            return _dbContext.refreshtokens.FirstOrDefault(a => a.RefreshTokenId == userRefreshTokenId);
        }

        public int RegisterUserDetail(UserDetailViewModel userDetailViewModel)
        {
            schooluser user = new schooluser();
            user.FirstName = userDetailViewModel.FirstName;
            user.LastName = userDetailViewModel.LastName;
            user.EmailId = userDetailViewModel.EmailId;
            user.Password = userDetailViewModel.Password;
            user.CreatedBy = Convert.ToInt32(UserRoleEnum.Guest);
            user.CreatedDateTime = DateTime.Now;

            userdetail userDetail = new userdetail();
            userDetail.FirstName = userDetailViewModel.FirstName;
            userDetail.MiddleName = userDetailViewModel.MiddleName;
            userDetail.LastName = userDetailViewModel.LastName;
            userDetail.MobileContact = userDetailViewModel.MobileContact;
            userDetail.ProfessionsBkg = userDetailViewModel.ProfessionsBkg;
            userDetail.PassionateTopic = userDetailViewModel.PassionateTopic;
            userDetail.ProfessionalCertificates = userDetailViewModel.ProfessionalCertificates;
            userDetail.CountryCode = userDetailViewModel.CountryCode;
            userDetail.StateCode = userDetailViewModel.StateCode;
            userDetail.CityCode = userDetailViewModel.CityCode;

            using (var dbTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    int insertFlag = AddUser(user);
                    if (insertFlag != 0)
                    {
                        user.userdetails.Add(userDetail);

                        userroleassoc userroleassocObj = new userroleassoc();
                        userroleassocObj.SchoolUserId = user.SchoolUserId;
                        userroleassocObj.RoleId = userDetailViewModel.UserRoleId > Convert.ToInt32(UserRoleEnum.Guest) ? userDetailViewModel.UserRoleId : Convert.ToInt32(UserRoleEnum.Guest);
                        user.userroleassocs.Add(userroleassocObj);

                        _dbContext.schoolusers.Update(user);
                        _dbContext.SaveChanges();

                        dbTransaction.Commit();

                        bool userBoardMappingFlag = AddUpdateUserRoleAssoc(userDetailViewModel.boardGradeSubUserList, userroleassocObj.SchoolUserId, userroleassocObj.RoleId);
                    }

                    return user.SchoolUserId;
                }
                catch (Exception ex)
                {
                    dbTransaction.Rollback();
                    return 0;
                }
            }
        }

        public bool AddUpdateUserRoleAssoc(List<BoardGradeSubUserAssocViewModel> boardGradeSubUserList, int? userId, int? roleId)
        {
            bool insertedRecordFlag = false;
            int userRoleAccocId = _dbContext.userroleassocs.Where(a => a.SchoolUserId == userId && a.RoleId == roleId).First().UserRoleAssocId;
            if (userRoleAccocId != 0)
            {
                var result = boardGradeSubUserList.Select(a => new { a.BoardId, a.GradeId, a.SubjectId }).Distinct().ToList();
                using (var dbTransaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var item in result)
                        {
                            bool assocAvailability = _dbContext.boardgradesubassocs.Any(a => a.BoardId == item.BoardId && a.GradeId == item.GradeId && a.SubjectId == item.SubjectId && a.UserRoleAssocId == userRoleAccocId);
                            if (!assocAvailability)
                            {
                                boardgradesubassoc boardgradesubassocObj = new boardgradesubassoc();
                                boardgradesubassocObj.BoardId = item.BoardId;
                                boardgradesubassocObj.GradeId = item.GradeId;
                                boardgradesubassocObj.SubjectId = item.SubjectId;
                                boardgradesubassocObj.UserRoleAssocId = userRoleAccocId;
                                _dbContext.boardgradesubassocs.Update(boardgradesubassocObj);
                                _dbContext.SaveChanges();
                            }
                        }
                        dbTransaction.Commit();
                        insertedRecordFlag = true;
                    }
                    catch (Exception ex)
                    {
                        insertedRecordFlag = false;
                        dbTransaction.Rollback();
                    }
                }
            }

            return insertedRecordFlag;
        }

        public int AddUserRoleAssoc(userroleassoc userRoleAssocObj)
        {
            _dbContext.userroleassocs.Add(userRoleAssocObj);
            return _dbContext.SaveChanges();
        }

        public List<rolemaster> GetAllRoles()
        {
            return _dbContext.rolemasters.ToList();
        }

        public List<userroleassoc> GetUserRole(int userId)
        {
            return _dbContext.userroleassocs.Where(a => a.SchoolUserId == userId).ToList();
        }

        public rolemaster GetRole(int roleId)
        {
            return _dbContext.rolemasters.Where(a => a.RoleId == roleId).First();
        }

        public List<boardmaster> GetAllBoards()
        {
            return _dbContext.boardmasters.ToList();
        }

        public boardmaster GetBoard(int boardId)
        {
            return _dbContext.boardmasters.Where(a => a.BoardId == boardId).First();
        }

        public List<grademaster> GetAllGrades()
        {
            return _dbContext.grademasters.ToList();
        }

        public grademaster GetGrade(int gradeId)
        {
            return _dbContext.grademasters.Where(a => a.GradeId == gradeId).First();
        }

        public List<subjectmaster> GetAllSubjects()
        {
            return _dbContext.subjectmasters.ToList();
        }

        public subjectmaster GetSubject(int subjectId)
        {
            return _dbContext.subjectmasters.Where(a => a.SubjectId == subjectId).First();
        }

        public List<int> CreateUserRoleSchedule(List<UserRoleScheduleViewModel> userRoleScheduleViewModel)
        {
            List<int> schuduleIdList = new List<int>();
            using (var dbTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in userRoleScheduleViewModel)
                    {
                        if (item.BoardGradeSubAssocId == 0)
                        {
                            boardgradesubassoc boardgradesubassoc = _dbContext.boardgradesubassocs.Where(a => a.BoardId == item.BoardId && a.GradeId == item.GradeId && a.SubjectId == item.SubjectId && a.UserRoleAssocId == item.UserRoleAssocId).FirstOrDefault();
                            if (boardgradesubassoc != null)
                            {
                                item.BoardGradeSubAssocId = boardgradesubassoc.BoardGradeSubAssocId;
                            }
                            else
                            {
                                boardgradesubassoc newboardgradesubassoc = new boardgradesubassoc();
                                newboardgradesubassoc.BoardId = item.BoardId;
                                newboardgradesubassoc.GradeId = item.GradeId;
                                newboardgradesubassoc.SubjectId = item.SubjectId;
                                newboardgradesubassoc.UserRoleAssocId = item.UserRoleAssocId;
                                _dbContext.boardgradesubassocs.Add(newboardgradesubassoc);
                                _dbContext.SaveChanges();
                                item.BoardGradeSubAssocId = newboardgradesubassoc.BoardGradeSubAssocId;
                            }
                        }
                        userroleschedule userroleschedule = new userroleschedule();
                        userroleschedule.BoardGradeSubAssocId = item.BoardGradeSubAssocId;
                        userroleschedule.UserRoleAssocId = item.UserRoleAssocId;
                        userroleschedule.ScheduleTopic = item.ScheduleTopic;
                        userroleschedule.TopicDesc = item.TopicDesc;
                        userroleschedule.ScheduledDateTime = item.ScheduledDateTime;
                        userroleschedule.ScheduledDuration = item.ScheduledDuration;
                        userroleschedule.MinCandidateSize = item.MinCandidateSize;
                        userroleschedule.MaxCandidateSize = item.MaxCandidateSize;
                        userroleschedule.TopicRefMaterialBasePath = item.TopicRefMaterialBasePath;
                        userroleschedule.TopicRefMaterial = item.TopicRefMaterial;
                        userroleschedule.ScheduleCost = item.ScheduleCost;
                        //userroleschedule.UserRoleScheduleId = item.UserRoleScheduleId;

                        _dbContext.userroleschedule.Add(userroleschedule);
                        _dbContext.SaveChanges();
                        dbTransaction.Commit();

                        schuduleIdList.Add(userroleschedule.UserRoleScheduleId);
                    }
                }
                catch (Exception ex)
                {
                    dbTransaction.Rollback();
                }
            }

            return schuduleIdList;
        }

        public List<int> UpdateUserRoleSchedule(List<UserRoleScheduleViewModel> userRoleScheduleViewModel)
        {
            List<int> scheduleIdList = new List<int>();
            using (var dbTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in userRoleScheduleViewModel)
                    {
                        userroleschedule userroleschedule = _dbContext.userroleschedule.Where(a => a.UserRoleScheduleId == item.UserRoleScheduleId).FirstOrDefault();
                        if (userroleschedule != null)
                        {
                            _dbContext.userroleschedule.Remove(userroleschedule);
                        }
                    }
                    _dbContext.SaveChanges();
                    dbTransaction.Commit();
                    scheduleIdList = CreateUserRoleSchedule(userRoleScheduleViewModel);
                }
                catch (Exception ex)
                {
                    dbTransaction.Rollback();
                }
            }
            return scheduleIdList;
        }

        public List<userroleschedule> GetUserRoleSchedule(UserRoleScheduleRequestViewModel userRoleScheduleRequestViewModel)
        {
            List<userroleschedule> userroleschedules = new List<userroleschedule>();
            if (userRoleScheduleRequestViewModel.UserRoleScheduleId > 0)
            {
                userroleschedules = _dbContext.userroleschedule.Where(a => a.UserRoleScheduleId == userRoleScheduleRequestViewModel.UserRoleScheduleId).ToList();
            }
            else {
                userroleschedules = _dbContext.userroleschedule.ToList();
            }
            return userroleschedules;
        }

        public boardgradesubassoc GetUserRoleAssocBoard(int boardGradeSubjectAssocId)
        {
            return _dbContext.boardgradesubassocs.Where(a => a.BoardGradeSubAssocId == boardGradeSubjectAssocId).FirstOrDefault();
        }

        public void UpdateDbErrorLog(string message)
        {
            errorLogger errorLoggerObj = new errorLogger();
            errorLoggerObj.ErrorLoggerMessage = message;
            _dbContext.errorLogger.Add(errorLoggerObj);
            _dbContext.SaveChanges();
        }
    }
}
