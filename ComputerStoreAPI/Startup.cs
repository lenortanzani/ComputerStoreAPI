using ComputerStoreAPI.Data;
using ComputerStoreAPI.RabbitMQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerStoreAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //Измененная строка
            services.AddControllers()
                .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ComputerStoreAPI", Version = "v1" });
            });           

            services.AddDbContext<ComputerStoreDbContext>(options => options.UseNpgsql(Configuration.GetConnectionString("ElephantDb")));

            // AddScoped - экземпляр класса будет создаваться на весь текущий запрос,
            // для всего запроса создается один экземпляр, запрос отработал - объект удаляется
            // Если кто-либо ожидает получить IDbService, давай ему DbService
            services.AddScoped<IDbService, DbService>();

            // через Dependency Injection внедряем класс который не реализует никакого интерфейса,
            // <IDbService, DbService> - напротив, тут класс реализует интерфейс
            //services.AddScoped<ComputerStoreDbContext>(); замена на 41 строке

            services.AddScoped<IRabbitMqService, RabbitMqService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ComputerStoreAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
