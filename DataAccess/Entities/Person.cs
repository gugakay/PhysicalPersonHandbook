using System.ComponentModel.DataAnnotations.Schema;
using DataAccess.Enums;

namespace DataAccess.Entities
{
    [Table("Persons")]
    public class Person
    {
        [Column("PersonId")]
        public int Id { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public Gender Gender { get; set; }

        public string PrivateNumber { get; set; }

        public DateTime BirthDate { get; set; }

        public int CityCode { get; set; }

        public Image Image { get; set; }

        public ICollection<PhoneNumber> PhoneNumbers { get; set; } = new HashSet<PhoneNumber>();

        public ICollection<ConnectedPerson> ConnectedPersons { get; set; } = new HashSet<ConnectedPerson>();
    }
}
