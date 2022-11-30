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

/// <summary>
/// 表に表示させる文字の一覧。
/// </summary>
public static class Result_chars
{
    /// <summary>
    /// チェック項目が正常だった時に表示する文字列
    /// </summary>
    public const char OK = '〇';

    /// <summary>
    /// チェック項目がダメだった時の文字
    /// </summary>
    public const char NG = '×';
    /// <summary>
    /// チェックを行っていない時の文字。チェック前の項目だったり、
    /// 前提となる条件から外れてチェックが行われなかった場合
    /// </summary>
    public const char NO_CHECK = '-';
    /// <summary>
    /// ワーク自体の合否には関係ないが、NGだった項目があったとき。
    /// 極性がない部品の向きなど
    /// </summary>
    public const char NO_GOOD = '△';
}


abstract  class ba
{
    abstract ba();
}

class o:ba
{
    override ba()
    {

    }
}