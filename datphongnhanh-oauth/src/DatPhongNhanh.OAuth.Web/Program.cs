using DatPhongNhanh.OAuth.Web;
using OpenIddict.Validation.AspNetCore;
using Vite.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.ConfigureViteIntegration();
builder.Services.ConfigureEntityFramework(builder.Configuration);
builder.Services.ConfigureIdentity();
builder.Services.ConfigureOpenIddict();
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
});
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/identity/signin";
    options.Cookie.Name = "dpn_auth";
});
builder.Services.AddAntiforgery(options => { 
    options.Cookie.Name = "dpn_xsrf"; 
    options.HeaderName = "X_XSRF_TOKEN"; 
});
builder.Services.AddAuthorization();
builder.Services.AddHostedService<SeedDataWorker>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

    app.UseWebSockets();
    app.UseViteDevelopmentServer(true);
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapStaticAssets();
app.MapControllers();
app.MapRazorPages();
// app.MapAreaControllerRoute(
//     "IdentityArea",
//     "Identity",
//     "Identity/{controller=Index}/{id?}");
app.MapControllerRoute(
        "default",
        "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

await app.RunAsync();