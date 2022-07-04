using CovacRegistration.Server;
using CovacRegistration.Server.Interfaces;
using CovacRegistration.Server.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddControllersWithViews()
    .AddJsonOptions(o =>
        {
            o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            o.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });
builder.Services.AddRazorPages();
builder.Services.AddMemoryCache();

builder.Services.AddDbContext<CovacRegistrationDbContext>(opt =>
    opt.UseInMemoryDatabase("CovacRegistration"));
builder.Services.AddScoped<IRegistrationService, RegistrationService>();
builder.Services.AddScoped<IScheduleService, ScheduleService>();
builder.Services.AddScoped<IVaccineService, VaccineService>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "CovacRegistration", Version = "v1" });
});


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    //3. Get the instance of CovacRegistrationDbContext in our services layer
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<CovacRegistrationDbContext>();

    //4. Call the DataGenerator to create sample data
    DataGenerator.Initialize(services);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DemoApi v1"));
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
