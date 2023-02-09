using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text;

namespace WebAPISample.Query
{
    /// <summary>
    /// 時間に関するクエリを管理するクラス
    /// </summary>
    public class ResultSearchParams
    {
        [FromQuery(Name = "ng_colum")]
        public String searchByNGColums { get; set; } = "";

        [FromQuery(Name = "result")]
        public String searchByResult { get; set; } = "";

        public bool IsSetNgColums
        {
            get
            {
                return searchByNGColums != null && !searchByNGColums.Equals("");
            }
        }

        public bool IsSetTermsResult
        {
            get { return searchByResult != null && !searchByResult.Equals(""); }
        }


        public bool IsSetParams
        {
            get { return IsSetNgColums || IsSetTermsResult; }
        }

        public String getTermsSql()
        {
            if (IsSetNgColums)
            {
                var e = Parameters.ERROR_CODES.Keys;
                if (e.ToList().IndexOf(searchByNGColums) != -1)
                {
                    return String.Format( " result_Code = '{0}'  ", searchByNGColums);
                }
                else if (searchByNGColums.ToUpper().Equals("VOLTAGE"))
                {
                    return " (Volt BETWEEN 1.55 AND 1.7)";
                }
                else if (searchByNGColums.ToUpper().Equals("FREQENCY"))
                {
                    return " (Freq BETWEEN 300 AND 400) ";
                }
                return "";
            }
            else if (IsSetTermsResult)
            {
                if (searchByResult.ToUpper().Equals( "OK"))
                {
                    return " result_Code = 'OK  ' ";
                }
                else if (searchByResult.ToUpper().Equals("NG"))
                {
                    return " not result_Code = 'OK  ' ";
                }
            }
            return "";
        }
    }
}