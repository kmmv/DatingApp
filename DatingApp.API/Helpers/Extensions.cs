using System;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DatingApp.API.Helpers
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        // extension method to add pagination
        public static void AddPagination(this HttpResponse response, 
                                        int currentPage, int itemsPerPage, int totalItems, int totalPages)
        {
            var paginationHeader = new PaginationHeader(currentPage, itemsPerPage,  totalItems, totalPages);
            
            //we need to send camelCase instead of TitleCase
            var camelCaseFormatter = new JsonSerializerSettings();
            camelCaseFormatter.ContractResolver =  new CamelCasePropertyNamesContractResolver();
           // Add header, SerializeObject will covert the object to JSON formatted object
            response.Headers.Add("Pagination", JsonConvert.SerializeObject(paginationHeader, camelCaseFormatter));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");

        }

        // extension method to calculate date 
       public static int CalculateAge(this DateTime theDatetime)
        {
            var age = DateTime.Today.Year - theDatetime.Year; 
            // if the birthday has passed take off an year
            if (theDatetime.AddYears(age) > DateTime.Today)
                age--;

            return age;
        }

    }
}