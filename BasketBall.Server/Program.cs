using BasketBall.Server.Data;
using BasketBall.Server.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Ajouter SignalR au conteneur de services
builder.Services.AddSignalR();

// Configuration des services CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("https://localhost:50985") // L'origine du frontend Angular
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Nécessaire pour SignalR
    });
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Ajouter les services applicatifs
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<TeamService>();
builder.Services.AddScoped<PlayerService>();
builder.Services.AddScoped<MatchService>();
builder.Services.AddScoped<StartingFiveService>();
builder.Services.AddScoped<TimerMatchService>();
builder.Services.AddScoped<BasketEventService>();
builder.Services.AddScoped<FaultEventService>();
builder.Services.AddScoped<PlayerSubstitutionEventService>();
builder.Services.AddScoped<AuthService>();


builder.Services.AddControllers();

// Configuration Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure le pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Ajouter la politique CORS
app.UseCors("AllowSpecificOrigins");

app.UseAuthorization();

// Mapper les contrôleurs
app.MapControllers();

// Mapper SignalR
app.MapHub<MatchEventsHub>("/hubs/match-events").RequireCors("AllowSpecificOrigins");

app.MapFallbackToFile("/index.html");

app.Run();
