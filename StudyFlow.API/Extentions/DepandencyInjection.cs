namespace StudyFlow.API.Extentions;

public static class DepandencyInjection
{
    public static IServiceCollection AddDependenciesServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Add services to the container.
        services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        services.AddConnectionConfig(configuration)
            .AddOpenApi()
            .AddIdentityConfig()
            .AddValidationConfig()
            .AddMapsterConfig()
            .AddRegistrationServicesConfig()
            .AddAuthConfig(configuration);
        return services;
    }

    private static IServiceCollection AddConnectionConfig(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ConString") ??
            throw new InvalidOperationException("Connection string 'ConString' not found in the configuration.");
        services.AddDbContext<ApplicationDbContext>(option =>
            option.UseNpgsql(connectionString)
        );
        return services;
    }

    private static IServiceCollection AddIdentityConfig(this IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        return services;
    }

    private static IServiceCollection AddAuthConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<JwtOptions>()
            .BindConfiguration(JwtOptions.SectionName)
            .ValidateDataAnnotations();
        var jwtoptions = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtoptions!.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtoptions!.Audience,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtoptions.AccessToken))
            };
        });
        return services;
    }

    private static IServiceCollection AddValidationConfig(this IServiceCollection services) =>
        services.AddFluentValidationAutoValidation()
        .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

    private static IServiceCollection AddMapsterConfig(this IServiceCollection services)
    {
        var mapConfig = TypeAdapterConfig.GlobalSettings;
        mapConfig.Scan(Assembly.GetExecutingAssembly());
        services.AddSingleton<IMapper>(new Mapper(mapConfig));
        return services;
    }

    private static IServiceCollection AddRegistrationServicesConfig(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        return services;
    }
}