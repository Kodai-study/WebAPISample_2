using Microsoft.AspNetCore.Mvc;

namespace WebAPISample.Modules
{

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
}