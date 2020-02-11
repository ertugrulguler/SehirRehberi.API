using AutoMapper;
using SehirRehberi.API.Models;
using SehirRehberi.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SehirRehberi.API.Helpers
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<City, CityViewModel>().ForMember(dest => dest.PhotoUrl, opt =>
               {
                   opt.MapFrom(src => src.Photos.FirstOrDefault(i => i.IsMain == true).Url);
               });

            CreateMap<City, CityDetailViewModel>();
            CreateMap<Photo, PhotoRequestViewModel>();
            CreateMap<PhotoRequestViewModel, PhotoResponseViewModel>();

        }
    }
}
