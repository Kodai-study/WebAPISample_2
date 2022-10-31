namespace WebAPISample.Models
{
    /*hello*/
    public class LogSample
    {
        public LogSample(DateTime? date,TimeSpan? time,int userId,string? userName)
        {
            this.date = date;
            this.time = time;   
            this.userId = userId;
            this.userName = userName;
        }
        
        public DateTime? date { get; set; }
        public TimeSpan? time { get; set; }
        public int userId { get; set; }
        public string? userName { get; set; }
    }
}
