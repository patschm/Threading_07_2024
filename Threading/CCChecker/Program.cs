
namespace CCChecker
{
    public class Program
    {
        static Random rnd = new Random();

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            
            app.MapGet("/check/{cc}", (HttpContext httpContext, string cc) =>
            {
                return rnd.Next(0, 4) == 3;
            })
            .WithName("CheckCreditcard")
            .WithOpenApi();

            app.Run();
        }
    }
}
