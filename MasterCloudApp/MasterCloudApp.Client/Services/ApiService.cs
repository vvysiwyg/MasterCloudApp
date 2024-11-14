using Amazon.S3.Transfer;
using Amazon.S3;
using Newtonsoft.Json.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace MasterCloudApp.Client.Services
{
    public class ApiService
    {
        public int PageCount { get; set; }

        public int VacanciesPerPage { get; set; }

        public async Task<string> Start(int pageCount = 2, int vacanciesPerPage = 10)
        {
            PageCount = pageCount;
            VacanciesPerPage = vacanciesPerPage;
            await GetVacanciesJson();
            return string.Empty;
            // return await SendVacanciesJson();
        }

        private async Task GetVacanciesJson()
        {
            for (int page = 0; page < PageCount; page++)
            {
                string json = await GetPageAsync(page, VacanciesPerPage);
                File.WriteAllText(@$"D:\VisualStudioProjects\MasterCloudApp\MasterCloudApp\MasterCloudApp.Client\{page}.json", json);
                await Task.Delay(250);
            }
        }

        private async Task<string> SendVacanciesJson()
        {
            try
            {
                var s3Client = new AmazonS3Client(@"YCAJE6aw0u8b0ZV9_prHlZ3eI", @"YCPzQHf_njUP2EDbYpHuOqTfxm1oE2Cpt3ms6NOh", new AmazonS3Config
                {
                    ServiceURL = "https://s3.yandexcloud.net"
                });

                var fileTransferUtility = new TransferUtility(s3Client);

                for (int i = 0; i < PageCount; i++)
                    await fileTransferUtility.UploadAsync(@$"D:\VisualStudioProjects\MasterCloudApp\MasterCloudApp\MasterCloudApp.Client\{i}.json", "vvysiwyg-b");

                return string.Empty;
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}\nStackTrace: {ex.StackTrace}";
            }
        }

        // Метод для получения страницы вакансий
        private static async Task<string> GetPageAsync(int page, int perPage)
        {
            using var client = new HttpClient();
            var url = $"https://api.hh.ru/vacancies?text=NAME:Программист&area=53&page={page}&per_page={perPage}";
            return await client.GetStringAsync(url);
        }
    }
}
