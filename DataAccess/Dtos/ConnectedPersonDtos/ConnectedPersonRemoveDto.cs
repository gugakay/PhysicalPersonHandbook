using DataAccess.Enums;


namespace DataAccess.Dtos.ConnectedPersonDtos
{
    public class ConnectedPersonRemoveDto
    {
        public int PersonId { get; set; }
        public int ConnectedPersonId { get; set; }
        public PersonConnectionType Type { get; set; }
    }
}
