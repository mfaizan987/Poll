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
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Options, opt => opt.MapFrom(src => src.Options))
                .ForMember(dest => dest.Question, opt => opt.MapFrom(src => src.Question))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
                .ForMember(dest => dest.AllowMultipleAnswers, opt => opt.MapFrom(src => src.AllowMultipleAnswers))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();

            CreateMap<CreatePollDto, PollEntity>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.UserName, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Options, opt => opt.MapFrom(src => src.Options));

            CreateMap<CreatePollOptionDto, PollOption>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PollId, opt => opt.Ignore())
                .ForMember(dest => dest.Poll, opt => opt.Ignore())
                .ForMember(dest => dest.PollVotes, opt => opt.Ignore());
        }
    }
}
