using DataAccess.Enums;

namespace DataAccess.Dtos.PhoneNumberDtos
{
    public class PhoneNumberCreateDto
    {
        public PhoneNumberType Type { get; set; }
        public string Number { get; set; }
    }
}
