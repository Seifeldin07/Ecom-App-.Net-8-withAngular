#region das
//using AutoMapper;
//using Ecom.API.Mapping;
//using Ecom.API.Middleware;
//using Ecom.infrastructure;

//namespace Ecom.API
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            var builder = WebApplication.CreateBuilder(args);

//            // Add services to the container.
//            builder.Services.AddCors(op=>
//            {
//                op.AddPolicy("CORSPolicy", builder =>
//                {
//                    builder.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:4200");
//                });
//            });
//            builder.Services.AddMemoryCache();
//            builder.Services.AddControllers();
//            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//            builder.Services.AddEndpointsApiExplorer();
//            builder.Services.AddSwaggerGen();
//            builder.Services.infrastructureConfiguration(builder.Configuration);

//            builder.Services.AddAutoMapper(cfg =>
//            {
//            }, typeof(CategoryMapping).Assembly);

//            var app = builder.Build();

//            // Configure the HTTP request pipeline.
//            if (app.Environment.IsDevelopment())
//            {
//                app.UseSwagger();
//                app.UseSwaggerUI();
//            }
//            app.UseCors("CORSPolicy");
//            app.UseMiddleware<ExceptionMiddleware>();
//            app.UseStaticFiles();
//            app.UseStatusCodePagesWithReExecute("/errors/{0}");
//            app.UseHttpsRedirection();

//            app.UseAuthorization();


//            app.MapControllers();

//            app.Run();
//        }
//    }
//}

//using AutoMapper;
//using Ecom.API.Mapping;
//using Ecom.API.Middleware;
//using Ecom.infrastructure;

//namespace Ecom.API
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            var builder = WebApplication.CreateBuilder(args);

//            // 1. Add CORS
//            builder.Services.AddCors(op =>
//            {
//                op.AddPolicy("CORSPolicy", policy =>
//                {
//                    policy.AllowAnyHeader()
//                          .AllowAnyMethod()
//                          .AllowCredentials()
//                          .WithOrigins("http://localhost:4200");
//                });
//            });

//            // 2. Add Services
//            builder.Services.AddMemoryCache();
//            builder.Services.AddControllers();
//            builder.Services.AddEndpointsApiExplorer();
//            builder.Services.AddSwaggerGen();

//            // Infrastructure configuration (DbContext, Redis, Repositories)
//            builder.Services.infrastructureConfiguration(builder.Configuration);

//            builder.Services.AddAutoMapper(cfg =>
//            {
//            }, typeof(CategoryMapping).Assembly);

//            var app = builder.Build();

//            // 3. Configure HTTP Pipeline
//            app.UseMiddleware<ExceptionMiddleware>();
//            app.UseAuthentication();
//            app.UseAuthorization();

//            app.UseStatusCodePagesWithReExecute("/errors/{0}");

//            if (app.Environment.IsDevelopment())
//            {
//                app.UseSwagger();
//                app.UseSwaggerUI();
//            }

//            app.UseHttpsRedirection(); // نقل الهيدر هنا لعدم كسر الـ CORS

//            app.UseStaticFiles();

//            app.UseCors("CORSPolicy"); // تطبيق سياسة CORS بشكل صحيح



//            app.MapControllers();

//            app.Run();
//        }
//    }
//}
#endregion
using AutoMapper;
using Ecom.API.Mapping;
using Ecom.API.Middleware;
using Ecom.infrastructure;

namespace Ecom.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. Add CORS
            builder.Services.AddCors(op =>
            {
                op.AddPolicy("CORSPolicy", policy =>
                {
                    policy.AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials()
                          .WithOrigins("http://localhost:4200");
                });
            });

            // 2. Add Services
            builder.Services.AddMemoryCache();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Infrastructure configuration (DbContext, Redis, Repositories)
            builder.Services.infrastructureConfiguration(builder.Configuration);

            builder.Services.AddAutoMapper(cfg =>
            {
            }, typeof(CategoryMapping).Assembly);

            var app = builder.Build();

            // 3. Configure HTTP Pipeline 
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("CORSPolicy");

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles();

            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseHttpsRedirection();

            app.MapControllers();

            app.Run();
        }
    }
}