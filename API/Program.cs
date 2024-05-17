using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Middleware;
using Application.Services.Implementation;
using Application.Services.Interfaces;
using Infrastructure.Authentication;
using Infrastructure.Data;
using Infrastructure.EmailService;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Stripe;
using System.Reflection;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// Add Swagger services
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Shopping App API", Version = "v1" });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);

    // Define the JWT Bearer authentication scheme
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });


    // Make sure Swagger UI requires a Bearer token to access the API
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IUserRepository, UserRepo>();
builder.Services.AddScoped<IAddressRepository,AddressRepo>();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthentication, Authentication>();
builder.Services.AddScoped<ITokenService, Application.Services.Implementation.TokenService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, Application.Services.Implementation.ProductService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ICartService,CartService>();
builder.Services.AddScoped<IPamentService, PaymentService>();
builder.Services.AddScoped<ICheckoutService, CheckoutService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IEmailSender, EmailSender>();


Stripe.StripeConfiguration.ApiKey = "sk_test_51PBBftBKfM8g2ev1FzRWy6XtCUYaSzilfG3AlqZQQuVJkQUa39ImNmZVgsLbAEjiRJpRYhmjSWE7Dqi9gYFklmjr00RaNudRWu";

builder.Services.Configure<AuthConfiguration>(options =>
{
    options.clientId = builder.Configuration["Auth0:ClientId"];
    options.connection = builder.Configuration["Auth0:Connection"]; 
    options.Domain = builder.Configuration["Auth0:Domain"];
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.Authority = $"https://{builder.Configuration["Auth0:Domain"]}/";
    options.Audience = builder.Configuration["Auth0:Audience"];
    options.TokenValidationParameters = new TokenValidationParameters
    {
        NameClaimType = ClaimTypes.NameIdentifier
    };
});

builder.Services
  .AddAuthorization(options =>
  {
      options.AddPolicy(
        "read:messages",
        policy => policy.Requirements.Add(
          new HasScopeRequirement("read:messages", builder.Configuration["Auth0:Domain"]
)
        )
      );
  });

builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

//
var logger = Serilog.Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration( builder.Configuration ).CreateLogger();
//builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Enable Swagger UI
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Demo API V1");

        // Enable JWT token input box in Swagger UI
        c.OAuthClientId("swagger-ui");
        c.OAuthAppName("Swagger UI");
    });
}


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
