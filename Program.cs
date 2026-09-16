using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjetoMultidiciplinar.Data;
using ProjetoMultidiciplinar.Hubs;
using ProjetoMultidiciplinar.Models;
using ProjetoMultidiciplinar.Providers;
using ProjetoMultidiciplinar.Services;

DotNetEnv.Env.Load();
var builder = WebApplication.CreateBuilder(args);

var dbServer = Environment.GetEnvironmentVariable("DB_SERVER");
var dbName = Environment.GetEnvironmentVariable("DB_NAME");
var dbUser = Environment.GetEnvironmentVariable("DB_USER");
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");

var connectionString =
    $"Server={dbServer};" +
    $"Database={dbName};" +
    $"User={dbUser};" +
    $"Password={dbPassword};";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)
    )
);
// Registra os Controllers
builder.Services.AddControllers();
//WebSockets
builder.Services.AddSignalR();
//==========================JWT
builder.Services.AddScoped<FilesService>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<MessagesService>();

var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");
var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .WithOrigins("http://127.0.0.1:5500")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = "ProjetoMultidisciplinar",
            ValidAudience = "ProjetoMultidisciplinar",

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey!)
            )
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];

                var path = context.HttpContext.Request.Path;

                if (!string.IsNullOrEmpty(accessToken) &&
                    path.StartsWithSegments("/eventHub"))
                {
                    context.Token = accessToken;
                }

                return Task.CompletedTask;
            }
        };
    });

// Permite utilizar [Authorize]
builder.Services.AddAuthorization();

//=============================

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddSingleton<IUserIdProvider, UserIdProvider>();

// OpenAPI
builder.Services.AddOpenApi();

var app = builder.Build();
//arquivo estaticso
app.UseStaticFiles();

// Configuração do ambiente de desenvolvimento (voltar isso if() dps)

app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("Frontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<EventHub>("/eventHub");

// Cria salas pra testar o chat

// using (var scope = app.Services.CreateScope())
// {
//     var context = scope.ServiceProvider
//         .GetRequiredService<AppDbContext>();

//     var conversation = new ConversationsModel
//     {
//         ID = Guid.NewGuid(),

//         User1Id = Guid.Parse(
//             "18689fa9-1b4a-4dc2-8e94-987ba871ac95"
//         ),

//         User2Id = Guid.Parse(
//             "c1514ef7-a967-444d-8f07-18e7ca68be2f"
//         )
//     };

//     context.Conversations.Add(conversation);

//     await context.SaveChangesAsync();

//     Console.WriteLine(
//         $"Conversation criada: {conversation.ID}"
//     );
// }


app.Run();