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
        options.AccessDeniedPath = "/User/AccesDenied";
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
builder.Services.AddScoped<_ICorrespondance, _CorrespondanceRepo>();
builder.Services.AddScoped<_IContrat, _ContratRepo>();
builder.Services.AddScoped<_IDecision, _DecisionRepo>();
builder.Services.AddScoped<_ICommunique,  _CommuniqueRepo>();
builder.Services.AddScoped<_IAttestation,  _AttestationRepo>();
builder.Services.AddScoped<_IArrete,  _ArreteRepo>();
builder.Services.AddScoped<_IDecret,  _DecretRepo>();
builder.Services.AddScoped<_IEtatPaiement, _EtatPaiementRepo>();
builder.Services.AddScoped<_ICertificat, _CertificatRepo>();
builder.Services.AddScoped<_INoteService, _NoteServiceRepo>();

builder.Services.AddScoped<IChronogramme, ChronogrammeRepo>();
builder.Services.AddScoped<IStructure,  StructureRepo>();
builder.Services.AddScoped<IRapport, RapportRepo>();
builder.Services.AddScoped<ICorrespondance, CorrespondanceRepo>();
builder.Services.AddScoped<IDossier, DossierRepo>();
builder.Services.AddScoped<ISource, SourceRepo>();
builder.Services.AddScoped<IFaculte, FaculteRepo>();
builder.Services.AddScoped<IFilieree, FiliereeRepo>();
builder.Services.AddScoped<IExamen, ExamenRepo>();
builder.Services.AddScoped<IPvExamen, PvExamenRepo>();
builder.Services.AddScoped<IPvCne, PvCneRepo>();
builder.Services.AddScoped<IArrete,  ArreteRepo>();
builder.Services.AddScoped<IListe, ListeRepo>();
builder.Services.AddScoped<IDecharge,  DechargeRepo>();

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
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//fin config security partie 2
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.Run();
