using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ScouTeams.Areas.Identity.Data;
using ScouTeams.Data;

[assembly: HostingStartup(typeof(ScouTeams.Areas.Identity.IdentityHostingStartup))]
namespace ScouTeams.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<ScouTDBContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("ScouTDBContextConnection")));

                services.AddDefaultIdentity<Scout>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                })
                    .AddEntityFrameworkStores<ScouTDBContext>();
            });
        }
    }
}