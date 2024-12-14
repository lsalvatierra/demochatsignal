

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSignalR();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();
// Rutas SignalR
app.MapHub<demochatsignal.Hubs.ChatHub>("/chatHub");

app.Run();
