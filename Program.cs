using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Настройка контекста базы данных
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

var app = builder.Build();

// Автоматическое создание базы данных (если она отсутствует)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var dbContext = services.GetRequiredService<AppDbContext>();
        
        // Проверяем, существует ли база данных. Если нет, создаем её.
        dbContext.Database.EnsureCreated();

        // Если вы используете миграции, примените их:
        // dbContext.Database.Migrate();
    }
    catch (Exception ex)
    {
        // Логируем ошибку, если что-то пошло не так
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while creating the database.");
    }
}

app.MapControllers();

app.Run();