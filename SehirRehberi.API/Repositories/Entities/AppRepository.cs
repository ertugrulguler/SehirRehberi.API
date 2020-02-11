using Microsoft.EntityFrameworkCore;
using SehirRehberi.API.Context;
using SehirRehberi.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SehirRehberi.API.Repositories.Entities
{
    public class AppRepository : IAppRepository
    {
        private DataContext _db;
        public AppRepository(DataContext db)
        {
            _db = db;
        }

        public void Add<T>(T entity) where T : class
        {
            _db.Add(entity);
            SaveAll();

        }

        public void Delete<T>(T entity) where T : class
        {
            _db.Remove(entity);
            SaveAll();
        }

        public List<City> GetCities()
        {
            var cities = _db.Cities.Include(i => i.Photos).ToList();
            return cities;
        }

        public City GetCityById(int cityId)
        {
            var city = _db.Cities.Include(i => i.Photos).FirstOrDefault(i => i.Id == cityId);
            return city;
        }

        public Photo GetPhoto(int id)
        {
            var photo = _db.Photos.FirstOrDefault(i => i.Id == id);
            return photo;
        }

        public List<Photo> GetPhotosByCityId(int id)
        {
            var photos = _db.Photos.Where(i => i.CityId == id).ToList();
            return photos;
        }

        public bool SaveAll()
        {
            return _db.SaveChanges() > 0;
        }
    }
}
