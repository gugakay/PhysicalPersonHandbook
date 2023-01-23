using DataAccess.Dtos.ConnectedPersonDtos;
using DataAccess.Dtos.PersonDtos;
using PhysicalPersonHandbook.Infrastructure.Result;

namespace PhysicalPersonHandBook.Services
{
    public interface IPersonService
    {
        Task<ServiceResult> AddPersonAsync(PersonCreateDto person, CancellationToken cancellationToken);
        Task<ServiceResult> AddConnectedPersonAsync(ConnectedPersonCreateDto personConnection, CancellationToken cancellationToken);
        Task<ServiceResult> RemoveConnectedPersonAsync(ConnectedPersonRemoveDto personConnection, CancellationToken cancellationToken);
        Task<ServiceResult> GetPersonByIdAsync(int personId, CancellationToken cancellationToken);
        Task<ServiceResult> GetPersons(PersonFilterDto personFilter, CancellationToken cancellationToken);
        ServiceResult GetPersonsDetailed(PersonFilterDetailedDto personFilter);
        ServiceResult GetPersonsConnectionReport();

        Task<ServiceResult> UpdateAsync(PersonUpdateDto person, CancellationToken cancellationToken);
        Task<ServiceResult> DeleteAsync(int personId, CancellationToken cancellationToken);

        Task<ServiceResult> PopulateAsync(CancellationToken cancellationToken);
    }
}
