using LibraryAdmin.DataAccess;
using LibraryAdmin.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddDbContext<LibraryAdminDbContext>(
    opts =>
    {
        opts.UseNpgsql(config.GetConnectionString(nameof(LibraryAdminDbContext)), b => b.MigrationsAssembly("LibraryAdmin"));
    }
);

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddTransient<AuthorRepository>();
builder.Services.AddTransient<BookRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
