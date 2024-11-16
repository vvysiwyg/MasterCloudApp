using MasterCloudApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace MasterCloudApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SendVacanciesJsonController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            try
            {
                ApiService apiService = new ApiService();
                return Ok(await apiService.SendVacanciesJson());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
