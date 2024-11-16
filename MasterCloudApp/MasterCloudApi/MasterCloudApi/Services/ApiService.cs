using Amazon.S3.Transfer;
using Amazon.S3;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using MasterCloudApi.DTO;

namespace MasterCloudApi.Services
{
    public class ApiService
    {
        public async Task FetchVacancies(int pageCount = 2, int vacanciesPerPage = 100)
        {
            HashSet<Vacancy> vacancies = new HashSet<Vacancy>();
            for (int page = 0; page < pageCount; page++)
            {
                JArray jarray = await GetPageAsync(page, vacanciesPerPage);
                foreach (JObject jobject in jarray)
                {
                    Vacancy vacancy = new Vacancy();
                    JToken? nameT = jobject.SelectToken("name");
                    JToken? empT = jobject.SelectToken("employer");
                    JToken? salaryT = jobject.SelectToken("salary");
                    JToken? areaT = jobject.SelectToken("area");
                    JToken? experienceT = jobject.SelectToken("experience");

                    vacancy.Name = nameT != null ? nameT.Value<string>() : string.Empty;

                    if (empT != null && empT.HasValues)
                    {
                        bool? AccreditedItEmployer = empT.Value<bool?>("accredited_it_employer");
                        vacancy.AccreditedItEmployer = AccreditedItEmployer.HasValue ? AccreditedItEmployer.Value : false;
                    }

                    if (salaryT != null && salaryT.HasValues)
                    {
                        string currency = salaryT.Value<string>("currency");
                        int? from = salaryT.Value<int?>("from");
                        vacancy.SalaryCurrency = currency ?? string.Empty;
                        vacancy.SalaryFrom = from.HasValue ? from.Value : 0;
                    }

                    if (areaT != null && salaryT.HasValues)
                    {
                        string name = areaT.Value<string>("name");
                        vacancy.AreaName = name ?? string.Empty;
                    }

                    if (experienceT != null && salaryT.HasValues)
                    {
                        string name = experienceT.Value<string>("name");
                        vacancy.ExperienceName = name ?? string.Empty;
                    }

                    vacancies.Add(vacancy);
                }
                await Task.Delay(250);
            }

            File.WriteAllText("data.json", JsonConvert.SerializeObject(vacancies, Formatting.Indented));
        }

        public async Task<string> SendVacanciesJson()
        {
            var s3Client = new AmazonS3Client(@"YCAJE6aw0u8b0ZV9_prHlZ3eI", @"YCPzQHf_njUP2EDbYpHuOqTfxm1oE2Cpt3ms6NOh", new AmazonS3Config
            {
                ServiceURL = "https://s3.yandexcloud.net"
            });

            var fileTransferUtility = new TransferUtility(s3Client);
            await fileTransferUtility.UploadAsync("data.json", "vvysiwyg-b");

            return "Ok";
        }

        // Метод для получения страницы вакансий
        private static async Task<JArray> GetPageAsync(int page, int perPage)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.UserAgent.ParseAdd("HH-User-Agent");
                var url = $"https://api.hh.ru/vacancies?text=NAME:Программист&page={page}&per_page={perPage}";
            var response = await client.GetStringAsync(url);
            return JObject.Parse(response)["items"] as JArray;
        }
    }
}
