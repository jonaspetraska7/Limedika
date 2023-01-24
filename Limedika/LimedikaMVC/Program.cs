using Common.Data;
using Common.Entities;
using LimedikaMVC.Data;
using LinqToDB;
using LinqToDB.AspNet;
using LinqToDB.AspNet.Logging;
using LinqToDB.Configuration;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddLinqToDBContext<LimedikaDataConnection>((provider, options) => {
    options
    .UseSqlServer(builder.Configuration.GetConnectionString("LimedikaDb"))
    .UseDefaultLogging(provider);
});
builder.Services.AddDbContext<LimedikaDataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("LimedikaDb")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var dataConnection = scope.ServiceProvider.GetService<LimedikaDataConnection>();
    var sp = dataConnection.DataProvider.GetSchemaProvider();
    var dbSchema = sp.GetSchema(dataConnection);
    if (!dbSchema.Tables.Any(t => t.TableName == nameof(Client)))
    {
        dataConnection.CreateTable<Client>();
    }
    if (!dbSchema.Tables.Any(t => t.TableName == nameof(Log)))
    {
        dataConnection.CreateTable<Log>();
    }
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Clients}/{action=Index}/{id?}");

app.Run();
