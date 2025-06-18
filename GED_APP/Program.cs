using GED_APP.Data;
using GED_APP.Repository.Implementations;
using GED_APP.Repository.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//debut  config security partie 1
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/User/Login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
         
    });

//fin config security 

//Serializing object to load data after mapping (Eager)
//builder.Services.AddControllers().AddNewtonsoftJson(options =>
//{
//    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
//    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
//});

//builder.Services.AddControllers().AddNewtonsoftJson(options =>
//{
//    options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
//    options.SerializerSettings.Formatting = Formatting.Indented;
//    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
//    options.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
//});
builder.Services.AddMvc(option => option.EnableEndpointRouting = false)
    .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
    .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
//MariaDb ou Mysql configuration
var connectionString = builder.Configuration.GetConnectionString("MysqlConn");
builder.Services.AddDbContext<AppDbContext>(options =>
{
   // options.UseLazyLoadingProxies();
    //.UseLazyLoadingProxies()
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});
// All Interfaces 
builder.Services.AddScoped<ICycle, CycleRepo>();
builder.Services.AddScoped<IFiliere, FiliereRepo>();
builder.Services.AddScoped<IDocument, DocumentRepo>();
builder.Services.AddScoped<IUser, UserRepo>();
builder.Services.AddScoped<IFileUploadService, LocalFileUploadService>(); 
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

//fin config security partie 2
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.Run();
