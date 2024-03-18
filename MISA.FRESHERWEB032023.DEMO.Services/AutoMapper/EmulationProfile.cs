using AutoMapper;
using MISA.FRESHERWEB032023.BL.Dto;
using MISA.FRESHERWEB032023.DL.Entity.Emualtion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.FRESHERWEB032023.BL.AutoMapper
{
    public class EmulationProfile : Profile
    {
        public EmulationProfile() {
            CreateMap<EmulationBase, EmulationDto>();
        }
    }
}
