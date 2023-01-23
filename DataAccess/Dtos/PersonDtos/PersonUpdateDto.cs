using DataAccess.Dtos.PhoneNumberDtos;
using DataAccess.Enums;


namespace DataAccess.Dtos.PersonDtos
{
    public class PersonUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string LastName { get; set; }

        public Gender? Gender { get; set; }

        public string PrivateNumber { get; set; }

        public DateTime? BirthDate { get; set; }

        public int? CityCode { get; set; }
        public IEnumerable<PhoneNumberUpdateDto> PhoneNumbers { get; set; }
    }
}