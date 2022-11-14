using System.Data.SqlClient;
using System;
using Microsoft.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore.Infrastructure;
using WebAPISample.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebAPISample.Modules.Class;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Parameters;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<cs>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("cs") ?? throw new InvalidOperationException("Connection string 'cs' not found.")));

// Add services to the container.

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

Parameters.sqlConnection = new SqlConnection("Data Source=RBPC12;Initial Catalog=Robot22_DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
Parameters.sqlConnection.Open();

using (var command = new SqlCommand("SELECT test_code FROM Test_Content order by test_code", Parameters.sqlConnection))
using (var reader = command.ExecuteReader())
{
    int[] indexs = new int[(int)Parts.ALL_OK];
    while (reader.Read())
    {
        string erCode = (string)reader[0];
        Tuple<Parts, int> value;

        switch (erCode.Substring(0, 2))
        {
            case "BS": value = new Tuple<Parts, int>(Parts.BSOCKET, indexs[(int)Parts.BSOCKET]++); break;
            case "DI": value = new Tuple<Parts, int>(Parts.DIODE, indexs[(int)Parts.DIODE]++); break;
            case "DS": value = new Tuple<Parts, int>(Parts.DIPSW, indexs[(int)Parts.DIPSW]++); break;
            case "IC": value = new Tuple<Parts, int>(Parts.IC, indexs[(int)Parts.IC]++); break;
            case "LE": value = new Tuple<Parts, int>(Parts.LED, indexs[(int)Parts.LED]++); break;
            case "RE": value = new Tuple<Parts, int>(Parts.RESISTER, indexs[(int)Parts.RESISTER]++); break;
            case "TR": value = new Tuple<Parts, int>(Parts.TR, indexs[(int)Parts.TR]++); break;
            case "WK": value = new Tuple<Parts, int>(Parts.WORK, indexs[(int)Parts.WORK]++); break;
            default: value = new Tuple<Parts, int>(Parts.ALL_OK, 0); break;
        }
        Parameters.ERROR_CODES.Add(erCode, value);
    }
}


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
    static public Dictionary<string, Tuple<Parts,int>> ERROR_CODES = new Dictionary<string, Tuple<Parts, int>>();
}