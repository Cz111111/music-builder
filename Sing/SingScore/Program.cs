using Microsoft.EntityFrameworkCore;

namespace SingScore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            String? connnectionString = builder.Configuration.GetConnectionString("DB");
      //      builder.Services.AddDbContext<RecordDbContext>(opt => opt.UseMySQL(connnectionString));
    //        builder.Services.AddScoped<RecordService>();
  //          builder.Services.AddScoped<UserService>();
//            builder.Services.AddScoped<PostService>();
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
