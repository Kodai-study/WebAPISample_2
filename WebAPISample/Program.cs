using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebAPISample.Modules.Class;
using static Parameters;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<cs>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("cs") ?? throw new InvalidOperationException("Connection string 'cs' not found.")));


builder.Services.AddControllers();
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

app.MapControllers();

Parameters.sqlConnection = new SqlConnection("Data Source=RBPC12;Initial Catalog=Robot22_2DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
Parameters.sqlConnection.Open();



app.Run();

/// <summary>
/// 
/// </summary>
static public class Parameters
{
    public static SqlConnection? sqlConnection = null;
    public enum Parts
    {
        BSOCKET,
        DIODE,
        DIPSW,
        IC,
        LED,
        RESISTER,
        TR,
        WORK,
        ALL_OK
    }
    static public Dictionary<string, Tuple<Parts, int>> ERROR_CODES = new Dictionary<string, Tuple<Parts, int>>();
}