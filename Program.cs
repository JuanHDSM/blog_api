using System.Text;
using blog_api.Services;
using BlogApi;
using BlogApi.Data;
using BlogApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

ConfigureAutehntication(builder);
ConfigureMvc(builder);
ConfigureServices(builder); 

var app = builder.Build();

LoadConfiguration(app);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

void LoadConfiguration(WebApplication app)
{
    Configuration.JwtKey = app.Configuration.GetValue<string>("JwtKey")!;
    Configuration.ApiKeyName = app.Configuration.GetValue<string>("ApiKeyName")!;
    Configuration.ApiKey = app.Configuration.GetValue<string>("ApiKey")!;

    var smtp = new Configuration.SmtpSettings();
    app.Configuration.GetSection("Smtp").Bind(smtp);
    Configuration.Smtp = smtp;
}

void ConfigureAutehntication(WebApplicationBuilder builder)
{
    var key = Encoding.ASCII.GetBytes(Configuration.JwtKey);

    builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(x =>
    {
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
}

void ConfigureMvc(WebApplicationBuilder builder)
{
    builder.Services.AddControllers().ConfigureApiBehaviorOptions(option => option.SuppressModelStateInvalidFilter = true);
}

void ConfigureServices(WebApplicationBuilder builder)
{
    builder.Services.AddDbContext<AppDbContext>();
    builder.Services.AddTransient<TokenService>();
    builder.Services.AddTransient<EmailService>();
}
