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
        new MySqlServerVersion(new Version(8, 4, 11)),
        mySqlOptions =>
        {
            mySqlOptions.EnableRetryOnFailure(
                maxRetryCount: 10,
                maxRetryDelay: TimeSpan.FromSeconds(5),
                errorNumbersToAdd: null
            );
        }
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

            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey!)
            ),

            ClockSkew = TimeSpan.Zero
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
            },

            OnAuthenticationFailed = context =>
            {
                Console.WriteLine("========== JWT ERRO ==========");
                Console.WriteLine(context.Exception.Message);
                Console.WriteLine("==============================");

                return Task.CompletedTask;
            },

            OnTokenValidated = context =>
            {
                Console.WriteLine("========== JWT OK ==========");
                Console.WriteLine(
                    $"Usuário: {context.Principal?.Identity?.Name}"
                );
                Console.WriteLine("============================");

                return Task.CompletedTask;
            }
        };
    });

// Permite utilizar [Authorize]
builder.Services.AddAuthorization();

//=============================

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IUserIdProvider, UserIdProvider>();

// OpenAPI
builder.Services.AddOpenApi();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    var maxAttempts = 10;

    for (var attempt = 1; attempt <= maxAttempts; attempt++)
    {
        try
        {
            Console.WriteLine(
                $"Tentando conectar ao MySQL... " +
                $"tentativa {attempt}/{maxAttempts}"
            );

            db.Database.Migrate();

            Console.WriteLine("MySQL conectado e migrations aplicadas.");
            break;
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"MySQL ainda não está disponível: {ex.Message}"
            );

            if (attempt == maxAttempts)
            {
                Console.WriteLine(
                    "Não foi possível conectar ao MySQL."
                );

                throw;
            }

            Thread.Sleep(TimeSpan.FromSeconds(5));
        }
    }
}

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


using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider
        .GetRequiredService<AppDbContext>();

    var conversation = new ConversationsModel
    {
        ID = Guid.NewGuid(),

        User1Id = Guid.Parse(
            "2a50545a-a119-45f0-86a2-f1ea4ccab843"
        ),

        User2Id = Guid.Parse(
            "def7c98e-8466-42ea-a7d2-6db818c6fe7b"
        )
    };

    context.Conversations.Add(conversation);

    await context.SaveChangesAsync();

    Console.WriteLine(
        $"Conversation criada: {conversation.ID}"
    );
}


app.Run();