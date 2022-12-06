using GeekShopping.IdentityServer.Configuration;
using GeekShopping.IdentityServer.Entities.Context;
using GeekShopping.IdentityServer.Initializer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<SqlContext>(
options => options
    .UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")
                  ?? throw new InvalidOperationException("Connection string 'SqlServer' not found.")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<SqlContext>()
    .AddDefaultTokenProviders();

IIdentityServerBuilder? identityServerBuilder = builder.Services.AddIdentityServer(options => {
        options.Events.RaiseErrorEvents = true;
        options.Events.RaiseInformationEvents = true;
        options.Events.RaiseFailureEvents = true;
        options.Events.RaiseSuccessEvents = true;
        options.EmitStaticAudienceClaim = true;
    })
    .AddInMemoryIdentityResources(IdentityConfiguration.IdentityResources)
    .AddInMemoryApiScopes(IdentityConfiguration.ApiScopes)
    .AddInMemoryClients(IdentityConfiguration.Clients)
    .AddAspNetIdentity<ApplicationUser>();

builder.Services.AddScoped<IDbInitializer, DbInitializer>();

    identityServerBuilder.AddDeveloperSigningCredential();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()){
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

IDbInitializer? dbInitializer = app.Services.CreateScope().ServiceProvider.GetService<IDbInitializer>();

app.UseHttpsRedirection();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseIdentityServer();
app.UseAuthorization();

dbInitializer?.Initialize();

app.MapControllerRoute(
name: "default",
pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
