namespace WebAPISample.JSONModels
{
    using System.Reflection.Metadata;
    using WebAPISample.Data;
    using WebAPISample.JSONModels;
    using WebAPISample.Modules;
    using ONE = Char;
    /// <summary>
    /// 検査情報を表すクラス
    /// </summary>
    public class CheckResult
    {

        /// <summary>
        /// 全てOK、もしくはNGだった時のコンストラクタ。
        /// 全ての項目にOKが入る。
        /// </summary>
        /// <param name="allResult">すべてOK(True)かすべてNG(False)</param>
        public CheckResult(int workID, float temprature, float humidity, float brightness, bool allResult)
        {
            this.workID = workID;
            this.workID = workID;
            this.temprature = temprature;
            this.humidity = humidity;
            this.brightness = brightness;
            this.AllResult = allResult ? Result_chars.OK : Result_chars.NG;
            this.result_visualInspection = new VisualInspectionResult(true);
        }



        public CheckResult(int workID, DateTime startTime, DateTime? endTime, List<string> errCodes,FunctionalInspectionResult functionResult)
        {
            this.startTime = startTime;
            this.workID = workID;
            this.result_visualInspection = new VisualInspectionResult(errCodes);
            AllResult = Result_chars.NG;
            if (endTime != null)
                this.cycleTime = endTime - startTime;
            this.result_functionalInspection = functionResult;
        }

        public CheckResult(int workID, DateTime startTime, DateTime? endTime, bool allok, FunctionalInspectionResult functionResult)
        {
            this.startTime = startTime;
            this.workID = workID;
            this.result_visualInspection = new VisualInspectionResult(allok);
            AllResult = Result_chars.OK;
            if (endTime != null)
                this.cycleTime = endTime - startTime;

            this.result_functionalInspection = functionResult;
        }

        /// <summary>
        /// 検査開始時刻(搬入コンベアのセンサに触れる)
        /// </summary>
        public DateTime startTime { get; set; }

        /// <summary>
        /// ワーク全体の合否
        /// </summary>
        public char AllResult { get; set; }

        /// <summary>
        /// ワークのID
        /// </summary>
        public int? workID { get; set; } = null;
        /// <summary>
        /// 検査が行われたとき(写真撮影時?)の温度
        /// </summary>
        public float? temprature { get; set; } = null;
        /// <summary>
        /// 検査が行われたとき(写真撮影時?)の湿度
        /// </summary>
        public float? humidity { get; set; } = null;
        /// <summary>
        /// 写真撮影時の照度
        /// </summary>
        public float? brightness { get; set; } = null;
        /// <summary>
        /// 検査結果を表すクラス
        /// </summary>
        public VisualInspectionResult? result_visualInspection { set; get; } = null;

        public FunctionalInspectionResult? result_functionalInspection { set; get; } = null;

        public TimeSpan? cycleTime { set; get; } = null;
    }

}