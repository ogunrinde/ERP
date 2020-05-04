using AutoMapper;
using ICSERP.Entities.UserManagament;
using ICSERP.Models.UserManagement;

namespace ERP.Helpers
{
    public class AutoMappingProfile : Profile
    {
        public AutoMappingProfile()
        {
            CreateMap<User, UserModel>();
            CreateMap<RegisterModel, User>();
            CreateMap<RoleModel, Role>();
            CreateMap<Role,ReturnRoleModel>();
            CreateMap<EmployeePersonalInformation, PersonalInformation>();
            CreateMap<BranchModel, Branch>();
            CreateMap<Branch, BranchModel>();

            CreateMap<UnitModel, Unit>();
            CreateMap<Unit, UnitModel>();

            CreateMap<DepartmentModel, Department>();
            CreateMap<Department, DepartmentModel>();
        }
    }
}