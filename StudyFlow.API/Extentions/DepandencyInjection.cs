namespace StudyFlow.API.Extentions;

public static class DepandencyInjection
{
    public static IServiceCollection AddDependenciesServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Add services to the container.
        services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        services.AddOpenApi(options => options.AddDocumentTransformer<BearerSecuritySchemeTransformer>());
        services.AddConnectionConfig(configuration)
            .AddEndpointsApiExplorer()
            .AddIdentityConfig()
            .AddCorsConfig(configuration)
            .AddFilterConfig()
            .AddMailConfig(configuration)
            .AddValidationConfig()
            .AddMapsterConfig()
            .AddRegistrationServicesConfig()
            .AddAuthConfig(configuration);
        services.Configure<ApiBehaviorOptions>(option =>
            option.SuppressModelStateInvalidFilter = true
        );
        services.AddHealthChecks()
            .AddNpgSql(connectionString: configuration.GetConnectionString("ConString")!, name: "database")
            .AddCheck<MailProviderHealthCheck>(name: "mail service", tags: ["api"]);
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
        services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        services.Configure<IdentityOptions>(option =>
        {
            option.Password.RequiredLength = 8;
            //option.SignIn.RequireConfirmedEmail = true;
            option.SignIn.RequireConfirmedPhoneNumber = false;
            option.User.RequireUniqueEmail = true;
            option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15); // Lockout duration
            option.Lockout.MaxFailedAccessAttempts = 5; // Max Failed
        });
        return services;
    }

    private static IServiceCollection AddAuthConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<JwtOptions>()
            .BindConfiguration(JwtOptions.SectionName)
            .ValidateDataAnnotations();
        var jwtOptions = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();
        services.AddOptions<GoogleOptions>()
            .BindConfiguration(GoogleOptions.SectionName)
            .ValidateDataAnnotations();
        var googleOptions = configuration.GetSection(GoogleOptions.SectionName).Get<GoogleOptions>();
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
                ValidIssuer = jwtOptions!.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtOptions!.Audience,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.AccessToken))
            };
        }).AddGoogle(options =>
        {
            options.ClientId = googleOptions!.ClientId;
            options.ClientSecret = googleOptions!.ClientSecret;
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
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IStudentService, StudentService>();
        services.AddSingleton<IJwtProvider, JwtProvider>();
        services.AddScoped<IEmailSender, EmailService>();
        services.AddScoped<IVerificationCodeService, VerificationCodeService>();
        return services;
    }

    private static IServiceCollection AddCorsConfig(this IServiceCollection services,IConfiguration configuration) =>
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(option =>
            {
                option.AllowAnyHeader();
                option.AllowAnyMethod();
                option.WithOrigins(configuration.GetSection("AllowedOrigins").Get<string[]>()!);
            });
        });
    private static IServiceCollection AddFilterConfig(this IServiceCollection services)
    {
        services
        .Configure<JsonOptions>(options =>
        {
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = true; // Keep case-insensitive deserialization
            options.JsonSerializerOptions.AllowTrailingCommas = false; // Prevents extra commas
            options.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Disallow; // Disallow comments
        });
        return services;
    }
    private static IServiceCollection AddMailConfig(this IServiceCollection services,IConfiguration configuration)
    {
        // Configure For Mail Setting
        services.Configure<MailSetting>(configuration.GetSection(nameof(MailSetting)));
        services.AddHttpContextAccessor();
        return services;
    }

}