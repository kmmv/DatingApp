using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class DatingRepository : IDatingRepository
    {
        private readonly DataContext _context;
        public DatingRepository(DataContext context)
        {
            _context =  context;

        }
        public void Add<T>(T entity) where T : class
        {
            // we are not using async code because when we add something we are not querying or doing anything with the database
            // this is going to be in memory until we actually add an entity
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<Photo> GetMainPhotoForUser(int userId)
        {
            return await _context.Photos.Where(u => u.UserId == userId).
                                        FirstOrDefaultAsync(p=>p.IsMain);
        }

        public async Task<Photo> GetPhoto(int id)
        {
            var photo = await _context.Photos.FirstOrDefaultAsync( p=>p.Id == id );
            return photo;
        }

        public async Task<User> GetUser(int id)
        {
            // include - means include our photos as well (because photos are navigational property 
            // just calling user will not return photos)
            var user = await  _context.Users.Include(p=>p.Photos).FirstOrDefaultAsync( u=>u.Id == id );

            // If cann't find a user or if default user is null  we will return null
            return user;
        }

        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
            // here also include is needed to retrive the photos
            var users =  _context.Users.Include(p=>p.Photos).
                        OrderByDescending(u=>u.LastActive).AsQueryable();;
   
            // filtering parameters
            users = users.Where(u => u.Id != userParams.UserId);
            users = users.Where(u => u.Gender == userParams.Gender);

              if (userParams.MinAge != 18 || userParams.MaxAge != 99)
            {
                // calculate age range
                var minDob = DateTime.Today.AddYears(-userParams.MaxAge - 1);
                var maxDob = DateTime.Today.AddYears(-userParams.MinAge);

                users = users.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);
            }

            if(!string.IsNullOrEmpty(userParams.OrderBy))
            {
                  switch (userParams.OrderBy)
                {
                    case "created":
                        users = users.OrderByDescending(u => u.Created);
                        break;
                    default:
                        users = users.OrderByDescending(u => u.LastActive);
                        break;
                }
            }

            // return as PagedList
            return  await PagedList<User>.CreateAsync(users, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<bool> SaveAll()
        {
            // we want to return true of false  
            // If this returns more than 0 that means the number of saves -0 mean no save
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Like> GetLike(int userId, int recipientId)
        {
            return await _context.Likes.FirstOrDefaultAsync(u =>
                u.LikerId == userId && u.LikeeId == recipientId);
        }

    }
}