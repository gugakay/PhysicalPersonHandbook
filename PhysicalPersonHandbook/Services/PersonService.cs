using AutoMapper;
using DataAccess;
using DataAccess.Dtos.ConnectedPersonDtos;
using DataAccess.Dtos.PersonDtos;
using DataAccess.Entities;
using DataAccess.Enums;
using Microsoft.EntityFrameworkCore;
using ILogger = Serilog.ILogger;
using DataAccess.Extensions;
using DataAccess.Dtos.ImageDtos;
using PhysicalPersonHandbook.Infrastructure.Result;
using System.Transactions;
using PhysicalPersonHandbook.Infrastructure.TestData;
using DataAccess.Resources.Person;
using Microsoft.Extensions.Localization;

namespace PhysicalPersonHandBook.Services
{
    public class PersonService : IPersonService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IFileService _fileService;
        private readonly IStringLocalizer<PersonResources> _resourcesLocalizer;

        public PersonService(
            IUnitOfWork unitOfWork, 
            IMapper mapper,
            ILogger logger,
            IFileService fileService,
            IStringLocalizer<PersonResources> resourcesLocalizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _fileService = fileService;
            _resourcesLocalizer = resourcesLocalizer;
        }

        public async Task<ServiceResult> AddPersonAsync(PersonCreateDto person, CancellationToken cancellationToken)
        {
            var newPerson = _mapper.Map<Person>(person);

            using (TransactionScope transaction = new(TransactionScopeAsyncFlowOption.Enabled))
            {

                var createdPerson = await _unitOfWork.PersonRepository().AddAsync(newPerson, cancellationToken);
                await _unitOfWork.SaveChangesAsync();

                var result = await _fileService.AddImageAsync(new ImageCreateDto
                {
                    PersonId = createdPerson.Id,
                    Image = person.Image
                },
                cancellationToken);

                if (!result.IsSuccess)
                {
                    _logger.Error("Error while adding image to person with id {PersonId}", createdPerson.Id);
                    transaction.Dispose();
                    return new ServiceResult(false, "Error while adding image to person");
                }

                transaction.Complete();
                return new ServiceResult(true, "Person added successfully", _mapper.Map<PersonViewDto>(createdPerson));
            }
        }

        public async Task<ServiceResult> AddConnectedPersonAsync(ConnectedPersonCreateDto person, CancellationToken cancellationToken)
        {
            var existingPerson = await GetQueryablePerson(person.PersonId)
                                        .Include(x => x.ConnectedPersons)
                                        .SingleOrDefaultAsync(cancellationToken);

            if (existingPerson == null)
                return new ServiceResult(false, _resourcesLocalizer["PersonNotFound"]);

            var existingConnectedPerson = await GetQueryablePerson(person.ConnectedPersonId)
                                                .SingleOrDefaultAsync(cancellationToken);

            if (existingConnectedPerson == null)
                return new ServiceResult(false, "Connected person not found");

            var connectionExist = existingPerson.ConnectedPersons
                                        .Any(x => x.ConnectedPersonId == existingConnectedPerson.Id 
                                        && x.Type == person.Type);

            if (connectionExist)
                return new ServiceResult(false, "Connection already exist");

            existingPerson.ConnectedPersons.Add(new ConnectedPerson
            {
                PersonId = existingPerson.Id,
                ConnectedPersonId = person.ConnectedPersonId,
                Type = person.Type
            });

            existingPerson = await _unitOfWork.PersonRepository().UpdateAsync(existingPerson, cancellationToken);
            await _unitOfWork.SaveChangesAsync();

            await TryAddTwoWayRelationAsync(existingConnectedPerson, existingPerson, person.Type, cancellationToken);

            return new ServiceResult(true, "Connected person added successfully");
        }

        public async Task TryAddTwoWayRelationAsync(Person existingConnectedPerson, Person existingPerson, PersonConnectionType connectionType, CancellationToken cancellationToken)
        {
            switch (connectionType)
            {
                case PersonConnectionType.Colleague:
                    await AddTwoWayRelationAsync(existingConnectedPerson, existingPerson, PersonConnectionType.Colleague, cancellationToken); break;
                case PersonConnectionType.Acquaintance:
                    await AddTwoWayRelationAsync(existingConnectedPerson, existingPerson, PersonConnectionType.Acquaintance, cancellationToken); break;
                case PersonConnectionType.Relative:
                    await AddTwoWayRelationAsync(existingConnectedPerson, existingPerson, PersonConnectionType.Relative, cancellationToken); break;
                case PersonConnectionType.Other: break;
            }
        }

        public async Task AddTwoWayRelationAsync(Person existingConnectedPerson, Person existingPerson, PersonConnectionType connectionType, CancellationToken cancellationToken)
        {
            var connectionExist = existingConnectedPerson.ConnectedPersons
                                                .Any(x => x.ConnectedPersonId == existingPerson.Id && x.Type == connectionType);
            if (!connectionExist)
            {
                existingConnectedPerson.ConnectedPersons.Add(new ConnectedPerson
                {
                    PersonId = existingConnectedPerson.Id,
                    ConnectedPersonId = existingPerson.Id,
                    Type = connectionType
                });

                await _unitOfWork.PersonRepository().UpdateAsync(existingConnectedPerson, cancellationToken);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<ServiceResult> RemoveConnectedPersonAsync(ConnectedPersonRemoveDto person, CancellationToken cancellationToken)
        {
            var existingPerson = await GetQueryablePerson(person.PersonId)
                                        .Include(x => x.ConnectedPersons)
                                        .SingleAsync(cancellationToken);

            if (existingPerson == null)
                return new ServiceResult(false, _resourcesLocalizer["PersonNotFound"]);

            var existingConnectedPerson = await GetPersonEntityByIdAsync(person.ConnectedPersonId, cancellationToken);

            if (existingConnectedPerson == null)
                return new ServiceResult(false, "Connected person not found");

            var personConnection = _unitOfWork.ConnectedPersonRepository().GetAllQueryable()
                                        .Single(x => x.PersonId == existingPerson.Id
                                        && x.ConnectedPersonId == existingConnectedPerson.Id
                                        && x.Type == person.Type);

            if (personConnection == null)
                return new ServiceResult(false, "Connection does not exist");

            _unitOfWork.ConnectedPersonRepository().Delete(personConnection);
            await _unitOfWork.SaveChangesAsync();

            var existingConnectedPersonConnection = await _unitOfWork.ConnectedPersonRepository().GetAllQueryable()
                                    .SingleOrDefaultAsync(x => x.PersonId == existingConnectedPerson.Id
                                    && x.ConnectedPersonId == existingPerson.Id
                                    && x.Type == person.Type, cancellationToken);

            if (existingConnectedPersonConnection != null)
            {
                _unitOfWork.ConnectedPersonRepository().Delete(existingConnectedPersonConnection);
                await _unitOfWork.SaveChangesAsync();
            }

            return new ServiceResult(true, "Connection was removed successfully");
        }

        public async Task<ServiceResult> GetPersons(PersonFilterDto PersonFilter, CancellationToken cancellationToken)
        {
            var persons = _unitOfWork.PersonRepository()
                .GetAllQueryable()
                .Include(x => x.ConnectedPersons)
                .Include(x => x.PhoneNumbers).AsQueryable();

            if(!await persons.AnyAsync(cancellationToken))
                return new ServiceResult(false, _resourcesLocalizer["NoPersonsFound"]);

            if (PersonFilter != null)
            {
                if (string.IsNullOrWhiteSpace(PersonFilter.Name) == false)
                    persons = persons.Where(x => x.Name.Contains(PersonFilter.Name));

                if (string.IsNullOrWhiteSpace(PersonFilter.LastName) == false)
                    persons = persons.Where(x => x.LastName.Contains(PersonFilter.LastName));

                if (string.IsNullOrWhiteSpace(PersonFilter.PrivateNumber) == false)
                    persons = persons.Where(x => x.PrivateNumber.Contains(PersonFilter.PrivateNumber));
            }
            
            if (PersonFilter.ItemsPerPage > 0 && PersonFilter.PageNumber > 0)
            {
                persons = IQueryableExtensions.Page(persons, PersonFilter.PageNumber, PersonFilter.ItemsPerPage);
            }

            var personsView = _mapper.Map<List<PersonViewDto>>(await persons.ToListAsync(cancellationToken));

            return new ServiceResult(true, "", personsView);
        }

        public ServiceResult GetPersonsDetailed(PersonFilterDetailedDto PersonFilter)
        {
            var persons = _unitOfWork.PersonRepository()
                .GetAllQueryable()
                .Include(x => x.ConnectedPersons)
                .Include(x => x.PhoneNumbers).AsQueryable();

            if (!persons.Any())
                return new ServiceResult(false, "No persons found");

            if (PersonFilter != null)
            {
                if (string.IsNullOrWhiteSpace(PersonFilter.Name) == false)
                    persons = persons.Where(x => x.Name.Equals(PersonFilter.Name));

                if (string.IsNullOrWhiteSpace(PersonFilter.LastName) == false)
                    persons = persons.Where(x => x.LastName.Equals(PersonFilter.LastName));

                if (string.IsNullOrWhiteSpace(PersonFilter.PrivateNumber) == false)
                    persons = persons.Where(x => x.PrivateNumber.Equals(PersonFilter.PrivateNumber));

                if (PersonFilter.BirthDate.HasValue)
                    persons = persons.Where(x => x.BirthDate.Equals(PersonFilter.BirthDate));

                if (PersonFilter.CityCode.HasValue)
                    persons = persons.Where(x => x.CityCode.Equals(PersonFilter.CityCode));
                
                if (PersonFilter.Gender.HasValue)
                    persons = persons.Where(x => x.Gender.Equals(PersonFilter.Gender));
            }

            if (PersonFilter.ItemsPerPage > 0 && PersonFilter.PageNumber > 0)
            {
                persons = IQueryableExtensions.Page(persons, PersonFilter.PageNumber, PersonFilter.ItemsPerPage);
            }

            var personsView = _mapper.Map<IEnumerable<PersonViewDto>>(persons);

            return new ServiceResult(true, "", personsView);
        }
    

        public ServiceResult GetPersonsConnectionReport()
        {
            var persons = _unitOfWork.ConnectedPersonRepository()
                .GetAllQueryable();

            if (!persons.Any())
                return new ServiceResult(false, "No connections found");

            var group = persons.GroupBy(x => x.PersonId).Select(p => new PersonConnectionReportDto
            {
                PersonId = p.Key,
                ConnectedPersonReport = new List<ConnectedPersonReportDto>(p.GroupBy(x => x.Type).Select(x => new ConnectedPersonReportDto
                {
                    Type = x.Key,
                    ConnectedPersonsCount = x.Count()
                }))
            }).AsEnumerable();

            return new ServiceResult(true, "", group);
        }

        public async Task<ServiceResult> GetPersonByIdAsync(int personId, CancellationToken cancellationToken)
        {
            var person = await GetQueryablePerson(personId)
                .Include(x => x.ConnectedPersons)
                .Include(x => x.PhoneNumbers)
                .Include(x => x.Image)
                .SingleOrDefaultAsync(cancellationToken);

            if (person == null)
                return new ServiceResult(false, _resourcesLocalizer["PersonNotFound"]);

            return new ServiceResult(true, null, _mapper.Map<PersonDetailedViewDto>(person));
        }

        public async Task<ServiceResult> UpdateAsync(PersonUpdateDto updatePerson, CancellationToken cancellationToken)
        {
            var existingPerson = await GetQueryablePerson(updatePerson.Id).SingleOrDefaultAsync(cancellationToken);

            if (existingPerson == null)
                return new ServiceResult(false, _resourcesLocalizer["PersonNotFound"]);

            UpdatePersonFields(ref existingPerson, updatePerson);
            var updatedPerson = await _unitOfWork.PersonRepository().UpdateAsync(existingPerson, cancellationToken);

            await _unitOfWork.SaveChangesAsync();

            return new ServiceResult(true, "Person updated successfully", _mapper.Map<PersonViewDto>(updatedPerson));
        }

        public async Task<ServiceResult> DeleteAsync(int personId, CancellationToken cancellationToken)
        {
            var existingPerson = await _unitOfWork.PersonRepository()
                .GetAllAsNoTrackingQueryable()
                .Where(x => x.Id == personId)
                .SingleOrDefaultAsync(cancellationToken);

            if (existingPerson == null)
                return new ServiceResult(false, _resourcesLocalizer["PersonNotFound"]);

            _unitOfWork.PersonRepository().Delete(existingPerson);
            await _unitOfWork.SaveChangesAsync();

            var existingConnections = await _unitOfWork.ConnectedPersonRepository()
                .GetAllAsNoTrackingQueryable()
                .Where(x => x.PersonId == personId || x.ConnectedPersonId == personId)
                .ToListAsync(cancellationToken);

            if (existingConnections.Any())
            {
                await _unitOfWork.ConnectedPersonRepository().BulkDeleteAsync(existingConnections);
                await _unitOfWork.SaveChangesAsync();
            }

            return new ServiceResult(true, "Person deleted successfully");
        }

        public async Task<Person> GetPersonEntityByIdAsync(int personId, CancellationToken cancellationToken) =>
            await _unitOfWork.PersonRepository().GetByIdAsync(personId, cancellationToken);

        public IQueryable<Person> GetQueryablePerson(int personId) =>
            _unitOfWork.PersonRepository()
                .GetAllQueryable()
                .Where(x => x.Id == personId)
                .Include(x => x.PhoneNumbers);

        private static void UpdatePersonFields(ref Person existingPerson, PersonUpdateDto personUpdate)
        {
            if (!string.IsNullOrEmpty(personUpdate.Name))
                existingPerson.Name = personUpdate.Name;

            if (!string.IsNullOrEmpty(personUpdate.LastName))
                existingPerson.LastName = personUpdate.LastName;

            if (personUpdate.Gender.HasValue)
                existingPerson.Gender = (Gender)personUpdate.Gender;

            if (!string.IsNullOrEmpty(personUpdate.PrivateNumber))
                existingPerson.PrivateNumber = personUpdate.PrivateNumber;

            if (personUpdate.BirthDate.HasValue)
                existingPerson.BirthDate = (DateTime)personUpdate.BirthDate;

            if (personUpdate.CityCode.HasValue)
                existingPerson.CityCode = (int)personUpdate.CityCode;

            if (personUpdate.PhoneNumbers != null && personUpdate.PhoneNumbers.Any())
                UpdatePhoneNumbers(ref existingPerson, personUpdate);
        }

        private static void UpdatePhoneNumbers(ref Person existingPerson, PersonUpdateDto personUpdate)
        {
            foreach (var newPhoneNumber in personUpdate.PhoneNumbers)
            {
                var existingPhoneNumber = existingPerson.PhoneNumbers.FirstOrDefault(x => x.Id == newPhoneNumber.Id);

                if (existingPhoneNumber != null)
                {
                    if (!string.IsNullOrEmpty(newPhoneNumber.Number))
                        existingPhoneNumber.Number = newPhoneNumber.Number;

                    if (newPhoneNumber.Type.HasValue)
                        existingPhoneNumber.Type = (PhoneNumberType)newPhoneNumber.Type;
                }
            }
        }

        public async Task<ServiceResult> PopulateAsync(CancellationToken cancellationToken)
        {
            var persons = TestData.GetPersons();

            _unitOfWork.PersonRepository().AddRange(persons);

            await _unitOfWork.SaveChangesAsync();

            return new ServiceResult(true, "Populated");
        }
    }
}
