using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DDNS {
    public class Startup {
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {
            services.AddControllers ();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IWebHostEnvironment env) {
            var a = EnvironmentHelper.Arguments;

            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            }
            app.UseHttpsRedirection ();

            app.UseRouting ();

            app.UseAuthorization ();

            app.UseEndpoints (endpoints => {
                endpoints.MapControllers ();
            });
        }

        public static class EnvironmentHelper {

            private static List<string> EnvironmentArguments = new List<string> () { "traefikhostName", "toeknclodflare", "email", "ASPNETCORE_ENVIRONMENT" };
            private static Dictionary<string, string> _arguments = new Dictionary<string, string> ();
            public static Dictionary<string, string> Arguments {
                get {
                    bool argumentsExist = _arguments != null && _arguments.Any ();
                    if (!argumentsExist) {
                        IDictionary environmentVariables = Environment.GetEnvironmentVariables ();
                        foreach (var item in EnvironmentArguments) {

                            if (!environmentVariables.Contains (item)) {
                                throw new Exception ("Environment Arguments " + item + "do not exist");
                            }
                            _arguments.Add (item, environmentVariables[item] as string);
                        }
                    }
                    return _arguments;
                }
            }
        }
    }
}