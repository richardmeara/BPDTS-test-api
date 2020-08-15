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
                var londonUsers = await _mainService.GetUsersByCity("London");
                if (londonUsers == null)
                {
                    return NotFound();
                }

                var allusers = await _mainService.GetUsers();
                if (allusers == null)
                {
                    return NotFound();
                }
                List<User> usersWithin60Miles = new List<User>();
                GeoCoordinate londonCoordinates = new GeoCoordinate(LondonLatitude, LondonLongitude);

                //merge
                foreach (var usr in allusers)
                {
                    double castLat = double.Parse(usr.latitude);
                    double castLong = double.Parse(usr.longitude);
                    //Only add valid users with correct coordinates
                    if (castLat >= LatitudeMinimum && castLat <= LatitudeMaximum && castLong >= LongitudeMinimum && castLong <= LongitudeMaximum)
                    {
                        //Get the user location from API and compare against central London coordinates
                        GeoCoordinate userLocation = new GeoCoordinate(double.Parse(usr.latitude), double.Parse(usr.longitude));
                        double distanceInMetres = userLocation.GetDistanceTo(londonCoordinates);
                        double distanceInMiles = 0;
                        //if the distance is valid
                        if (distanceInMetres > 0)
                        {
                            //convert the metres to miles
                            distanceInMiles = (distanceInMetres * MetresToMiles);
                            if (distanceInMiles <= DistanceToLondonRequirement)
                            {
                                usersWithin60Miles.Add(usr);
                            }
                        }
                    }
                }

                //combine the users from both lists and return
                var totalUsers = londonUsers.Concat(usersWithin60Miles).ToList();

                return Ok(totalUsers);
            }
            catch (Exception ex)
            {
                _logger.LogError($"API: Exception thrown when retreiving londons users: {ex}");
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("users/london/citynameonly")]
        public async Task<IActionResult> GetLondonUsersByCityName()
        {
            try
            {
                var londonUsers = await _mainService.GetUsersByCity("London");
                if (londonUsers == null)
                {
                    return NotFound();
                }

                //simply return users by city name of London
                return Ok(londonUsers);
            }
            catch (Exception ex)
            {
                _logger.LogError($"API: Exception thrown when retreiving londons users: {ex}");
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("users/london/coordinatesonly")]
        public async Task<IActionResult> GetLondonUsersByCoordinates()
        {
            try
            {
                var allusers = await _mainService.GetUsers();
                if (allusers == null)
                {
                    return NotFound();
                }
                List<User> usersWithin60Miles = new List<User>();
                GeoCoordinate londonCoordinates = new GeoCoordinate(LondonLatitude, LondonLongitude);

                foreach (var usr in allusers)
                {
                    double castLat = double.Parse(usr.latitude);
                    double castLong = double.Parse(usr.longitude);
                    //Only add valid users with correct coordinates
                    if (castLat >= LatitudeMinimum && castLat <= LatitudeMaximum && castLong >= LongitudeMinimum && castLong <= LongitudeMaximum)
                    {
                        //Get the user location from API and compare against central London coordinates
                        GeoCoordinate userLocation = new GeoCoordinate(double.Parse(usr.latitude), double.Parse(usr.longitude));
                        double distanceInMetres = userLocation.GetDistanceTo(londonCoordinates);
                        double distanceInMiles = 0;
                        //if the distance is valid
                        if (distanceInMetres > 0)
                        {
                            //convert the metres to miles
                            distanceInMiles = (distanceInMetres * MetresToMiles);
                            if (distanceInMiles <= DistanceToLondonRequirement)
                            {
                                usersWithin60Miles.Add(usr);
                            }
                        }
                    }
                }

                //return only users within 60 miles of the center of london
                return Ok(usersWithin60Miles);
            }
            catch (Exception ex)
            {
                _logger.LogError($"API: Exception thrown when retreiving londons users: {ex}");
            }

            return BadRequest();
        }
    }
}