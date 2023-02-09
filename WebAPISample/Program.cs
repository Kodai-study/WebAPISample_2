//#define debug

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebAPISample.Modules;
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

#if debug
Parameters.sqlConnection = new SqlConnection("Data Source=RBPC11;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
Parameters.sqlConnection.Open();
#else
Parameters.sqlConnection = new SqlConnection("Data Source=RBPC12;Initial Catalog=Robot22_2DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
Parameters.sqlConnection.Open();
using (var command = new SqlCommand("SELECT Result_Code FROM Test_Content order by Result_Code", Parameters.sqlConnection))
using (var reader = command.ExecuteReader())
{
    int[] indexs = new int[(int)Parts.ALL_OK];
    while (reader.Read())
    {
        string erCode = (string)reader[0];
        Tuple<Parts, int> value = erCode.Substring(0, 2) switch
        {
            "DS" => new Tuple<Parts, int>(Parts.DIPSW, indexs[(int)Parts.DIPSW]++),
            "IC" => new Tuple<Parts, int>(Parts.IC, indexs[(int)Parts.IC]++),
            "R0" => new Tuple<Parts, int>(Parts.RESISTER, indexs[(int)Parts.RESISTER]++),
            "R1" => new Tuple<Parts, int>(Parts.RESISTER, indexs[(int)Parts.RESISTER]++),
            "WK" => new Tuple<Parts, int>(Parts.WORK, indexs[(int)Parts.WORK]++),
            _ => new Tuple<Parts, int>(Parts.ALL_OK, 0),
        };
        Parameters.ERROR_CODES.Add(erCode, value);
    }
}
#endif
app.Run();

/// <summary>
/// 
/// </summary>
static public class Parameters
{
    public static SqlConnection? sqlConnection = null;
    public enum Parts
    {
        IC,
        DIPSW,
        RESISTER,
        WORK,
        ALL_OK
    }
    static public Dictionary<string, Tuple<Parts, int>> ERROR_CODES = new Dictionary<string, Tuple<Parts, int>>();
}