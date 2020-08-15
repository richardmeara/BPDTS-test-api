using System.Collections.Generic;
using System.Threading.Tasks;

namespace BPDTS_Test.API.Models.Interfaces
{
    public interface IBPDTSTestAppService
    {
        Task<List<User>> GetUsers();

        Task<List<User>> GetUsersByCity(string city);
    }
}