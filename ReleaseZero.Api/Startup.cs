using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using ReleaseZero.Api.Infrastructure;
using ReleaseZero.Api.Swagger;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Swashbuckle.AspNetCore.Swagger;

namespace ReleaseZero.Api
{
    /// <summary>
    /// Startup.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:ReleaseZero.Api.Startup"/> class.
        /// </summary>
        /// <param name="env">Env.</param>
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
	            .MinimumLevel.Override("System", LogEventLevel.Warning)
                .ReadFrom.Configuration(Configuration)
                .WriteTo.Console(new CompactJsonFormatter())
                .CreateLogger();
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        public IConfigurationRoot Configuration { get; }

		/// <summary>
		/// This method gets called by the runtime. Use this method to add services to the container.
		/// </summary>
		/// <param name="services">Services.</param>
		public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<LettersContext>(opt => opt.UseInMemoryDatabase(databaseName: "LettersDb"));

            services.AddMvc(options => options.AddMetricsResourceFilter())
                    .AddJsonOptions(jsonOptions => jsonOptions.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddApiVersioning(options => {
                options.ApiVersionReader = new QueryStringOrHeaderApiVersionReader("X-LK-API-VERSION");
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });

            services.AddMetrics()
                    .AddJsonSerialization()
                    .AddHealthChecks()
                    .AddMetricsMiddleware();

			services.AddSwaggerGen(c =>
			{
				c.DocInclusionPredicate((docName, apiDesc) =>
				{
					var model = apiDesc.ActionDescriptor.GetProperty<ApiVersionModel>();
					switch (model)
					{
						case ApiVersionModel _ when model.IsApiVersionNeutral: return true;
						case ApiVersionModel _ when model.DeclaredApiVersions.Any():
							return model.DeclaredApiVersions
										.Any(apiVersion => apiVersion.ToString() == docName);
						case ApiVersionModel _ when model.ImplementedApiVersions.Any():
							return model.ImplementedApiVersions
										.Any(apiVersion => apiVersion.ToString() == docName);
						default: return false;
					}
				});


				c.SwaggerDoc("1.0",
					new Info
					{
						Title = "Release Zero API",
						Version = "1.0",
						Description = "Release Zero v1",
						TermsOfService = "None",
						Contact = new Contact { Name = "Chris Gundersen", Email = "chris.gundersen@leankit.com", Url = "https://leankit.com" },
						License = new License { Name = "Subscription Agreement", Url = "http://info.leankit.com/subscription-agreement" }
					}
				);
				c.SwaggerDoc("2.0",
					new Info
					{
						Title = "Release Zero API",
						Version = "2.0",
						Description = "Release Zero v2",
						TermsOfService = "None",
						Contact = new Contact { Name = "Chris Gundersen", Email = "chris.gundersen@leankit.com", Url = "https://leankit.com" },
						License = new License { Name = "Subscription Agreement", Url = "http://info.leankit.com/subscription-agreement" }
					}
				);

				c.IncludeXmlComments(Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "ReleaseZero.Api.xml"));

				c.DocumentFilter<SetVersionInPaths>();
				c.OperationFilter<RemoveVersionParameters>();
			});
        }

		/// <summary>
		/// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		/// </summary>
		/// <returns>The configure.</returns>
		/// <param name="app">App.</param>
		/// <param name="env">Env.</param>
		/// <param name="loggerFactory">Logger factory.</param>
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            loggerFactory.AddSerilog();

            app.UseMetrics();

            app.UseStaticFiles();

            var context = app.ApplicationServices.GetService<LettersContext>();
            AddTestData(context);

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            app.UseMvcWithDefaultRoute();

			//app.UseApiVersioning();

			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/1.0/swagger.json", "V1 Docs");
				c.SwaggerEndpoint("/swagger/2.0/swagger.json", "V2 Docs");
			});
        }

		private static void AddTestData(LettersContext context)
		{
            if (!context.Letters.Any())
			    context.Letters.AddRange(Helpers.GetLetterArray());

			context.SaveChanges();
		}
    }
}
