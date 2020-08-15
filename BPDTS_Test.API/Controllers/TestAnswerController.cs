using BPDTS_Test.API.Models;
using BPDTS_Test.API.Models.Interfaces;
using GeoCoordinatePortable;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPDTS_Test.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestAnswerController : ControllerBase
    {
        private readonly ILogger<TestAnswerController> _logger;
        private readonly IBPDTSTestAppService _mainService;
        private const double LongitudeMinimum = -180;
        private const double LongitudeMaximum = 180;
        private const double LatitudeMinimum = -90;
        private const double LatitudeMaximum = 90;
        private const double LondonLatitude = 51.5074;
        private const double LondonLongitude = 0.1277;
        private const double MetresToMiles = 0.00062137;
        private const double DistanceToLondonRequirement = 60;
        private const string LondonCityName = "London";

        public TestAnswerController(ILogger<TestAnswerController> logger, IBPDTSTestAppService mainService)
        {
            _logger = logger;
            _mainService = mainService;
        }

        [HttpGet]
        [Route("users/london")]
        public async Task<IActionResult> GetLondonUsersByCityNameAndCoordinates()
        {
            try
            {
                List<User> londonUsers = await _mainService.GetUsersByCity(LondonCityName);
                if (londonUsers == null)
                {
                    return NotFound();
                }

                List<User> usersWithinLondonLimit = await GetUsersByLondonProximity();
                if (usersWithinLondonLimit == null)
                {
                    return NotFound();
                }

                //combine the users from both lists and return
                List<User> totalUsers = londonUsers.Concat(usersWithinLondonLimit).ToList();

                return Ok(totalUsers);
            }
            catch (Exception ex)
            {
                _logger.LogError($"API: Exception thrown when retreiving londons users by city name and coordinates: {ex}");
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("users/london/citynameonly")]
        public async Task<IActionResult> GetLondonUsersByCityName()
        {
            try
            {
                //simply return users by city name of London
                List<User> londonUsers = await _mainService.GetUsersByCity(LondonCityName);
                if (londonUsers == null)
                {
                    return NotFound();
                }

                return Ok(londonUsers);
            }
            catch (Exception ex)
            {
                _logger.LogError($"API: Exception thrown when retreiving londons users by city name: {ex}");
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("users/london/coordinatesonly")]
        public async Task<IActionResult> GetLondonUsersByCoordinates()
        {
            try
            {
                //return only users within 60 miles of the center of london
                List<User> usersWithinLondonLimit = await GetUsersByLondonProximity();
                if (usersWithinLondonLimit == null)
                {
                    return NotFound();
                }

                return Ok(usersWithinLondonLimit);
            }
            catch (Exception ex)
            {
                _logger.LogError($"API: Exception thrown when retreiving londons users by coordinates: {ex}");
            }

            return BadRequest();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<List<User>> GetUsersByLondonProximity()
        {
            var allUsers = await _mainService.GetUsers();
            if (allUsers == null)
            {
                return null;
            }
            List<User> usersWithinLondonLimit = new List<User>();
            GeoCoordinate londonCoordinates = new GeoCoordinate(LondonLatitude, LondonLongitude);

            //Loop over every user
            foreach (var usr in allUsers)
            {
                //If their latitude and longitudes are valid doubles continue
                if (double.TryParse(usr.latitude, out double castLat) && double.TryParse(usr.longitude, out double castLong))
                {
                    //Only add valid users with correct coordinates
                    if (castLat >= LatitudeMinimum && castLat <= LatitudeMaximum && castLong >= LongitudeMinimum && castLong <= LongitudeMaximum)
                    {
                        //Get the user location from API and compare against central London coordinates
                        GeoCoordinate userLocation = new GeoCoordinate(castLat, castLong);
                        double distanceInMetres = userLocation.GetDistanceTo(londonCoordinates);
                        //if the distance is valid
                        if (distanceInMetres > 0)
                        {
                            //convert the metres to miles
                            double distanceInMiles = (distanceInMetres * MetresToMiles);
                            if (distanceInMiles <= DistanceToLondonRequirement)
                            {
                                usersWithinLondonLimit.Add(usr);
                            }
                        }
                    }
                }
            }

            return usersWithinLondonLimit;
        }
    }
}