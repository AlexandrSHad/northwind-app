using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc.Formatters;
using Northwind.WebApi.Repositories;
using Packt.Shared;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddNorthwindContext();

builder.Services.AddControllers(options =>
{
    Console.WriteLine("Default output formatters:");
    foreach (IOutputFormatter formatter in options.OutputFormatters)
    {
        OutputFormatter? mediaFormatter = formatter as OutputFormatter;
        if (mediaFormatter is null)
        {
            Console.WriteLine($" {formatter.GetType().Name}");
        }
        else // OutputFormatter class has SupportedMediaTypes
        {
            Console.WriteLine(" {0}, Media types: {1}",
            arg0: mediaFormatter.GetType().Name,
            arg1: string.Join(", ", mediaFormatter.SupportedMediaTypes));
        }
    }
})
.AddXmlDataContractSerializerFormatters()
.AddXmlSerializerFormatters();
builder.Services.AddHttpLogging(options => {
    options.LoggingFields = HttpLoggingFields.All;
    options.RequestBodyLogLimit = 4096;
    options.ResponseBodyLogLimit = 4096;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

builder.Services.AddHealthChecks()
    .AddDbContextCheck<NorthwindContext>()
    .AddSqlite(@"Data Source=..\Database\Northwind.db");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => {
        options.SwaggerEndpoint(
            url: "/swagger/v1/swagger.json",
            name: "Northwind Service API version 1");
        options.SupportedSubmitMethods(new [] { SubmitMethod.Get, SubmitMethod.Post, SubmitMethod.Put, SubmitMethod.Delete });
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseHttpLogging();

app.UseHealthChecks(path: "/health");

app.MapControllers();

app.Run();
