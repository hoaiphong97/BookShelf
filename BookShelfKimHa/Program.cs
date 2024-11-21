using AutoMapper;
using BookShelfKimHa.Behavior;
using CoreInfrastructure.DataContext;
using CoreInfrastructure.IRepository;
using CoreInfrastructure.Repository;
using Infrastructure.Common;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Service.Implementation;
using Service.IService;


var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<AppSetting>(builder.Configuration.GetSection("AppSetting"));

var configuration = builder.Configuration;
var connectionString = configuration.GetConnectionString("DbConnectionString");
// Add services to the container.

builder.Services.AddDbContext<IBaseDbContext, MyDataContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
}).CreateMapper());

builder.Services.AddCorsPolicyServices(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();;
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddTransient<IBookRepository, BookRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //await app.ApplyMigrationsData();
}

app.UseCorsPolicyServices();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
