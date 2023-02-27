namespace WebAPISample.JSONModels
{
    /// <summary>
    ///  単位時間ごとの、検査数や検査項目ごとの数を表したモデル
    ///  
    /// </summary>
    public class DailyResults
    {
        /// <summary>
        ///  一つの項目で、範囲の始めの日付
        /// </summary>
        public DateTime firstDateOfRange { get; set; }

        /// <summary>
        ///  範囲の終わりの日付。
        ///  1日毎の場合は始めの日付と同じ値、
        ///  1週間の場合はその週の最終日、
        ///  1か月の場合は月の最終日
        /// </summary>
        public DateTime endDateOfRange { get; set; }

        /// <summary>
        ///  検査した項目の合計
        /// </summary>
        public int count_Scan { get; set; }

        /// <summary>
        ///  合格した個数
        /// </summary>
        public int count_Ok { get; set; }

        /// <summary>
        ///  不合格だったワークの個数
        /// </summary>
        public int count_Ng { get; set; }


        public float defectRate { get; set; }

        /// <summary>
        ///  外観検査で、IC1の項目で不合格だったワークの個数
        /// </summary>
        public int ngCount_IC1 { get; set; }

        /// <summary>
        ///  外観検査で、IC1の項目で不合格だったワークの個数
        /// </summary>
        public int ngCount_IC2 { get; set; }

        /// <summary>
        ///  外観検査で、抵抗器 R5の項目で不合格だったワークの個数
        /// </summary>
        public int ngCount_R5 { get; set; }
        public int ngCount_R10 { get; set; }
        public int ngCount_R11 { get; set; }
        public int ngCount_R12 { get; set; }
        public int ngCount_R18 { get; set; }
        public int ngCount_DIPSW { get; set; }
        public int ngCount_Voltage { get; set; }
        public int ngCount_Frequency { get; set; }


        public DailyResults(DateTime firstDateOfRange, DateTime endDateOfRange, int count_Scan, int count_Ok, int count_Ng, int ngCount_IC1, int ngCount_IC2, int ngCount_R5, int ngCount_R10, int ngCount_R11, int ngCount_R12, int ngCount_R18, int ngCount_DIPSW, int ngCount_Voltage, int ngCount_Frequency)
        {
            this.firstDateOfRange = firstDateOfRange;
            this.endDateOfRange = endDateOfRange;
            this.count_Scan = count_Scan;
            this.count_Ok = count_Ok;
            this.count_Ng = count_Ng;
            this.ngCount_IC1 = ngCount_IC1;
            this.ngCount_IC2 = ngCount_IC2;
            this.ngCount_R5 = ngCount_R5;
            this.ngCount_R10 = ngCount_R10;
            this.ngCount_R11 = ngCount_R11;
            this.ngCount_R12 = ngCount_R12;
            this.ngCount_R18 = ngCount_R18;
            this.ngCount_DIPSW = ngCount_DIPSW;
            this.ngCount_Voltage = ngCount_Voltage;
            this.ngCount_Frequency = ngCount_Frequency;
            if (count_Ok == 0)
                this.defectRate = 0f;
            else
                this.defectRate = ((float)count_Ok / count_Scan) * 100;
        }
    }
}