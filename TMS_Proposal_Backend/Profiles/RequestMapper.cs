using AutoMapper;
using TMS_Proposal_Backend.DTOs;
using TMS_Proposal_Backend.Models;

namespace TMS_Proposal_Backend.Profiles
{
    public class RequestMapper:Profile
    {
        public RequestMapper() 
        {
            CreateMap<CreateRequestDTO, Request>()
                    .ForMember(dest => dest.Id, opt => opt.Ignore())
                    .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
                    .ForMember(dest => dest.CreatedDated, opt => opt.Ignore())
                    .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore());
            CreateMap<Request, ResponseRequestDTO>().ReverseMap();
        }
    }
}
