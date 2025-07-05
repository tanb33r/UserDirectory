using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using UserDirectory.Application.Dtos;
using UserDirectory.Application.Interfaces;
using UserDirectory.Infrastructure.Sql;
using UserDirectory.Infrastructure.Mongo.Repositories;
using UserDirectory.Application.Services;
using UserDirectory.Infrastructure.Sql.Services;
using UserDirectory.Application.Mapping;
using UserDirectory.Infrastructure.Mongo.Services;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<UserDirectory.WebApi.Services.IDataSourceContext, UserDirectory.WebApi.Services.DataSourceContext>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddDbContext<UserDirectoryDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("MSSMS")));


builder.Services.AddAutoMapper(typeof(IUserService).Assembly,
                               typeof(MappingProfile).Assembly);


var mongoConn = configuration.GetConnectionString("Mongo");
var mongoDbName = configuration.GetSection("MongoDb")["DatabaseName"];
builder.Services.AddSingleton<MongoUserRepositoryAsync>(_ => new MongoUserRepositoryAsync(mongoConn, mongoDbName));
builder.Services.AddScoped<SqlUserRepository>();

//builder.Services.AddScoped<UserService>();


// Register DynamicUserService as the only IUserService
builder.Services.AddScoped<IUserService, DynamicUserService>();

builder.Services.AddScoped<IUserRepository>(sp =>
{
    var dsCtx = sp.GetRequiredService<UserDirectory.WebApi.Services.IDataSourceContext>();
    var dataSource = dsCtx.GetCurrentDataSource();
    if (dataSource == "MongoDB")
        return sp.GetRequiredService<MongoUserRepositoryAsync>();
    return sp.GetRequiredService<SqlUserRepository>();
});

builder.Services.AddSingleton<MongoRoleService>(_ => new MongoRoleService(mongoConn, mongoDbName));
builder.Services.AddScoped<RoleService>();

builder.Services.AddScoped<IRoleService>(sp =>
{
    var dsCtx = sp.GetRequiredService<UserDirectory.WebApi.Services.IDataSourceContext>();
    var dataSource = dsCtx.GetCurrentDataSource();
    if (dataSource == "MongoDB")
        return sp.GetRequiredService<MongoRoleService>();
    return sp.GetRequiredService<RoleService>();
});

builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserDto>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy => policy.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                 .AllowCredentials());
});


var app = builder.Build();

app.UseSession();
app.UseCors("AllowAngularApp");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();