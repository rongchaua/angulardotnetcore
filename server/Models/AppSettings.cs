namespace WebApplication.Models
{
    public class AppSettings
    {
        public string CorsAllowedOrigin { get; set; }
        public string AzureStorageName { get; set; }

        public string AzureStorageKey { get; set; }
        public string AzureStorageFileShareName { get; set; }
    }
}