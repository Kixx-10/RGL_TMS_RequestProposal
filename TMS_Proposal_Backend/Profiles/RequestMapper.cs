using AutoMapper;
using TMS_Proposal_Backend.DTOs;
using TMS_Proposal_Backend.Models;
using TMS_Proposal_Backend.Models.Master;

namespace TMS_Proposal_Backend.Profiles
{
    public class RequestMapper : Profile
    {
        public RequestMapper()
        {
   
            CreateMap<CreateRequestDTO, Request>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDated, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
              
                .ForMember(dest => dest.Trucks, opt => opt.MapFrom(src => src.Trucks))
                .ForMember(dest => dest.Attachments, opt => opt.MapFrom(src => src.Attachments));

        
            CreateMap<Request, ResponseRequestDTO>()
                .ForMember(dest => dest.Trucks, opt => opt.MapFrom(src => src.Trucks))
                .ForMember(dest => dest.Attachments, opt => opt.MapFrom(src => src.Attachments));

            // Truck Mappings
            CreateMap<CreateTruckDTO, Truck>()
                .ForMember(dest => dest.TruckId, opt => opt.Ignore())
                .ForMember(dest => dest.RequestId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDated, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore());

            CreateMap<Truck, ResponseTruckDTO>();

            //  Attachment Mappings
            CreateMap<CreateRequestAttachmentDTO, RequestAttachment>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.RequestId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore());

            CreateMap<RequestAttachment, ResponseAttachmentDTO>();
        }
    }
}