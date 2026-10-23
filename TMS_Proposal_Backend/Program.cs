using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;
using TMS_Proposal_Backend.Data;
using TMS_Proposal_Backend.DTOs;
using TMS_Proposal_Backend.Repository.AuditsRepository;
using TMS_Proposal_Backend.Repository.AuthRepository;
using TMS_Proposal_Backend.Repository.RequestRepository;
using TMS_Proposal_Backend.Services.AuditsLogService;
using TMS_Proposal_Backend.Services.AuthService;
using TMS_Proposal_Backend.Services.RequestService;
using TMS_Proposal_Backend.Services.TokenService;
using TMS_Proposal_Backend.Validations;

var builder = WebApplication.CreateBuilder(args);

//Register AppDbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

//DI for Repository layer
builder.Services.AddScoped<ISignUpRepository, SignUpRepository>();
builder.Services.AddScoped<ISignInRepository, SignInRepository>();
builder.Services.AddScoped<IRequestRepository, RequestRepository>();
builder.Services.AddScoped<IAuditsLogRepoitory, AuditsLogRepository>();

//DI for Service layer
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ISignUpService, SignUpService>();
builder.Services.AddScoped<ISignInService, SignInService>();
builder.Services.AddScoped<IRequestService, RequestService>();
builder.Services.AddScoped<IAuditsLogService, AuditsLogService>();
builder.Services.AddHttpContextAccessor();

//validation di
builder.Services.AddScoped<IValidator<SignUpDTO>, SignUpValidator>();
builder.Services.AddScoped<IValidator<SignInDTO>, SignInValidator>();

//AutoMapper DI
builder.Services.AddAutoMapper(cfg => { }, typeof(Program));

//JWT Token Generation
// Fetch JWT settings from appsettings.json
var jwtSettings = builder.Configuration.GetSection("Jwt");

var secret = jwtSettings["Key"];
var issuer = jwtSettings["Issuer"];
var audience = jwtSettings["Audience"];

if (string.IsNullOrWhiteSpace(secret) ||
    string.IsNullOrWhiteSpace(issuer) ||
    string.IsNullOrWhiteSpace(audience))
{
    throw new InvalidOperationException(
        "JWT configuration must include Jwt:Key, Jwt:Issuer, and Jwt:Audience.");
}

var key = Encoding.UTF8.GetBytes(secret);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "TMS_Proposal API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter the JWT access token here."
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>()
    });
});

//Add Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Angular URL
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TMS_Proposal Api Endpoints");
    });
}

app.UseHttpsRedirection();
app.UseCors("AngularFrontend");
app.UseAuthentication();

app.UseCors("AllowAngular");

app.UseAuthorization();

app.MapControllers();

app.Run();