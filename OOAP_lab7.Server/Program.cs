using OOAP_lab7.Server.Data;
using OOAP_lab7.Server.Handlers;
using OOAP_lab7.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ICommentRepository, InMemoryCommentRepository>();
builder.Services.AddScoped<CommandManager>();
builder.Services.AddScoped<AuthenticationHandler>();
builder.Services.AddScoped<RolePermissionHandler>();
builder.Services.AddScoped<CommentStatusHandler>();
builder.Services.AddScoped<IModerationHandler>(provider =>
{
    var authHandler = provider.GetRequiredService<AuthenticationHandler>();
    var roleHandler = provider.GetRequiredService<RolePermissionHandler>();
    var statusHandler = provider.GetRequiredService<CommentStatusHandler>();

    authHandler.SetNext(roleHandler).SetNext(statusHandler);
    return authHandler;
});

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("/index.html");

app.Run();
