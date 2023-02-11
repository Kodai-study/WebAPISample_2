using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text;
using WebAPISample.Data;

namespace WebAPISample.Query
{
    /// <summary>
    /// 時間に関するクエリを管理するクラス
    /// </summary>
    public class ResultSearchParams
    {
        /// <summary>
        ///  検査項目を指定することで、その項目が不合格になったデータのみを
        ///  指定して表示する
        /// </summary>
        [FromQuery(Name = "ng_colum")]
        public String searchByNGColums { get; set; } = "";

        /// <summary>
        ///  全体の検査結果に対して、OK,NGを指定して
        ///  絞り込みの後に表示する。
        /// </summary>
        [FromQuery(Name = "result")]
        public String searchByResult { get; set; } = "";

        /// <summary>
        ///  表示項目
        /// </summary>
        private readonly List<String> ERROR_CODES =
            InspectionParameters.ERROR_CODES.Keys.ToList();

        /// <summary>
        ///  不合格になった検査項目による絞り込みの
        ///  条件が指定されているかどうか
        /// </summary>
        public bool IsSetNgColums
        {
            get { return searchByNGColums != null && searchByNGColums != ""; }
        }

        /// <summary>
        ///  NG項目による絞り込みが指定されているかどうか
        /// </summary>
        public bool IsSetTermsResult
        {
            get { return searchByResult != null && searchByResult != ""; }
        }

        /// <summary>
        ///  絞り込みの項目が指定されているかどうか
        /// </summary>
        public bool IsSetParams
        {
            get { return IsSetNgColums || IsSetTermsResult; }
        }

        /// <summary>
        ///  条件を指定するSQL文を取得する。
        ///  検査項目と、結果のOK,NGの指定の両方がある場合は、
        ///  検査項目の指定が優先される
        /// </summary>
        /// <returns>
        ///  WHERE 以降に書くSQL文の文字列。
        /// </returns>
        public String CreateSQL()
        {
            /* NGになった項目が指定されていた場合、その項目のみを選択して表示する。 */
            if (IsSetNgColums)
            {
                if (ERROR_CODES.IndexOf(searchByNGColums) != -1)
                {
                    return String.Format(" result_Code = '{0}'  ", searchByNGColums);
                }
                else if (searchByNGColums.ToUpper() == ("VOLTAGE"))
                {
                    return String.Format(" (Volt BETWEEN {0} AND {1})",
                        InspectionParameters.VOLTAGE_MIN,InspectionParameters.VOLTAGE_MAX);
                }
                else if (searchByNGColums.ToUpper() == ("FREQENCY"))
                {
                    return String.Format(" (Freq BETWEEN {0} AND {1}) ",
                        InspectionParameters.FREQENCY_MIN,InspectionParameters.FREQENCY_MAX);
                }
                return "";
            }
            /* 検査結果が指定されたときは、検査結果がOK,NGかで絞り込みを行う */
            else if (IsSetTermsResult)
            {
                if (searchByResult.ToUpper() == ("OK"))
                {
                    return " result_Code = 'OK  ' ";
                }
                else if (searchByResult.ToUpper() == ("NG"))
                {
                    return " not result_Code = 'OK  ' ";
                }
            }
            return "";
        }
    }
}