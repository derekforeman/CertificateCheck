using CertificateCheck.Base.Models;
using CertificateCheck.Base.Services;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<CertService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/certcheck/{domain}", (CertService certService, string domain) =>
{
    var deets =  certService.GetTest(domain);
    return deets;
}).WithTags("Get");

app.MapGet("/certcheck/", ([FromBody] string[] domains) =>
{


}).WithTags("Get");


app.Run();
