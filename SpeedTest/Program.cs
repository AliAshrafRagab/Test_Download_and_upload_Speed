using SpeedTest;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
// تفعيل التصريحات للملفات الثابتة (Static Files)
app.UseStaticFiles(); // تأكد من أنك تستخدم هذه لتفعيل الملفات الثابتة

// دمج الـ Middleware مباشرة مع MapControllerRoute باستخدام MapWhen
// دمج الـ Middleware مباشرة مع MapControllerRoute باستخدام MapWhen
app.MapWhen(context => context.Request.Path.Equals("/TestUploadSpeed", StringComparison.OrdinalIgnoreCase),
    builder =>
    {
        builder.Run(async context =>
        {
            // إرسال استجابة فورية للمسار /upload
            context.Response.StatusCode = 200;
            await context.Response.WriteAsync("Upload handler - 200 OK");
        });
    });


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
