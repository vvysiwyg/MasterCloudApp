namespace MasterCloudApp.Client.Services
{
    public interface ICommonService
    {
        public Task FetchVacancies(int pageCount, int vacanciesPerPage);
        public Task<string> SendVacanciesJson();
    }
}
