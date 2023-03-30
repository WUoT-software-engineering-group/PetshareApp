namespace Petshare.WebAPI.Configuration
{
    internal class DatabaseConfig
    {
        public const string SectionName = "Database";

        public string Server { get; set; } = string.Empty;
        public string DbName { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
