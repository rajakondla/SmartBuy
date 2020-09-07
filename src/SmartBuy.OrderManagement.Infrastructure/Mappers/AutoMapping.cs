using AutoMapper;
using SmartBuy.OrderManagement.Domain;
using SmartBuy.OrderManagement.Infrastructure.Abstractions.DTOs;

namespace SmartBuy.OrderManagement.Infrastructure.Mappers
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<Tank, TankDetail>()
                .ForMember(dest => dest.Bottom, src => src.MapFrom(src => src.Measurement.Bottom))
                .ForMember(dest => dest.Top, src => src.MapFrom(src => src.Measurement.Top))
                .ForMember(dest => dest.Quantity, src => src.MapFrom(src => src.Measurement.Quantity));
        }
    }
}
