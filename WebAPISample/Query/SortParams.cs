using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text;
using WebAPISample.Data;

namespace WebAPISample.Query
{
    /// <summary>
    /// 時間に関するクエリを管理するクラス
    /// </summary>
    public class SortParams
    {
        /// <summary>
        ///  検査項目を指定することで、その項目が不合格になったデータのみを
        ///  指定して表示する
        /// </summary>
        [FromQuery(Name = "sortColum")]
        public String sortColum { get; set; } = "";

        /// <summary>
        ///  全体の検査結果に対して、OK,NGを指定して
        ///  絞り込みの後に表示する。
        /// </summary>
        [FromQuery(Name = "orderBy")]
        public String sortingMethod { get; set; } = "";

        /// <summary>
        ///  表示項目
        /// </summary>
        private readonly List<String> ERROR_CODES =
            InspectionParameters.ERROR_CODES.Keys.ToList();

        /// <summary>
        ///  不合格になった検査項目による絞り込みの
        ///  条件が指定されているかどうか
        /// </summary>
        public bool IsSetSortColum
        {
            get { return sortColum != null && sortColum != ""; }
        }

        /// <summary>
        ///  NG項目による絞り込みが指定されているかどうか
        /// </summary>
        public bool IsSetSotringMethod
        {
            get
            {
                return sortingMethod != null && sortingMethod != "" &&
                    (sortingMethod.ToUpper() == "ASC" || sortingMethod.ToUpper() == "DESC");
            }
        }

        /// <summary>
        ///  絞り込みの項目が指定されているかどうか
        /// </summary>
        public bool IsSetAnyParams
        {
            get { return IsSetSortColum || IsSetSotringMethod; }
        }

        public String CreateSQL(String defaultSortColum, String? defaultSortingMethod=null)
        {
            StringBuilder sb = new StringBuilder(" ORDER BY ");
            if (IsSetSortColum)
                sb.Append(sortColum);
            else
                sb.Append(defaultSortColum);

            sb.Append(' ');

            if (IsSetSotringMethod)
                sb.Append(sortingMethod.ToUpper());
            else
            {
                if (defaultSortingMethod == null)

                    sb.Append(" ASC ");
                else
                    sb.Append(defaultSortingMethod);

            }
            return sb.ToString();
        }

    }
}