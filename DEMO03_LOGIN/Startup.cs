using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace DEMO03_LOGIN
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(option => option.EnableEndpointRouting = false);
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseStaticFiles();//facilita el uso de los archivos de web root ( wwwroot por defecto)
            app.UseDeveloperExceptionPage();//captura las instancias de excepción y genera respuesta de error
            app.UseSession();
            app.UseMvc(router =>
            {
                router.MapRoute(
                name: "default",
                template: "{controller=Login}/{action=Login}/{id?}");
            });
        }
    }
}