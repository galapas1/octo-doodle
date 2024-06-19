using PdfOcrManager.Interfaces;
using PdfOcrManager.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IPdfParserService, PdfParserService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment()) { }

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapControllerRoute(name: "default", pattern: "{controller=ParsePDF}/");

app.Run();
