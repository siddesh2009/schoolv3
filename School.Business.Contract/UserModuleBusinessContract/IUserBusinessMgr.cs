using School.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Business.Contract
{
    public interface IUserBusinessMgr
    {
        AuthenticateResponseViewModel Authenticate(AuthenticateRequestViewModel model, string ipAddress);

        AuthenticateResponseViewModel RefreshToken(string token, string ipAddress);

        bool RevokeToken(string token, string ipAddress);

        List<UserDetailViewModel> GetAllUsers(int? roleId);

        UserViewModel GetUserById(int id);

        int RegisterUserDetail(UserDetailViewModel userDetailViewModel);

        List<RoleMasterViewModel> GetAllRoles();

        List<CountryMasterViewModel> GetAllCountries();

        List<StateMasterViewModel> GetAllStates();

        List<CityMasterViewModel> GetAllCities();

        List<BoardMasterViewModel> GetAllBoards();

        List<GradeMasterViewModel> GetAllGrades();

        List<SubjectMasterViewModel> GetAllSubjects();

        List<int> CreateUserRoleSchedule(List<UserRoleScheduleViewModel> userRoleScheduleViewModel);

        List<int> UpdateUserRoleSchedule(List<UserRoleScheduleViewModel> userRoleScheduleViewModel);

        List<UserRoleScheduleViewModel> GetUserRoleSchedule(UserRoleScheduleRequestViewModel userRoleScheduleRequestViewModel);

        void UpdateDbErrorLog(string message);
    }
}
