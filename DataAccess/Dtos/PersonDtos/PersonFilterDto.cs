

namespace DataAccess.Dtos.PersonDtos
{
    public class PersonFilterDto
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string PrivateNumber { get; set; }
        
        
        public int? PageNumber { get; set; }
        public int? ItemsPerPage { get; set; }
    }
}
