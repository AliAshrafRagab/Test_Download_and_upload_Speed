public class UploadRewriteMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<UploadRewriteMiddleware> _logger;

    public UploadRewriteMiddleware(RequestDelegate next, ILogger<UploadRewriteMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value.ToLower();

        // تحديد المسار فقط إذا كان /upload
        if (path == "/upload")
        {
            // إرسال استجابة سريعة للمسار المطلوب
            context.Response.StatusCode = 200;
            await context.Response.WriteAsync("Upload handler - 200 OK");
            return; // لا تحتاج إلى متابعة باقي الـ pipeline لهذه الطلبات
        }

        // إذا لم يكن المسار هو /upload، تابع معالجة الطلب
        await _next(context);
    }
}
