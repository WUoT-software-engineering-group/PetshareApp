namespace Petshare.WebAPI.Configuration
{
    internal class DatabaseConfig
    {
        public const string SectionName = "Database";

        public string Server { get; set; } = default!;

        public string DbName { get; set; } = default!;

        public string Login { get; set; } = default!;

        public string Password { get; set; } = default!;
    }
}
