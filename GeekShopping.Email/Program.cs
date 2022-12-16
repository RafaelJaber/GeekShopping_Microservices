using GeekShopping.Email.MessageConsumer;
using GeekShopping.Email.Models.Context;
using GeekShopping.Email.Repository;
using GeekShopping.Email.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SqlContext>(
options => options
    .UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")
                  ?? throw new InvalidOperationException("Connection string 'SqlServer' not found.")));

// Add services to the container.

builder.Services.AddControllers();

// Services
var sqlContext = new DbContextOptionsBuilder<SqlContext>();
sqlContext.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));

builder.Services.AddSingleton(new EmailRepository(sqlContext.Options));

builder.Services.AddHostedService<RabbitMqPaymentConsumer>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "GeekShopping.Email", Version = "v1" });
    c.EnableAnnotations();
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"Enter 'Bearer' [space] and your token!",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()){
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
