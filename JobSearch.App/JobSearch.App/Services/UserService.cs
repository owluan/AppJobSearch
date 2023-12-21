using JobSearch.App.Models;
using JobSearch.Domain.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace JobSearch.App.Services
{
    public class UserService : Service
    {
        public async Task<ResponseService<User>> GetUser(string email, string password)
        {
            HttpResponseMessage response = await _client.GetAsync($"{BaseApiUrl}/api/users?email={email}&password={password}");

            ResponseService<User> responseService = new ResponseService<User>();

            responseService.IsSuccess = response.IsSuccessStatusCode;
            responseService.StatusCode = (int)response.StatusCode;

            if (response.IsSuccessStatusCode)
            {
                responseService.Data = await response.Content.ReadAsAsync<User>();
            }
            else
            {
                string problemResponse = await response.Content.ReadAsStringAsync();
                var erros = JsonConvert.DeserializeObject<ResponseService<User>>(problemResponse);
                responseService.Errors = erros.Errors;
            }
            return responseService;
        }

        public async Task<ResponseService<User>> AddUser(User user)
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync($"{BaseApiUrl}/api/users", user);

            ResponseService<User> responseService = new ResponseService<User>();

            responseService.IsSuccess = response.IsSuccessStatusCode;
            responseService.StatusCode = (int)response.StatusCode;

            if (response.IsSuccessStatusCode)
            {
                responseService.Data = await response.Content.ReadAsAsync<User>();
            }
            else
            {
                string problemResponse = await response.Content.ReadAsStringAsync();
                var erros = JsonConvert.DeserializeObject<ResponseService<User>>(problemResponse);
                responseService.Errors = erros.Errors;
            }
            return responseService;
        }
    }
}
