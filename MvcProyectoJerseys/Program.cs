using Azure.Security.KeyVault.Secrets;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using MvcProyectoJerseys.Data;
using MvcProyectoJerseys.Helpers;
using MvcProyectoJerseys.Repositories;
using MvcProyectoJerseys.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAzureClients(factory =>
{
    factory.AddSecretClient(builder.Configuration.GetSection("KeyVault"));

});
SecretClient secretClient = builder.Services.BuildServiceProvider().GetService<SecretClient>();

// Add services to the container.
builder.Services.AddAntiforgery();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();
KeyVaultSecret secret= await secretClient.GetSecretAsync("ApiCamisetas");
string connectionString = secret.Value;
builder.Services.AddTransient<ServiceCamisetas>();
builder.Services.AddTransient<RepositoryCamisetas>();
builder.Services.AddDbContext<CamisetasContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddSingleton<HelperPathProvider>();


KeyVaultSecret storageSecret = await secretClient.GetSecretAsync("StorageAccount");
string storage = storageSecret.Value;
BlobServiceClient blobServiceClient = new BlobServiceClient(storage);
builder.Services.AddTransient<BlobServiceClient>(x => blobServiceClient);
builder.Services.AddTransient<ServiceStorageBlobs>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultSignInScheme=CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme=CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme=CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie();
builder.Services.AddControllersWithViews(options => options.EnableEndpointRouting=false).AddSessionStateTempDataProvider();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
//app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

//app.MapStaticAssets();
app.UseSession();

app.UseMvc(routes =>
{
    routes.MapRoute
    (name: "default",
    template: "{controller=Home}/{action=Index}/{id?}");
});

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}")
//    .WithStaticAssets();


app.Run();
