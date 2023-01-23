using DataAccess.Enums;

namespace DataAccess.Dtos.PersonDtos
{
    public class PersonFilterDetailedDto
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public Gender? Gender { get; set; }
        public string PrivateNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? CityCode { get; set; }
        
        
        public int? PageNumber { get; set; }
        public int? ItemsPerPage { get; set; }
    }
}
