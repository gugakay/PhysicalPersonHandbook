using DataAccess.Enums;


namespace DataAccess.Dtos.ConnectedPersonDtos
{
    public class ConnectedPersonViewDto
    {
        public int Id { get; set; }
        public PersonConnectionType Type { get; set; }
    }
}
