using Microsoft.AspNetCore.Mvc;

namespace WebAPISample.Models
{
    [Route("api/times")]
    [ApiController]
    public class Class : ControllerBase
    {
        public Class(int id, string? name, bool flg_data)
        {
            this.id = id;
            this.name = name;
            this.flg_data = flg_data;
        }

        public Class(int id, string? name)
        {
            this.id = id;
            this.name = name;
        }

        public Class()
        {
            id = 0;
            name = "noname";
            flg_data = false;
        }

        public int id { get; set; }
        public string? name { get; set; }
        public bool flg_data { get; set; }
    }
}

public static class Result_chars
{
    public const char OK = '〇';
    public const char NG = '×';
    public const char NO_CHECK = '-';
    public const char NO_GOOD = '△';
}
