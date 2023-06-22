namespace backend.Config;
    public class AzureStorageOptions
    {   
        public const string AzureStorageConfig = "AzureStorageConfig";
        public string AccountName { get; set; }
        public string AccountKey { get; set; }
        public string ImageContainer { get; set; }
        public string ThumbnailContainer { get; set; }
    }
