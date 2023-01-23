using System.ComponentModel.DataAnnotations.Schema;
using DataAccess.Enums;

namespace DataAccess.Entities
{
    [Table("PhoneNumbers")]
    public class PhoneNumber
    {
        [Column("PhoneNumberId")]
        public int Id { get; set; }
        public PhoneNumberType Type { get; set; }
        public string Number { get; set; }
        public int PersonId { get; set; }
        public Person Person { get; set; }
    }
}
