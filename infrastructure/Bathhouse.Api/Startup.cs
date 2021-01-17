using Bathhouse.Memory;
using Bathhouse.Memory.Repositories;
using Bathhouse.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Bathhouse.Entities;
using System.IO;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Bathhouse.Api
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

      services.AddControllers()
              .AddJsonOptions(options =>
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", 
          new OpenApiInfo { 
            Title = "Bathhouse.Api", 
            Version = "v1",
            Description = "An API to Bathhouse",
            Contact = new OpenApiContact
            {
              Name = "Alex Potapchuk",
              Email = "a.v.potapchuk@yandex.ru"
            },
            License = new OpenApiLicense
            {
              Name = "Apache 2.0",
              Url = new Uri("http://www.apache.org/licenses/LICENSE-2.0.html")
            }
          });

        

        // Set the comments path for the Swagger JSON and UI.
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);

        // Set the comments path for the Swagger JSON and UI.
        var xmlFileDTO = $"Bathhouse.xml";
        var xmlPathDTO = Path.Combine(AppContext.BaseDirectory, xmlFileDTO);
        c.IncludeXmlComments(xmlPathDTO);
      });


      services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


      services.AddSingleton<ICRUDRepository<Office>, MemoryBaseCRUDRepository<Office>>();
      services.AddSingleton<ICRUDRepository<Employee>, MemoryBaseCRUDRepository<Employee>>();
      services.AddSingleton<ICRUDRepository<Client>, MemoryBaseCRUDRepository<Client>>();
      services.AddSingleton<ICRUDRepository<WorkItem>, MemoryBaseCRUDRepository<WorkItem>>();
      services.AddSingleton<ICRUDRepository<Survey>, MemoryBaseCRUDRepository<Survey>>();

      services.AddCors();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
          c.SwaggerEndpoint("/swagger/v1/swagger.json", "Bathhouse.Api v1");
        }
        );
      }

      app.UseHttpsRedirection();

      app.UseRouting();

      app.UseCors(policy =>
        policy
        .AllowAnyMethod()
        .AllowAnyOrigin()
        .AllowAnyHeader());

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });


    }
  }
}
