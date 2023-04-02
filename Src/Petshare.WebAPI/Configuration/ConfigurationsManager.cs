﻿using Petshare.Services.Abstract;

namespace Petshare.WebAPI.Configuration;

public class ConfigurationsManager : IServicesConfiguration
{
    public readonly IConfiguration Configuration;

    public ConfigurationsManager(IConfiguration configuration)
    {
        Configuration = configuration;
        DatabaseConnectionString = Configuration.GetValue<string>("SSDatabaseConnectionString")!;
    }

    public string DatabaseConnectionString { get; }
}