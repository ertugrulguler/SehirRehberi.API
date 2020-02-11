using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SehirRehberi.API.Helpers;
using SehirRehberi.API.Models;
using SehirRehberi.API.Repositories;
using SehirRehberi.API.ViewModels;

namespace SehirRehberi.API.Controllers
{
    [Route("api/cities/{cityId}/[controller]")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private IAppRepository _appRepository;
        private IMapper _mapper;
        private IOptions<CloudinarySettings> _cloudinaryConfig;

        private Cloudinary _cloudinary;
        public PhotosController(IAppRepository appRepository, IMapper mapper, IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _appRepository = appRepository;
            _mapper = mapper;
            _cloudinaryConfig = cloudinaryConfig;

            Account account = new Account(_cloudinaryConfig.Value.CloudName, _cloudinaryConfig.Value.ApiKey, _cloudinaryConfig.Value.ApiSecret);
            _cloudinary = new Cloudinary(account);
        }
        [HttpPost]
        public ActionResult AddPhotoForCity(int cityId, [FromBody]PhotoRequestViewModel photoRequest)
        {
            var city = _appRepository.GetCityById(cityId);

            if (city == null)
            {
                return BadRequest("Şehir bulunamadı.");
            }

            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (currentUserId != city.UserId)
            {
                return Unauthorized();
            }

            var file = photoRequest.File;

            var uploadResult = new ImageUploadResult();


            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.FileName, stream)
                    };
                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }

            photoRequest.Url = uploadResult.Uri.ToString();
            photoRequest.PublicId = uploadResult.PublicId;

            var photo = _mapper.Map<Photo>(photoRequest);
            photo.City = city;

            if (!city.Photos.Any(i => i.IsMain))
            {
                photo.IsMain = true;
            }

            city.Photos.Add(photo);

            if (_appRepository.SaveAll())
            {
                var photoResponse = _mapper.Map<PhotoResponseViewModel>(photo);
                return CreatedAtAction("GetPhoto", new { id = photo.Id }, photoResponse);
            }

            return BadRequest("Fotoğraf eklenemedi.");

        }

        [HttpGet("{id}")]
        public ActionResult GetPhoto(int id)
        {
            var photo = _appRepository.GetPhoto(id);
            var response = _mapper.Map<PhotoResponseViewModel>(photo);

            return Ok(response);
        }
    }
}