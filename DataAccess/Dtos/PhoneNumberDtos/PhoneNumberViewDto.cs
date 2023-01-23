using DataAccess.Enums;

namespace DataAccess.Dtos.PhoneNumberDtos
{
    public class PhoneNumberViewDto
    {
        public PhoneNumberType Type { get; set; }
        public string Number { get; set; }
    }
}
