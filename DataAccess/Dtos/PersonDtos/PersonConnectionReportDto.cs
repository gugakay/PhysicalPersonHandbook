using DataAccess.Dtos.ConnectedPersonDtos;

namespace DataAccess.Dtos.PersonDtos
{
    public class PersonConnectionReportDto
    {
        public int PersonId { get; set; }
        public IEnumerable<ConnectedPersonReportDto> ConnectedPersonReport { get; set; }
    }
}
