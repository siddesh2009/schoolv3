using School.ViewModel;
using SchoolModel.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Data.Contract
{
    public interface IUserDataMgr
    {
        schooluser Authenticate(AuthenticateRequestViewModel model, string ipAddress);

        int UpdateUser(schooluser user);

        schooluser GetRefreshTokens(string token);

        List<userdetail> GetAllUsers(int? roleId);

        schooluser GetUserById(int id);

        int RegisterUserDetail(UserDetailViewModel userDetailViewModel);

        refreshtoken GetRefreshTokenById(int userRefreshTokenId);

        int SaveRefreshToken(refreshtoken refreshToken);

        void RevokeRefreshToken(string revokeToken);

        List<rolemaster> GetAllRoles();

        List<userroleassoc> GetUserRole(int userId);

        rolemaster GetRole(int roleId);

        int AddUserRoleAssoc(userroleassoc userRoleAssocObj);

        List<countrymaster> GetAllCountry();

        countrymaster GetCountry(string countryCode);

        List<statesmaster> GetAllState();

        statesmaster GetState(string countryCode, string stateCode);

        List<citiesmaster> GetAllCities();

        citiesmaster GetCity(string stateCode, string cityCode);

        List<boardmaster> GetAllBoards();

        boardmaster GetBoard(int boardId);

        List<grademaster> GetAllGrades();

        grademaster GetGrade(int gradeId);

        List<subjectmaster> GetAllSubjects();

        subjectmaster GetSubject(int subjectId);

        List<int> CreateUserRoleSchedule(List<UserRoleScheduleViewModel> userRoleScheduleViewModel);

        List<int> UpdateUserRoleSchedule(List<UserRoleScheduleViewModel> userRoleScheduleViewModel);

        List<userroleschedule> GetUserRoleSchedule(UserRoleScheduleRequestViewModel userRoleScheduleRequestViewModel);

        boardgradesubassoc GetUserRoleAssocBoard(int boardGradeSubjectAssocId);

        void UpdateDbErrorLog(string message);
    }
}





