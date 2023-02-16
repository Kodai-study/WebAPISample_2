//#define debug

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebAPISample.Data;
using WebAPISample.JSONModels;
using static WebAPISample.Data.InspectionParameters;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<cs>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("cs") ?? throw new InvalidOperationException("Connection string 'cs' not found.")));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseDeveloperExceptionPage();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

#if debug
InspectionParameters.Init("Data Source=RBPC04\\SQLEXPRESS;Initial Catalog=Robot22_2DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
#else
InspectionParameters.Init();
#endif
app.Run();