using DataAccess.Dtos.ConnectedPersonDtos;
using DataAccess.Dtos.ImageDtos;
using DataAccess.Dtos.PersonDtos;
using Microsoft.AspNetCore.Mvc;
using PhysicalPersonHandbook.Infrastructure.Result;
using PhysicalPersonHandBook.Services;
using System.Net;

namespace PhysicalPersonHandBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : Controller
    {
        private readonly IPersonService _personService;
        private readonly IFileService _fileService;

        public PersonController(IPersonService personService, IFileService fileService)
        {
            _personService = personService;
            _fileService = fileService;
        }

        [HttpPost("persons")]
        public async Task<ResultWrapper> Persons(PersonFilterDto personFilter, CancellationToken cancellationToken)
        {
            var result = await _personService.GetPersons(personFilter, cancellationToken);

            if (!result.IsSuccess)
                return new ResultWrapper(HttpStatusCode.NotFound, result);

            return new ResultWrapper(HttpStatusCode.OK, result);
        }

        [HttpPost("persons-detailed")]
        public ResultWrapper PersonsDetailed(PersonFilterDetailedDto personFilter)
        {
            var result = _personService.GetPersonsDetailed(personFilter);

            if (!result.IsSuccess)
                return new ResultWrapper(HttpStatusCode.NotFound, result);

            return new ResultWrapper(HttpStatusCode.OK, result);
        }

        [HttpPost("persons-report")]
        public ResultWrapper PersonsReport()
        {
            var result = _personService.GetPersonsConnectionReport();

            if (!result.IsSuccess)
                return new ResultWrapper(HttpStatusCode.NotFound, result);

            return new ResultWrapper(HttpStatusCode.OK, result);
        }

        [HttpGet("{id}")]
        public async Task<ResultWrapper> Get(int id, CancellationToken cancellationToken)
        {
            var result = await _personService.GetPersonByIdAsync(id, cancellationToken);

            if (!result.IsSuccess)
                return new ResultWrapper(HttpStatusCode.NotFound, result);

            return new ResultWrapper(HttpStatusCode.OK, result);
        }

        [HttpPost("person")]
        public async Task<ResultWrapper> Add([FromForm]PersonCreateDto person, CancellationToken cancellationToken)
        {
            var result = await _personService.AddPersonAsync(person, cancellationToken);

            if (!result.IsSuccess)
                return new ResultWrapper(HttpStatusCode.NotFound, result);

            return new ResultWrapper(HttpStatusCode.Created, result);
        }

        [HttpPatch]
        public async Task<ResultWrapper> Update(PersonUpdateDto person, CancellationToken cancellationToken)
        {
            var result = await _personService.UpdateAsync(person, cancellationToken);

            if (!result.IsSuccess)
                return new ResultWrapper(HttpStatusCode.NotFound, result);

            return new ResultWrapper(HttpStatusCode.OK, result);
        }

        [HttpPost("connected-person")]
        public async Task<ResultWrapper> AddConnectedPerson(ConnectedPersonCreateDto connectedPerson, CancellationToken cancellationToken)
        {
            var result = await _personService.AddConnectedPersonAsync(connectedPerson, cancellationToken);

            if (!result.IsSuccess)
                return new ResultWrapper(HttpStatusCode.NotFound, result);

            return new ResultWrapper(HttpStatusCode.OK, result);
        }

        [HttpDelete("connected-person")]
        public async Task<ResultWrapper> RemoveConnectedPerson(ConnectedPersonRemoveDto personConnection, CancellationToken cancellationToken)
        {
            var result = await _personService.RemoveConnectedPersonAsync(personConnection, cancellationToken);

            if (!result.IsSuccess)
                return new ResultWrapper(HttpStatusCode.NotFound, result);

            return new ResultWrapper(HttpStatusCode.OK, result);
        }

        [HttpDelete("{id}")]
        public async Task<ResultWrapper> Delete(int id, CancellationToken cancellationToken)
        {
            var result = await _personService.DeleteAsync(id, cancellationToken);

            if (!result.IsSuccess)
                return new ResultWrapper(HttpStatusCode.NotFound, result);

            return new ResultWrapper(HttpStatusCode.OK, result);
        }

        [HttpPost("image")]
        public async Task<ResultWrapper> Add([FromForm] ImageCreateDto image, CancellationToken cancellationToken)
        {
            var result = await _fileService.AddImageAsync(image, cancellationToken);

            if (!result.IsSuccess)
                return new ResultWrapper(HttpStatusCode.NotFound, result);

            return new ResultWrapper(HttpStatusCode.OK, result);
        }

        [HttpGet]
        public async Task<ResultWrapper> Populate(CancellationToken cancellationToken)
        {
            var result = await _personService.PopulateAsync(cancellationToken);

            if (!result.IsSuccess)
                return new ResultWrapper(HttpStatusCode.NotFound, result);

            return new ResultWrapper(HttpStatusCode.OK, result);
        }
    }
}
