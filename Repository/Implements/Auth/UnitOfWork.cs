using MyPSG.API.Repository.Implements.Auth;
using MyPSG.API.Repository.Interfaces;
using MyPSG.API.Repository.Interfaces.Auth;
using Opium.Api.Repository.Implements.Utl;

namespace MyPSG.API.Repository.Implements
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDapperContext _context;
        private IAuthRepository _authRepository;
        private IRolePrivilegeRepository _rolePrivilegeRepository;
        private IRoleRepository _roleRepository;
        private IMenuItemRepository _menuItemRepository;
        private IMenuRepository _menuRepository;
        // private ICompanyProfileRepository _companyProfile; 
        private IAppVersionInfoRepository _appVersionInfoRepository; 
        // private IFileUploadRepository _fileUploadRepository; 
      
        public UnitOfWork(IDapperContext context)
        {
            _context = context;
        }

        // ============== Repository =================
        public IAuthRepository AuthRepository {
            get { return _authRepository ??= new AuthRepository(_context); }
        }
        public IRolePrivilegeRepository RolePrivilegeRepository {
            get { return _rolePrivilegeRepository ??= new RolePrivilegeRepository(_context); }
        }
        public IRoleRepository RoleRepository {
            get { return _roleRepository ??= new RoleRepository(_context); }
        } 
        public IMenuRepository MenuRepository {
            get { return _menuRepository ??= new MenuRepository(_context); }
        }
        public IMenuItemRepository MenuItemRepository {
            get { return _menuItemRepository ??= new MenuItemRepository(_context); }
        }
        // public ICompanyProfileRepository CompanyProfileRepository {
        //     get { return _companyProfile ?? (_companyProfile = new CompanyProfileRepository(_context)); }
        // }
        public IAppVersionInfoRepository AppVersionInfoRepository {
            get { return _appVersionInfoRepository ??= new AppVersionInfoRepository(_context);}
        }
        // public IFileUploadRepository FileUploadRepository {
        //     get { return _fileUploadRepository ?? (_fileUploadRepository = new FileUploadRepository(_context));}
        // }
    }
}   