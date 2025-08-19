using AuthService;
using AuthService.ConfigurationResolve;
using AuthService.ConfigurationsRegistration;
using AuthService.CustomExceptions;
using AuthService.DbContextModels;
using AuthService.Models;
using AuthService.Services;
using AuthService.Services.NotificationService;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Diagnostics;
using System.Reflection;
using System.Threading.RateLimiting;

internal class Program
{
    private static void Main(string[] args)
    {
        if (!Debugger.IsAttached)
        {
            Debugger.Launch();  // This will prompt to attach a debugger.
        }
        var builder = WebApplication.CreateBuilder(args);

        //register using two classes implementing single interface

        //builder.Services.AddScoped<INotify, Message>();
        //builder.Services.AddScoped<INotify, Mail>();



        builder.Services.AddScoped<Message>();
        builder.Services.AddScoped<Mail>();

        builder.Host.UseDefaultServiceProvider(op =>
        {
            op.ValidateOnBuild = true;
            op.ValidateScopes = true;
        });

        //try
        //{
        //    builder.Services.AddScoped<Func<string, INotify>>(serviceProvider => key =>
        //    {
        //        return key switch
        //        {
        //            "mail" => serviceProvider.GetRequiredService<Mail>(),
        //            "message" => serviceProvider.GetRequiredService<Message>(),
        //            _ =>throw new OwnException()
        //        };
        //    });
        //}
        //catch (Exception ex)
        //{
             
        //}
       


        //Registering Services
        builder.Services.AddScoped<IMyServScoped,ScopedMyServ > ();
        builder.Services.AddScoped<IMyServSingle, SingletonMyServ>();
        builder.Services.AddScoped<IMyServTrans, TransientMyServ>();
         
        //builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        //Adding custom json file for 
        builder.Configuration.AddJsonFile("VinnuCustom.json", optional: false, reloadOnChange: true);
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddScoped<TokenService>();
        // environment variables
        //builder.Configuration.AddEnvironmentVariables();
        var vinayEnvVar = builder.Configuration["vinayEnvVar"];

        ///serilog
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            //.WriteTo.Console() // Log to terminal
            //.WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Minute) // File logs
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} | VinayId: {TraceId}{NewLine}{Exception}")

            .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day, outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} | VinayId: {TraceId}{NewLine}{Exception}")
            .WriteTo.Seq("http://localhost:5341") // Optional: log to Seq UI
            .CreateLogger();

        builder.Host.UseSerilog();

        var name = builder.Configuration.GetValue<string>("myownSettings:Name");
        var Age = builder.Configuration.GetValue<string>("myownSettings:Age");
        builder.Services.Configure<CustomJsonSettings>(builder.Configuration.GetSection("myownSettings"));
        //Versioning
        //builder.Services.AddApiVersioning(options =>
        //{
        //    options.AssumeDefaultVersionWhenUnspecified = true;
        //    options.DefaultApiVersion = new ApiVersion(1, 0);
        //    options.ReportApiVersions = true;

        //    // Enable URL segment versioning like /api/v1/controller
        //    options.ApiVersionReader = new UrlSegmentApiVersionReader();
        //});

        ///////////////////Cors
        ///
         

        //builder.Services.AddCors(options =>
        //{
        //    options.AddPolicy("mypolicy", policy =>
        //    {
        //        policy.WithOrigins("http://localhost:4200") // Angular dev server
        //              .AllowAnyHeader()
        //              .AllowAnyMethod()
        //              .AllowCredentials(); // VERY IMPORTANT!
        //    });
        //});
        //ratelimitor
        builder.Services.AddRateLimiter(options =>
        {
            // Example: 5 requests per 10 seconds per IP
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
            {
                var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                return RateLimitPartition.GetFixedWindowLimiter(ipAddress, _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 3,
                    Window = TimeSpan.FromSeconds(5),
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    QueueLimit = 0
                });
            });

            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        });

        //builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        //    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://login.microsoftonline.com/9ef56c7a-123b-4d47-8ffd-774bd0c128f8/v2.0";
        options.Audience = "api://9ef56c7a-123b-4d47-8ffd-774bd0c128f8";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true
        };
    });


        //builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        //    .AddJwtBearer(options =>
        //    {
        //        var key = Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]);
        //        options.TokenValidationParameters = new TokenValidationParameters
        //        {
        //            ValidateLifetime = true,
        //            ValidateIssuer = true,
        //            ValidateAudience = true,
        //            ValidIssuers = new List<string>() { "EPMSAuth0", "EPMSAuth", "Admin" },
        //            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        //            ValidAudience = builder.Configuration["JwtSettings:Audience"],

        //            ValidateIssuerSigningKey = true,
        //            IssuerSigningKey = new SymmetricSecurityKey(key),
        //        };
        //        options.Events = new JwtBearerEvents
        //        {

        //            OnMessageReceived = ctx =>
        //            {
        //                ctx.Token = ctx.Request.Cookies["access_token"];
        //                return Task.CompletedTask;
        //            }
        //        };
        //    });
        builder.Services.AddAuthorization();
        builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
        builder.Services.AddScoped<Whitelisting>();
        var app = builder.Build();
        app.Use(async (context, next) =>
        {
            using (Serilog.Context.LogContext.PushProperty("VinayId", context.TraceIdentifier))
            {
                await next();
            }
        });
        app.UseSerilogRequestLogging();
        //app.UseMiddleware<Whitelisting>();
        app.addCorsMiddleware();
        
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseRateLimiter();
        //app.Use(async (context, next) =>
        //{
        //    if (!HttpMethods.IsPost(context.Request.Method))
        //    {
        //        context.Response.StatusCode = StatusCodes.Status405MethodNotAllowed;
        //        await context.Response.WriteAsync("Only POST requests are allowed.");
        //        return;
        //    }

        //    await next();
        //});
       
        app.UseAuthentication();
        app.UseAuthorization();


        app.MapControllers();

        app.MapFallbackToFile("index.html");
        app.Run();
    }
}