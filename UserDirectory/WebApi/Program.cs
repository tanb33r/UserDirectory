using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using UserDirectory.Application.Dtos;
using UserDirectory.Infrastructure.Sql;
using UserDirectory.Infrastructure.Mongo.Repositories;
using UserDirectory.Application.Services;
using UserDirectory.Application.Mapping;
using UserDirectory.Infrastructure.Mongo.Services;
using AutoMapper;
using UserDirectory.Infrastructure.Sql.Repositories;
using UserDirectory.Application.Abstraction.Services;
using UserDirectory.Application.Abstraction.Repositories;

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
builder.Services.AddSingleton(_ => new MongoUserRepository(mongoConn, mongoDbName));
builder.Services.AddScoped<SqlUserRepository>();

builder.Services.AddSingleton(_ => new MongoRoleRepository(mongoConn, mongoDbName));
builder.Services.AddScoped<SqlRoleRepository>();

//builder.Services.AddScoped<UserService>();


// Register DynamicUserService as the only IUserService
builder.Services.AddScoped<IUserService, DynamicUserService>();

builder.Services.AddScoped<IUserRepository>(sp =>
{
    var dsCtx = sp.GetRequiredService<UserDirectory.WebApi.Services.IDataSourceContext>();
    var dataSource = dsCtx.GetCurrentDataSource();
    if (dataSource == "MongoDB")
        return sp.GetRequiredService<MongoUserRepository>();
    return sp.GetRequiredService<SqlUserRepository>();
});

builder.Services.AddScoped<IRoleService, DynamicRoleService>();

builder.Services.AddScoped<IRoleRepository>(sp =>
{
    var dsCtx = sp.GetRequiredService<UserDirectory.WebApi.Services.IDataSourceContext>();
    var dataSource = dsCtx.GetCurrentDataSource();
    if (dataSource == "MongoDB")
        return sp.GetRequiredService<MongoRoleRepository>();
    return sp.GetRequiredService<SqlRoleRepository>();
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