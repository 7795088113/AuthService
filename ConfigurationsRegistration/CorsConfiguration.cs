namespace AuthService.ConfigurationsRegistration
{
    public static  class CorsConfiguration
    {
        public static void AddCorsCustomConfiguration(IServiceCollection Services)
        {
            Services.AddCors(options =>
            {
                options.AddPolicy("mypolicy", policy =>
                {
                    policy.WithOrigins("http://localhost:4200") // Angular dev server
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials(); // VERY IMPORTANT!
                });
            });
           
        }
        public static void addCorsMiddleware(this IApplicationBuilder builder)
        {
            builder.UseCors("mypolicy");
        }
    }
}
