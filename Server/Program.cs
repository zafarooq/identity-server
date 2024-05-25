using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Extra;


var seed = args.Contains("/seed");
if (seed)
{
    args = args.Except(new[] { "/seed" }).ToArray();
}


var builder = WebApplication.CreateBuilder(args);
var assembly = typeof(Program).Assembly.GetName().Name;
var defaultConnection = builder.Configuration.GetConnectionString("DefaultConnection");

if (seed)
{
    SeedData.EnsureSeedData(defaultConnection);
}

builder.Services.AddDbContext<AspNetIdentityDbContext>(options =>
{
    options.UseSqlServer(defaultConnection,
        b => b.MigrationsAssembly(assembly));
});


builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AspNetIdentityDbContext>();

builder.Services.AddIdentityServer()
    .AddAspNetIdentity<IdentityUser>()
    .AddConfigurationStore(options =>
    {
        options.ConfigureDbContext = b =>
        b.UseSqlServer(defaultConnection, opt => opt.MigrationsAssembly(assembly));
    })
    .AddOperationalStore(options =>
    {
        options.ConfigureDbContext = b =>
        b.UseSqlServer(defaultConnection, options => options.MigrationsAssembly(assembly));
    })
    .AddDeveloperSigningCredential();

var app = builder.Build();
app.UseStaticFiles();

app.UseIdentityServer();
app.UseAuthorization();
app.UseEndpoints(options =>
{
    options.MapDefaultControllerRoute();
});
app.Run();
