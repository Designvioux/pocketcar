using CarRentalApp.Data;
using CarRentalApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CarRentalApp.Services;
using Microsoft.Extensions.FileProviders;
using QuestPDF.Infrastructure;
using QuestPDF;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.HttpOverrides;


var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddControllers()
    .AddJsonOptions(x =>
        x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles); ;

//QuestPDF.Settings.License = LicenseType.Community;

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/*builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});*/

builder.Services.AddScoped<EmailService>();
builder.Services.AddSingleton<SmsService>();
builder.Services.AddScoped<AuthService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy => policy.WithOrigins("https://pocketcar.in")
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

/*builder.Services.AddDbContext<WheelzOnDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));*/

try
{
    builder.Services.AddDbContext<WheelzOnDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 41))));
    Console.WriteLine("Connected successfully to MySQL!");
}
catch (Exception ex)
{
    Console.WriteLine($"Connection failed: {ex.Message}");
}


builder.Services.AddDatabaseDeveloperPageExceptionFilter();



//JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["Secret"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"]))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.MapGet("/test-db", async (WheelzOnDbContext db) =>
{
    try
    {
        await db.Database.OpenConnectionAsync();
        return "Database Connection Successful!";
    }
    catch (Exception ex)
    {
        return $"Database Connection Failed: {ex.Message}";
    }
});

app.Use(async (context, next) =>
{
    var host = context.Request.Host;
    var scheme = context.Request.Scheme;
    var path = context.Request.Path;
    var fullUrl = $"{scheme}://{host}{path}";

    Console.WriteLine($"Incoming request: {fullUrl}");

    await next();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // remove after solving problem
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
    RequestPath = ""
});

app.UseRouting();

app.UseCors("AllowAngular");

app.UseAuthentication();

app.UseAuthorization();

/*app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});*/

// Serve Angular frontend
app.UseDefaultFiles();
//app.UseStaticFiles();

// This handles all non-API routes with index.html
/*app.Use(async (context, next) =>
{
    if (!context.Request.Path.StartsWithSegments("/api") &&
        !Path.HasExtension(context.Request.Path.Value))
    {
        context.Request.Path = "/index.html";
    }
    await next();
});*/

//app.UseCors("AllowAll");

app.MapControllers();

var serverAddresses = app.Services.GetService<IServer>()?
    .Features.Get<IServerAddressesFeature>()?.Addresses;

if (serverAddresses != null)
{
    foreach (var address in serverAddresses)
    {
        Console.WriteLine($"Application is listening on: {address}");
    }
}
else
{
    Console.WriteLine("Could not determine the listening address.");
}

app.Run();
