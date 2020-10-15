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
                .ForMember(dest => dest.Measurement, src => src.MapFrom(src => src.Measurement));
        }
    }
}
