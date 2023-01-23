using DataAccess.Enums;

namespace DataAccess.Dtos.PhoneNumberDtos
{
    public class PhoneNumberUpdateDto
    {
        public int Id { get; set; }
        public PhoneNumberType? Type { get; set; }
        public string Number { get; set; }
    }
}
