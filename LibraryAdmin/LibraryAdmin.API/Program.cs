using LibraryAdmin.API.ExceptionFilters;
using LibraryAdmin.Business.CustomExceptions;
using LibraryAdmin.Business.Services;
using LibraryAdmin.DataAccess;
using LibraryAdmin.DataAccess.Repositories.Contracts;
using LibraryAdmin.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddDbContext<LibraryAdminDbContext>(
    opts =>
    {
        opts.UseNpgsql(config.GetConnectionString(nameof(LibraryAdminDbContext)), b => b.MigrationsAssembly("LibraryAdmin.DataAccess"));
    }
);

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IAuthorRepository, AuthorRepository>();
builder.Services.AddTransient<IBookRepository, BookRepository>();

builder.Services.AddTransient<AuthorService>();
builder.Services.AddTransient<BookService>();

builder.Services.AddMvc(opts => 
{
    opts.Filters.Add<OperationCancelledExceptionFilter>();
    opts.Filters.Add<InvalidEntityExceptionFilter>();
    opts.Filters.Add<EntityNotFoundExceptionFilter>();
    opts.Filters.Add<NegativeBooksAmountExceptionFilter>();
});


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseHttpsRedirection();

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
