using AutoMapper;
using TMS_Proposal_Backend.DTOs;
using TMS_Proposal_Backend.Models.Auth;

namespace TMS_Proposal_Backend.Profiles
{
    public class UserMapper : Profile
    {
        public UserMapper() {
            CreateMap<SignUpDTO, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Password, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDated, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<User, UserResponseDTO>();
        }
    }
}
