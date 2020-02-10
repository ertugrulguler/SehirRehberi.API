using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SehirRehberi.API.Repositories;
using SehirRehberi.API.ViewModels;

namespace SehirRehberi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly IAppRepository _appRepository;

        public CitiesController(IAppRepository appRepository)
        {
            _appRepository = appRepository;
        }

        public ActionResult GetCities()
        {
            var cities = _appRepository.GetCities().Select(i => new CityViewModel { Description = i.Description, Id = i.Id, Name = i.Name, PhotoUrl = i.Photos.FirstOrDefault(a => a.IsMain == true).Url }).ToList();
            return Ok(cities);
        }
    }
}