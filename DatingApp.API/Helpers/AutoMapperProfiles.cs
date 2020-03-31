using System.Linq;
using AutoMapper;
using DatingApp.API.Dtos;
using DatingApp.API.Models;

namespace DatingApp.API.Helpers
{
    public class AutoMapperProfiles: Profile
    {
        
       public AutoMapperProfiles()
       {
           CreateMap<User, UserForListDto>()
                    // dest is the photoURL property of the UserListDto where we want the value
                    .ForMember(dest =>dest.PhotoUrl, opt => 
                                // Map from is from the photos propery of our users and (users's main photo) the url of the photo
                                opt.MapFrom(src=> src.Photos.FirstOrDefault(p =>p.IsMain).Url))
                                // CalculateAge is defined on the Extensions inside Helper folder
                    .ForMember(dest =>dest.Age, opt => 
                                opt.MapFrom(src=> src.DateOfBirth.CalculateAge()));

           CreateMap<User, UserForDetailedDto>()
                    .ForMember(dest =>dest.PhotoUrl, opt => 
                                opt.MapFrom(src=> src.Photos.FirstOrDefault(p =>p.IsMain).Url))
                    .ForMember(dest =>dest.Age, opt => 
                                opt.MapFrom(src=> src.DateOfBirth.CalculateAge()));

          


           CreateMap<Photo, PhotosForDetailedDto>();  
             // the following map is used to persist changes on the member update user 
           CreateMap<UserForUpdateDto, User>();     
       }
    }
}