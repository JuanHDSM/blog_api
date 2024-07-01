using blog_api.Services;
using BlogApi.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers().ConfigureApiBehaviorOptions(option => option.SuppressModelStateInvalidFilter = true);
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddTransient<TokenService>();

var app = builder.Build();

app.MapControllers();

app.Run();
