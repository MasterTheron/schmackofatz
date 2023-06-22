namespace backend.Config;
public class DatabaseConfig 
{
    public const string DatabaseConfigName = "DatabaseConfig";
    public string AccountEndpoint { get; set; }
    public string AccountKey { get; set; }
    public string DatabaseName { get; set; }
}
