namespace WebAPISample.Modules
{
    public class DailyResults
    {
        public DateOnly firstDateOfRange { get; set; }
        public DateOnly endDateOfRange { get; set; }
        public int count_Scan { get; set; }
        public int count_Ok { get; set; }

        public int count_Ng { get; set; }

        public int ngCount_IC1 { get; set; }
        public int ngCount_IC2 { get; set; }
        public int ngCount_R5{ get; set; }
        public int ngCount_R10 { get; set; }
        public int ngCount_R11 { get; set; }
        public int ngCount_R12 { get; set; }
        public int ngCount_R18 { get; set; }
        public int ngCount_DIPSW { get; set; }
        public int ngCount_Voltage { get; set; }
        public int ngCount_Frequency { get; set; }

        public DailyResults(DateOnly firstDateOfRange, DateOnly endDateOfRange, int count_Scan, int count_Ok,int count_Ng, int ngCount_IC1, int ngCount_IC2, int ngCount_R5, int ngCount_R10, int ngCount_R11, int ngCount_R12, int ngCount_R18, int ngCount_DIPSW, int ngCount_Voltage, int ngCount_Frequency)
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
        }
    }
}