using DataAccess.Entities;

namespace DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DefaultDbContext _dbContext;

        public UnitOfWork(DefaultDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private IRepository<Person> _personRepository;
        public IRepository<Person> PersonRepository() => _personRepository ??= new Repository<Person>(_dbContext);

        private IRepository<PhoneNumber> _phoneNumberRepository;
        public IRepository<PhoneNumber> PhoneNumberRepository() => _phoneNumberRepository ??= new Repository<PhoneNumber>(_dbContext);

        private IRepository<ConnectedPerson> _connectedPersonRepository;
        public IRepository<ConnectedPerson> ConnectedPersonRepository() => _connectedPersonRepository ??= new Repository<ConnectedPerson>(_dbContext);

        private IRepository<Image> _imageRepository;
        public IRepository<Image> ImageRepository() => _imageRepository ??= new Repository<Image>(_dbContext);

        public async Task<int> SaveChangesAsync() =>
            await _dbContext.SaveChangesAsync();

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

