using AutoMapper;
using Poll.Application.Dtos;
using Poll.Application.DTOs.Poll;
using Poll.Domain.Entity;

namespace Poll.Application.Mappings
{
    public class PollProfile : Profile
    {
        public PollProfile()
        {
            CreateMap<PollEntity, PollListDto>()
                .ForMember(dest => dest.Options, opt => opt.MapFrom(src => src.Options))
                .ReverseMap();

            CreateMap<PollOption, PollOptionDto>()
                .ForMember(dest => dest.VoteCount, opt => opt.MapFrom(src => src.PollVotes.Count))
                .ReverseMap();

            CreateMap<PollEntity, PollDto>()
                .ForMember(dest => dest.Options, opt => opt.MapFrom(src => src.Options))
                .ReverseMap();
        }
    }
}
