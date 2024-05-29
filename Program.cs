using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DataBaseContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DataBaseContext") ?? throw new InvalidOperationException("Connection string 'DataBaseContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]!);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
    x.Events = new JwtBearerEvents {
        OnMessageReceived = context => {
            if (context.Request.Cookies.ContainsKey("jwt") 
                && string.IsNullOrEmpty(context.Token )
                && new JwtSecurityTokenHandler().ReadJwtToken(context.Request.Cookies["jwt"]).ValidTo > DateTime.UtcNow
            ) {
                    context.Token = context.Request.Cookies["jwt"];
            }
            return Task.CompletedTask;
        }
    };
    // x.Events = new JwtBearerEvents {
    //     OnMessageReceived = context => {
    //         if (context.Request.Cookies.ContainsKey("jwt")) {
    //                 context.Token = context.Request.Cookies["jwt"];
    //         }
    //         return Task.CompletedTask;
    //     },
    //     OnAuthenticationFailed = context => {
    //         Console.WriteLine("Authentication failed");
    //         context.Response.Redirect("/login");
    //         return Task.CompletedTask;
    //     },
    //     OnChallenge = context => {
    //         if(!context.Response.HasStarted) {
    //             context.Response.Redirect("/login");
    //         }
    //         return Task.CompletedTask;
    //     }
    // };
});
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();







// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.EntityFrameworkCore.Migrations.Operations;
// using Data;
// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.IdentityModel.Tokens;
// using System.Text;
// using System.IdentityModel.Tokens.Jwt;
// using Microsoft.AspNetCore.Authentication.Cookies;


// var builder = WebApplication.CreateBuilder(args);
// builder.Services.AddDbContext<DataBaseContext>(options =>
//     options.UseSqlite(builder.Configuration.GetConnectionString("DataBaseContext") ?? throw new InvalidOperationException("Connection string 'DataBaseContext' not found.")));

// // Add services to the container.
// builder.Services.AddControllersWithViews();
// builder.Services.AddSwaggerGen();

// var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);

// builder.Services.AddAuthorization();

// var app = builder.Build();

// // Configure the HTTP request pipeline.
// if (!app.Environment.IsDevelopment())
// {
//     app.UseExceptionHandler("/Home/Error");
//     // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//     app.UseHsts();
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

// app.UseHttpsRedirection();
// app.UseStaticFiles();

// app.UseRouting();

// app.UseAuthentication();
// app.UseAuthorization();

// app.Use(async (ctx, next) =>
// {
//     await next();

//     if(ctx.Response.StatusCode == 401)
//     {
//         //Re-execute the request so the user gets the error page
//         string originalPath = ctx.Request.Path.Value;
//         ctx.Items["originalPath"] = originalPath;
//         ctx.Request.Path = "/login";
//         await next();
//     }
// });

// app.MapControllerRoute(
//     name: "default",
//     pattern: "{controller=Home}/{action=Index}/{id?}");

// app.Run();
