using System.ComponentModel.DataAnnotations.Schema;
using DataAccess.Enums;

namespace DataAccess.Entities
{
    [Table("ConnectedPersons")]
    public class ConnectedPerson
    {
        public int Id { get; set; }
        [Column("PersonId")]
        public int PersonId { get; set; }
        public int ConnectedPersonId { get; set; }

        public PersonConnectionType Type { get; set; }
        public Person Person { get; set; }
    }
}
