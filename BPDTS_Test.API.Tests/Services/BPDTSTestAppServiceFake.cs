using BPDTS_Test.API.Models;
using BPDTS_Test.API.Models.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BPDTS_Test.API.Tests.Services
{
    public class BPDTSTestAppServiceFake : IBPDTSTestAppService
    {
        public List<User> MockUsers { get; set; }
        public List<User> MockLondonUsers { get; set; }

        public BPDTSTestAppServiceFake()
        {
            MockUsers = new List<User>()
            {
                new User { id = "1",
                email = "testuser_1@richardmeara.com",
                first_name = "richard",
                last_name = "meara",
                ip_address = "192.168.0.1",
                latitude = "51.4671",
                longitude = "-0.1202" },
                new User { id = "2",
                email = "testuser_3@richardmeara.com",
                first_name = "joe",
                last_name = "bloggs",
                ip_address = "192.168.0.2",
                latitude = "26.0000",
                longitude = "72.0000" },
                new User { id = "3",
                email = "testuser_3@richardmeara.com",
                first_name = "jane",
                last_name = "doe",
                ip_address = "192.168.0.3",
                latitude = "27.0000",
                longitude = "73.0000" },
            };

            MockLondonUsers = new List<User>()
            {
                new User { id = "4",
                email = "testuser_4@richardmeara.com",
                first_name = "jack",
                last_name = "ryan",
                ip_address = "192.168.0.4",
                latitude = "51.4653",
                longitude = "-0.0532" }
            };
        }

        public async Task<List<User>> GetUsers()
        {
            return MockUsers;
        }

        public async Task<List<User>> GetUsersByCity(string city)
        {
            return MockLondonUsers;
        }
    }
}