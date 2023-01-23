using DataAccess.Enums;


namespace DataAccess.Dtos.ConnectedPersonDtos
{
    public class ConnectedPersonCreateDto
    {
        public int PersonId { get; set; }
        public int ConnectedPersonId { get; set; }
        public PersonConnectionType Type { get; set; }
    }
}
