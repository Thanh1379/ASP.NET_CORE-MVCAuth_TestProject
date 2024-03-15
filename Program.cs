using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using MVCAuth1.Data;
using MVCAuth1.Hubs;
using MVCAuth1.Services;
using Neo4jClient;
using NRedisStack;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

//Add SignalR
builder.Services.AddSignalR();
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
//Add Policy for allow CORS
builder.Services.AddCors(options => {
    options.AddPolicy("CORSPolicy", builder => builder.AllowAnyMethod().AllowAnyHeader().AllowCredentials().SetIsOriginAllowed((hosts) => true));
});
//Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddDbContext<ApplicationDbContext>((options) => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});
builder.Services.AddSingleton<IImagesServeControllerConfiguration>(
    new ImagesServeControllerConfiguration(builder.Configuration.GetSection("StaticFiles:UserStaticFiles:Images"))
    );
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddSignInManager<SignInManager<IdentityUser>>();
//Add Redis
var redisRepoHost = builder.Configuration.GetSection("RedisRepo")?.GetValue<string>("Host");
if(redisRepoHost != null)
{
    builder.Services.AddSingleton<ConnectionMultiplexer>(
        ConnectionMultiplexer.Connect(redisRepoHost)
    );
}
else
{
    throw new Exception("Not Find Redis Repo");
}
builder.Services.Configure<IdentityOptions>((options) => {
    //configure password
    options.Password.RequiredLength = 3;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    
    //configure lockout
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
    options.Lockout.MaxFailedAccessAttempts = 3;
    options.Lockout.AllowedForNewUsers = true;

    //configure user
    options.User.AllowedUserNameCharacters = 
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;
    options.User.RequireUniqueEmail = false;
    
    //configure sign in
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;

});
builder.Services.Configure<SessionOptions>((options) => {
    options.Cookie.Name = "MyCookie";
    options.IdleTimeout = TimeSpan.FromDays(1);
});
builder.Services.ConfigureApplicationCookie((options) => {
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(1);
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/AccessDenied";
    options.SlidingExpiration = true;
});
builder.Services.AddAuthentication().AddGoogle((options) => {
    var googleAuthSetting = builder.Configuration.GetSection("Authentication:Google");
    options.ClientId = googleAuthSetting["ClientId"] ?? "";
    options.ClientSecret = googleAuthSetting["ClientSecret"] ?? "";
    options.CallbackPath = "/GoogleLogin";
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseCors("CORSPolicy");
app.UseHttpsRedirection();
//app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.UseStaticFiles();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapHub<ChatHub>("/Chat");
app.Run();