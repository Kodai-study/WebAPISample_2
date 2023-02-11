using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using static WebAPISample.Data.InspectionParameters;

namespace WebAPISample.Data
{
    /// <summary>
    ///  
    /// </summary>
    static public class InspectionParameters
    {
        public static SqlConnection? sqlConnection = null;

        public const String DEFAULT_CONNECTION_STRING =
            "Data Source=RBPC12;Initial Catalog=Robot22_2DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public const float VOLTAGE_MIN = 1.55f;
        public const float VOLTAGE_MAX = 1.7f;

        public const int FREQENCY_MIN = 300;
        public const int FREQENCY_MAX = 400;

        public enum Parts
        {
            IC,
            DIPSW,
            RESISTER,
            WORK,
            ALL_OK
        }

        public static Dictionary<string, Tuple<Parts, int>> ERROR_CODES = new();
            

        public static void Init()
        {
            Init(DEFAULT_CONNECTION_STRING);
        }

        public static void Init(String connectionString)
        {
            sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            errorCodeInit();
        }

        /// <summary>
        ///  外観検査のエラーコード表の初期化を行う。
        ///  パーツごとに、検査失敗の項目
        /// </summary>
        private static void errorCodeInit()
        {
            using (var command = new SqlCommand("SELECT Result_Code FROM Test_Content order by Result_Code", InspectionParameters.sqlConnection))
            using (var reader = command.ExecuteReader())
            {
                int[] indexs = new int[(int)Parts.ALL_OK];
                while (reader.Read())
                {
                    string errorCode = reader.GetString(0);
                    Tuple<Parts, int> value = errorCode[..2] switch
                    {
                        "DS" => new Tuple<Parts, int>(Parts.DIPSW, indexs[(int)Parts.DIPSW]++),
                        "IC" => new Tuple<Parts, int>(Parts.IC, indexs[(int)Parts.IC]++),
                        "R0" => new Tuple<Parts, int>(Parts.RESISTER, indexs[(int)Parts.RESISTER]++),
                        "R1" => new Tuple<Parts, int>(Parts.RESISTER, indexs[(int)Parts.RESISTER]++),
                        "WK" => new Tuple<Parts, int>(Parts.WORK, indexs[(int)Parts.WORK]++),
                        _ => new Tuple<Parts, int>(Parts.ALL_OK, 0),
                    };
                    ERROR_CODES.Add(errorCode, value);
                }
            }
        }

    }
}