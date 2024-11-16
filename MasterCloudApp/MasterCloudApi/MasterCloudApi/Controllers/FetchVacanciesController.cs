using MasterCloudApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace MasterCloudApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FetchVacanciesController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post(int pageCount, int vacanciesPerPage)
        {
            try
            {
                ApiService apiService = new ApiService();
                await apiService.FetchVacancies(pageCount, vacanciesPerPage);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
