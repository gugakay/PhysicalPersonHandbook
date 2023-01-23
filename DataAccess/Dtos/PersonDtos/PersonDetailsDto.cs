using DataAccess.Dtos.PhoneNumberDtos;
using DataAccess.Entities;
using DataAccess.Enums;

namespace DataAccess.Dtos.PersonDtos
{
    public class PersonDetailsDto
    {
        public string Name { get; set; }

        public string LastName { get; set; }

        public Gender Gender { get; set; }

        public string PrivateNumber { get; set; }

        public DateTime BirthDate { get; set; }

        public int CityCode { get; set; }

        public IEnumerable<PhoneNumberViewDto> PhoneNumbers { get; set; }

        public IEnumerable<ConnectedPerson> ConnectedPersons { get; set; }
    }
}
