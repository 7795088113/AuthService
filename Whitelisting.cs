namespace AuthService
{
    public class Whitelisting :IMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _config;  
        private readonly HashSet<string> _whitelistedIps;
        public Whitelisting(IConfiguration configuration)
        {
            _config = configuration;
            var ipList = _config.GetSection("IpWhitelist:allowedIp").Get<List<string>>() ?? new();
            _whitelistedIps = ipList.Select(x => x.Trim()).ToHashSet<string>();
        }
       //public async Task InvokeAsync(HttpContext context)
       // {
       //     await _next(context);
       // }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await next(context);
        }
    }
}
