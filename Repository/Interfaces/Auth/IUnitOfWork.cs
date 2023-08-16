using MyPSG.API.Repository.Interfaces.Auth;

namespace MyPSG.API.Repository.Interfaces
{
    public interface IUnitOfWork 
    {
        IAuthRepository AuthRepository { get; }
        IRolePrivilegeRepository RolePrivilegeRepository {get;}
        IRoleRepository RoleRepository {get;}
        IMenuRepository MenuRepository { get; }
        // ICompanyProfileRepository CompanyProfileRepository {get;}  
        IAppVersionInfoRepository AppVersionInfoRepository {get;}
        // IFileUploadRepository FileUploadRepository {get;}
        

    }
}