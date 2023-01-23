using DataAccess.Enums;


namespace DataAccess.Dtos.ConnectedPersonDtos
{
    public class ConnectedPersonReportDto
    {
        public PersonConnectionType Type { get; set; }
        public int ConnectedPersonsCount { get; set; }
    }
}
