using SpeedTest;

var builder = WebApplication.CreateBuilder(args);

// إضافة خدمات الـ HTTP Context
builder.Services.AddHttpContextAccessor();

// إضافة إعدادات CORS للسماح بالطلبات من جميع النطاقات
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());
});

// إضافة الخدمات اللازمة للتحكم في العرض
builder.Services.AddControllersWithViews();

var app = builder.Build();

// تحديد البيئة (لتخصيص التعامل مع الأخطاء)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseRouting();

// تفعيل ملفات ثابتة و CORS
app.MapStaticAssets();
app.UseStaticFiles();
app.UseCors("AllowAllOrigins");


// تخصيص المسار /TestUploadSpeed للتعامل مع POST
app.MapWhen(context => context.Request.Path.Equals("/TestUploadSpeed", StringComparison.OrdinalIgnoreCase),
    builder =>
    {
        builder.Run(async context =>
        {
            // إرسال استجابة فورية للمسار /TestUploadSpeed
            context.Response.StatusCode = 200;
            await context.Response.WriteAsync("Upload handler - 200 OK");
        });
    });
app.Use(async (context, next) =>
{
    context.Response.Headers["Cache-Control"] = "no-store, no-cache, no-transform, must-revalidate";
    await next.Invoke();
});
app.Use(async (context, next) =>
{
    context.Response.Headers["Cache-Control"] = "no-store";
    context.Response.Headers["Pragma"] = "no-cache";
    context.Response.Headers["Expires"] = "0";
    await next.Invoke();
});
app.Use(async (context, next) =>
{
    context.Request.ContentLength = 2147483647; // تحديد الحد الأقصى لحجم الطلب
    await next.Invoke();
});

// تخصيص المسار الافتراضي
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// تشغيل التطبيق
app.Run();
