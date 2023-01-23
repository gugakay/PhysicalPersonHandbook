using AutoMapper;
using DataAccess.Dtos.PersonDtos;
using DataAccess.Dtos.PhoneNumberDtos;
using DataAccess.Entities;
using DataAccess.Dtos.ConnectedPersonDtos;

namespace CarBox.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Person
            CreateMap<PersonCreateDto, Person>()
               .ForMember(x => x.PhoneNumbers, y => y.MapFrom(x => x.PhoneNumbers))
               .ForMember(x => x.Image, y => y.Ignore());

            CreateMap<Person, PersonViewDto>()
                .ForMember(x => x.PhoneNumbers, y => y.MapFrom(x => x.PhoneNumbers))
                .ForMember(x => x.ConnectedPersons, y => y.MapFrom(x => x.ConnectedPersons));

            CreateMap<Person, PersonDetailedViewDto>()
            .ForMember(x => x.Image, y => y.MapFrom(x => File.ReadAllBytes(x.Image.Path)));
            
            CreateMap<PhoneNumberCreateDto, PhoneNumber>()
                .ForMember(x => x.Person, opt => opt.Ignore());

            CreateMap<PhoneNumber, PhoneNumberViewDto>();
            CreateMap<ConnectedPerson, ConnectedPersonViewDto>();
        }
    }
}
