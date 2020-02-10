using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SehirRehberi.API.Models;
using SehirRehberi.API.Repositories;
using SehirRehberi.API.ViewModels;

namespace SehirRehberi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly IAppRepository _appRepository;
        private readonly IMapper _mapper;
        public CitiesController(IAppRepository appRepository, IMapper mapper)
        {
            _appRepository = appRepository;
            _mapper = mapper;
        }

        public ActionResult GetCities()
        {
            var cities = _appRepository.GetCities();/*.Select(i => new CityViewModel { Description = i.Description, Id = i.Id, Name = i.Name, PhotoUrl = i.Photos.FirstOrDefault(a => a.IsMain == true).Url }).ToList();*/
            var response = _mapper.Map<List<CityViewModel>>(cities);
            return Ok(response);
        }

        [HttpPost]
        [Route("create")]
        public ActionResult PostCity([FromBody] City city)
        {
            if (ModelState.IsValid)
            {
                _appRepository.Add<City>(city);
                if (_appRepository.SaveAll())
                {
                    return Ok("Şehir başarıyla eklendi.");
                }
                return BadRequest("Bir hata oluştu.");
            }
            return BadRequest(ModelState);
        }
        [HttpGet]
        [Route("detail")]
        public ActionResult GetCityById(int id)
        {
            var city = _appRepository.GetCityById(id);/*.Select(i => new CityViewModel { Description = i.Description, Id = i.Id, Name = i.Name, PhotoUrl = i.Photos.FirstOrDefault(a => a.IsMain == true).Url }).ToList();*/
            var response = _mapper.Map<CityDetailViewModel>(city);
            return Ok(response);
        }
    }
}