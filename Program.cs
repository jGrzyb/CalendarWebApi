using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using projekt.Data;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
// builder.Services.AddDbContext<DataBaseContext>(options =>
//     options.UseSqlServer(builder.Configuration.GetConnectionString("DataBaseContext") ?? throw new InvalidOperationException("Connection string 'DataBaseContext' not found.")));

builder.Services.AddDbContext<DataBaseContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DataBaseContext")
                      ?? throw new InvalidOperationException("Connection string 'DataBaseContext' not found.")
    )
);

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("AuthDataBaseContext")
                      ?? throw new InvalidOperationException("Connection string 'DataBaseContext' not found.")
    )
);
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AuthDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
    {
        options.LoginPath = new PathString("/Login/Login");
        options.AccessDeniedPath = new PathString("/Shared/AccessDenied");
    }
);

builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<DataBaseContext>();
dbContext.Database.EnsureCreated();
var authContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
authContext.Database.EnsureCreated();

app.Run();


// var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]!);
// builder.Services.AddAuthentication(x =>
//         {
//             x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//             x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//         }
//     )
//     .AddJwtBearer(x =>
//         {
//             x.RequireHttpsMetadata = false;
//             x.SaveToken = true;
//             x.TokenValidationParameters = new TokenValidationParameters
//             {
//                 ValidateIssuer = true,
//                 ValidateAudience = true,
//                 ValidateLifetime = true,
//                 ValidateIssuerSigningKey = true,
//                 ValidIssuer = builder.Configuration["Jwt:Issuer"],
//                 ValidAudience = builder.Configuration["Jwt:Audience"],
//                 IssuerSigningKey = new SymmetricSecurityKey(key)
//             };
//             x.Events = new JwtBearerEvents
//             {
//                 OnMessageReceived = context =>
//                 {
//                     if (context.Request.Cookies.ContainsKey("jwt")
//                         && string.IsNullOrEmpty(context.Token)
//                         && new JwtSecurityTokenHandler().ReadJwtToken(context.Request.Cookies["jwt"]).ValidTo > DateTime.UtcNow
//                        )
//                     {
//                         context.Token = context.Request.Cookies["jwt"];
//                     }
//
//                     return Task.CompletedTask;
//                 }
//             };
//             // x.Events = new JwtBearerEvents {
//             //     OnMessageReceived = context => {
//             //         if (context.Request.Cookies.ContainsKey("jwt")) {
//             //                 context.Token = context.Request.Cookies["jwt"];
//             //         }
//             //         return Task.CompletedTask;
//             //     },
//             //     OnAuthenticationFailed = context => {
//             //         Console.WriteLine("Authentication failed");
//             //         context.Response.Redirect("/login");
//             //         return Task.CompletedTask;
//             //     },
//             //     OnChallenge = context => {
//             //         if(!context.Response.HasStarted) {
//             //             context.Response.Redirect("/login");
//             //         }
//             //         return Task.CompletedTask;
//             //     }
//             // };
//         }
//     );