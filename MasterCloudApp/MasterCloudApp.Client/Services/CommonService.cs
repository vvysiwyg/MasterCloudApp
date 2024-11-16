namespace MasterCloudApp.Client.Services
{
    public class CommonService : ICommonService
    {
        private readonly HttpClient _httpclient;

        public CommonService(HttpClient httpclient)
        {
            _httpclient = httpclient;
        }

        public async Task FetchVacancies(int pageCount, int vacanciesPerPage)
        {
            using HttpResponseMessage response =
                await _httpclient.PostAsync($"FetchVacancies?pageCount={pageCount}&vacanciesPerPage={vacanciesPerPage}", null);

            response.EnsureSuccessStatusCode();
        }

        public async Task<string> SendVacanciesJson()
        {
            using HttpResponseMessage response =
                await _httpclient.PostAsync($"SendVacanciesJson", null);

            if(response is null)
            {
                throw new Exception("Ошибка");
            }

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
