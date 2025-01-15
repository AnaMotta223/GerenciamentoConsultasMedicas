using AppointmentsManager.Application.Services;
using AppointmentsManager.Domain.Interfaces;
using AppointmentsManager.Domain.Services;
using AppointmentsManager.Infrastructure.Repositories;
using Microsoft.OpenApi.Models;
using AppointmentsManager.Utils;
using AppointmentsManager.Domain.ValueObjects;
using AppointmentsManager.Infrastructure.Configuration;
using Dapper.FluentMap;

var builder = WebApplication.CreateBuilder(args);

FluentMapper.Initialize(config =>
{
    config.AddMap(new DoctorMap());
    config.AddMap(new PatientMap());
    config.AddMap(new AppointmentMap());
    config.AddMap(new DateTimeWorkMap());
});
builder.Services.AddControllers();
Dapper.SqlMapper.AddTypeHandler(new EmailTypeHandler());
Dapper.SqlMapper.AddTypeHandler(new CPFTypeHandler());
Dapper.SqlMapper.AddTypeHandler(new RMCTypeHandler());
builder.Services.AddSingleton<PasswordEncrypter>();


builder.Services.AddSingleton<IDatabaseConfig, DatabaseConfig>();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Medical Appointments Manager",
        Description = "An ASP.NET Core Web API for managing medical appointments",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Contact",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "License Apache 2.0",
            Url = new Uri("http://www.apache.org/licenses/LICENSE-2.0.html")
        }
    });
    options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    options.EnableAnnotations();
});
builder.Services.AddScoped<AppointmentService>();
builder.Services.AddScoped<DoctorService>();
builder.Services.AddScoped<PatientService>();
builder.Services.AddScoped<DateTimeWorkService>();
builder.Services.AddScoped<AvailabilityService>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IDateTimeWorkRepository, DateTimeWorkRepository>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("./v1/swagger.json", "com.t2mlab.appointmentmanagerapi.Api v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
