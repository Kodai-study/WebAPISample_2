namespace WebAPISample.JSONModels
{
    using Microsoft.Data.SqlClient;
    using System.Reflection.Metadata;
    using WebAPISample.Data;
    using WebAPISample.JSONModels;
    using WebAPISample.Modules;
    using ONE = Char;
    /// <summary>
    /// 検査情報を表すクラス
    /// </summary>
    public class TotalInspectionDatas
    {
        public int count_Scan { get; set; }
        public int count_OK { get; set; }
        public int count_NG { get; set; }

        public int count_VisualInspectionNG { get; set; }
        public int count_FunctionalInspectionNG { get; set; }
        public int count_FrequencyNG { get; set; }
        public int count_VoltageNG { get; set; }
        public int count_VoltAndFreqNG { get; set; }


        public TotalInspectionDatas()
        {
            count_Scan = getOneColumWithSql(sqlArray[0]);
            count_OK = getOneColumWithSql(sqlArray[1]);
            count_VisualInspectionNG = getOneColumWithSql(sqlArray[2]);
            count_FrequencyNG = getOneColumWithSql(sqlArray[3]);
            count_VoltageNG = getOneColumWithSql(sqlArray[4]);

            count_NG = count_Scan - count_OK;
            count_FunctionalInspectionNG = count_NG - count_VisualInspectionNG;
            count_VoltAndFreqNG = count_FrequencyNG + count_VoltageNG - count_FunctionalInspectionNG;
        }

        private int getOneColumWithSql(String sql)
        {
            var errCommand = new SqlCommand(sql, InspectionParameters.sqlConnection);

            using SqlDataReader errReader = errCommand.ExecuteReader();
            if (errReader.Read())
            {
                return errReader.GetInt32(0);
            }
            return -1;
        }
        private readonly String[] sqlArray = { "SELECT COUNT(DISTINCT ID) FROM ALL_resultView",
        "SELECT COUNT(DISTINCT ID) FROM ALL_resultView WHERE result_Code = 'OK  'AND (Freq BETWEEN 300 AND 400) AND (Volt BETWEEN 1.5 AND 1.75)",
        "SELECT COUNT(DISTINCT ID) FROM ALL_resultView WHERE result_Code != 'OK  '",
        "SELECT COUNT(DISTINCT ID) FROM ALL_resultView WHERE NOT(Freq BETWEEN 300 AND 400)",
            "SELECT COUNT(DISTINCT ID) FROM ALL_resultView WHERE NOT(Volt BETWEEN 1.5 AND 1.75)"
        };
    }
}