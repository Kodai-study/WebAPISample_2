namespace WebAPISample.Models
{
    /*hello*/
    public class LogSample
    {
        public LogSample(DateTime? date, int userId, string? userName)
        {
            this.date = date;
            this.userId = userId;
            this.userName = userName;
        }


        public DateTime? date { get; set; }
        public int userId { get; set; }
        public string? userName { get; set; }
    }
}
