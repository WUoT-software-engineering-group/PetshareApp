using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Petshare.DataPersistence;
using Petshare.DataPersistence.Repositories;
using Petshare.Domain.Repositories.Abstract;
using Petshare.Services;
using Petshare.Services.Abstract;
using Petshare.WebAPI.Configuration;

namespace Petshare.WebAPI
{
    public class Startup
    {
        private readonly ConfigurationsManager _configurationsManager;

        public Startup(IConfiguration configuration)
        {
            _configurationsManager = new ConfigurationsManager(configuration);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<PetshareDbContext>(conf =>
                conf.UseLazyLoadingProxies().UseSqlServer(_configurationsManager.DatabaseConnectionString));

            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
            services.AddScoped<IServiceWrapper, ServiceWrapper>();

            var mappingConfig = TypeAdapterConfig.GlobalSettings;
            mappingConfig.Scan(typeof(Services.Mapping.AssemblyReference).Assembly);
            services.AddSingleton(mappingConfig);
            services.AddScoped<IMapper, ServiceMapper>();

            services.AddControllers()
                .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);

            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();               
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Petshare");
                c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
