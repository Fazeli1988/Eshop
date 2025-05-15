using Asp.Versioning;
using AutoMapper;
using DotNetCore.CAP;
using IDP.Application.Handler.Command.User;
using IDP.Application.Helper;
using IDP.Domain.IRepository.Command;
using IDP.Domain.IRepository.Command.Base;
using IDP.Domain.IRepository.Query;
using IDP.Infra.Data;
using IDP.Infra.Repository.Command;
using IDP.Infra.Repository.Command.Base;
using IDP.Infra.Repository.Query;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
#region redisConfig
builder.Services.AddStackExchangeRedisCache(option =>
{
    option.Configuration = builder.Configuration.GetValue<string>("CachSetting:RedisUrl");
});
// Auto Mapper Configurations
var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

#endregion
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(typeof(UserHandler).GetTypeInfo().Assembly);
builder.Services.AddScoped<IOtpRedisRepository, OtpRedisRepository>();

builder.Services.AddScoped<IUserCommandRepository, UserCommandRepository>();
builder.Services.AddScoped<IUserQueryRepository, UserQueryRepository>();
builder.Services.AddScoped(typeof(ICommandRepository<>), typeof(CommandRepository<>));
builder.Services.AddTransient<ShopCommandDbContext>();
builder.Services.AddTransient<ShopQueryDbContext>();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1);
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("X-Api-Version"));
})
.AddMvc() // This is needed for controllers
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'V";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddCap(options =>
{
    options.UseEntityFramework<ShopCommandDbContext>();
    options.UseDashboard(path => path.PathMatch = "/cap");
    options.UseRabbitMQ(options =>
    {
        options.ConnectionFactoryOptions = options =>
        {
            options.Ssl.Enabled = builder.Configuration.GetValue<bool>("RabbitMq:RedisUrl"); 
            options.HostName = builder.Configuration.GetValue<string>("RabbitMq:localhost");
            options.UserName = builder.Configuration.GetValue<string>("RabbitMq:UserName"); 
            options.Password = builder.Configuration.GetValue<string>("SslEnabled:Password"); 
            options.Port = builder.Configuration.GetValue<int>("RabbitMq:Port"); 
        };

    });
    options.FailedRetryCount = builder.Configuration.GetValue<int>("RabbitMq:FailedRetryCount"); 
    options.FailedRetryInterval = builder.Configuration.GetValue<int>("RabbitMq:FailedRetryInterval"); 


});
//builder.Services.AddSingleton<CustomerAddedEventSubscriber>();
Auth.Extensions.AddJwt(builder.Services, builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
