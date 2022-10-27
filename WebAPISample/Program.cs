using System.Data.SqlClient;
using System;
using Microsoft.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore.Infrastructure;
using WebAPISample.Models;

var builder = WebApplication.CreateBuilder(args);

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


using (var connection = new SqlConnection("Data Source=RBPC11;Initial Catalog=test;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
{
    // 接続開始
    connection.Open();

    // SqlCommand：DBにSQL文を送信するためのオブジェクトを生成
    // SqlDataReader：読み取ったデータを格納するためのオブジェクトを生成
    using (var command = new SqlCommand("select * from table1", connection))
    using (var reader = command.ExecuteReader())
    {
        // 1行ごとに読み取る。読み取ったらtrue
        //app.MapGet("/", () => new Class((int)reader[(string?)"value"], (string)reader["str"]));
        if (!reader.Read())
            return;


        int value = (int)reader[0];
        string str = (string)reader[1];
        var c = new Class(value, str);
        app.MapGet("/DBsample", () => c);
    }

}

app.Run();


