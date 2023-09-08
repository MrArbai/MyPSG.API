using MyPSG.API.Repository.Interfaces.Master;
using MyPSG.API.Repository.Interfaces;

namespace MyPSG.API.Repository.Implements.Master
{
    public class UnitOfWorkMaster : IUnitOfWorkMaster
    {
        private IDapperContext _context;
        private IItemRepository _itemRepository;
        private IItemUomRepository _itemUomRepository;
      
        public UnitOfWorkMaster(IDapperContext context)
        {
            _context = context;
        }

        // ============== Repository =================
        public IItemRepository ItemRepository {
            get { return _itemRepository ??= new ItemRepository(_context); }
        }
        public IItemUomRepository ItemUomRepository {
            get { return _itemUomRepository ??= new ItemUomRepository(_context); }
        }
       
    }
}   