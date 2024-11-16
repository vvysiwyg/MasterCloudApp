namespace MasterCloudApi.DTO
{
    public class Vacancy
    {
        public string Name { get; set; } = string.Empty;
        public bool AccreditedItEmployer { get; set; } = false;
        public int SalaryFrom { get; set; } = 0;
        public string SalaryCurrency { get; set; } = string.Empty;
        public string AreaName { get; set; } = string.Empty;
        public string ExperienceName { get; set; } = string.Empty;
    }
}
