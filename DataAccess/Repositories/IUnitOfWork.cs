using DataAccess.Entities;

namespace DataAccess
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Person> PersonRepository();
        IRepository<PhoneNumber> PhoneNumberRepository();
        IRepository<ConnectedPerson> ConnectedPersonRepository();

        IRepository<Image> ImageRepository();

        Task<int> SaveChangesAsync();
    }
}
