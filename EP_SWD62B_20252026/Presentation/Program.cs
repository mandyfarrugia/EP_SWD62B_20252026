using DataAccess.Context;
using DataAccess.Repositories;
using DataAccess.Services;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Presentation.ActionFilters;
using Presentation.Factory;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ShoppingCartDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<CustomUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ShoppingCartDbContext>();
builder.Services.AddControllersWithViews();

/* The below line will register the service BooksRepository with a list of known services the CLR can instantiate.
 * BooksRepository is also referred to as a service. */
builder.Services.AddScoped(typeof(CategoriesRepository));
//builder.Services.AddScoped(typeof(BooksRepository));
builder.Services.AddScoped(typeof(JournalsRepository));

builder.Services.AddScoped(typeof(BookFactory));

/* KeyedScoped will allow us to use both implementations within the same controller. */
builder.Services.AddKeyedScoped(typeof(IBooksRepository), "db", typeof(BooksRepository));
builder.Services.AddKeyedScoped(typeof(IBooksRepository), "file", typeof(BooksFileRepository));

builder.Services.AddScoped(typeof(OrdersRepository));
builder.Services.AddScoped(typeof(FilterKeywordActionFilter));

//builder.Services.AddScoped<ICalculatingTotal, NoPromotion>();

//Switch between different promotions according to a setting in the appsettings.json.
string promotion = builder.Configuration.GetValue<string>("promotion"); //Searches within appsettings.json and looks for a key-value pair with the key titled promotion, then fetches its respective value.

if (!string.IsNullOrEmpty(promotion))
{
    if (promotion.ToLower().Equals("blackfriday", StringComparison.OrdinalIgnoreCase))
    {
        //Prevent Dependency Injection from auto-wiring, instead instruct it to register the IBooksRepository with the key "db". Otherwise, Dependency Injection will have no idea which concrete repository to use and will throw an exception.
        builder.Services.AddScoped<ICalculatingTotal, BlackFridayPromotion>(
            serviceProvider => new BlackFridayPromotion(serviceProvider.GetRequiredKeyedService<IBooksRepository>("db")
            ));
    }
    else
    {
        //The problem has happening over here because a concrete implementation was not being registered, leaving it to Dependency Injection to try and find the type of IBooksRepository to register. It cannot guess the key to use, hence throws the "Unable to resolve service for type 'IBooksRepository'" error.
        builder.Services.AddScoped<ICalculatingTotal, NoPromotion>( 
            serviceProvider => new NoPromotion(serviceProvider.GetRequiredKeyedService<IBooksRepository>("db"))
        );
    }
}
else
{
    //Ditto.
    builder.Services.AddScoped<ICalculatingTotal, NoPromotion>(
        serviceProvider => new NoPromotion(serviceProvider.GetRequiredKeyedService<IBooksRepository>("db"))
    ); //Serves as a fallback.
}

builder.Services.AddScoped<ICalculatingTotal>(serviceProvider =>
{
    string promotion = builder.Configuration.GetValue<string>("promotion");

    IBooksRepository concreteDatabaseRepositoy = serviceProvider.GetRequiredKeyedService<IBooksRepository>("db");

    if (!string.IsNullOrEmpty(promotion) &&
        promotion.Equals("blackfriday", StringComparison.OrdinalIgnoreCase))
    {
        return new BlackFridayPromotion(concreteDatabaseRepositoy);
    }

    return new NoPromotion(concreteDatabaseRepositoy);
});

var log = new LoggerConfiguration().WriteTo.File(
               "logs/log.txt",
               rollingInterval: RollingInterval.Day,
               restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
               .CreateLogger();

builder.Logging.AddSerilog(log);

/* Why do you go for Scoped/Transient/Singleton? 
 * - Scoped = It will create a new instance per request per user. 
 * - Transient = It will create a new instance per call per request per user. 
 * - Singleton = It will create a new instance per application. */

/* Property Injection in action: 
 * BooksController controller = new BooksController(null);
 * controller._booksRepository = (BooksRepository)app.Services.GetService(typeof(BooksRepository)); */

builder.Services.AddRazorPages();

var app = builder.Build();

app.UseDeveloperExceptionPage();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    //The default HSTS value is 30 days.You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
