using CarBox.Infrastructure;
using DataAccess;
using Serilog;
using Microsoft.EntityFrameworkCore;
using PhysicalPersonHandBook.Services;
using Microsoft.Extensions.Options;
using DataAccess.Entities.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using PhysicalPersonHandbook.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigurationManager configuration = builder.Configuration;
IWebHostEnvironment environment = builder.Environment;

builder.Services.AddDbContext<DefaultDbContext>(opt =>
        opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<DbContext, DefaultDbContext>();

//localization for resources
builder.Services.AddLocalization();

builder.Services.Configure<RequestLocalizationOptions>(options => {
    var cultures = builder.Configuration.GetSection("Cultures");
    var defaultCulture = cultures.GetValue<string>("Default");
    var supportedCultures = cultures.GetSection("Supported").Get<string[]>();

    options 
        .SetDefaultCulture(defaultCulture)
        .AddSupportedCultures(supportedCultures)
        .AddSupportedUICultures(supportedCultures);
});

//builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddTransient<IPersonService, PersonService>();
builder.Services.AddTransient<IFileService, FileService>();
builder.Services.AddAutoMapper(c => c.AddProfile<MappingProfile>(), typeof(Program));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


builder.Services.AddControllers();

builder.Services.AddValidatorsFromAssemblyContaining<PersonValidator>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var _logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(builder.Configuration.GetSection("Serilog"))
                    .WriteTo.File(Path.Combine(environment.ContentRootPath, "Logs/Log.log"), rollingInterval: RollingInterval.Day)
                    .CreateLogger();

builder.Host.UseSerilog(_logger);

var app = builder.Build();

app.UseExceptionMiddleware();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DefaultDbContext>();
    //db.Database.EnsureDeleted();
    if (!db.Database.CanConnect())
        db.Database.Migrate();
}

app.UseRequestLocalization(app.Services.
                            GetRequiredService<IOptions<RequestLocalizationOptions>>()
                            .Value);

app.UseAuthorization();

app.MapControllers();

app.Run();
