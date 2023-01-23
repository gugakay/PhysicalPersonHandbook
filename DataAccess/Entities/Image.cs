using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    [Table("Images")]
    public class Image
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public int PersonId { get; set; }
        public Person Person { get; set; }
    }
}
