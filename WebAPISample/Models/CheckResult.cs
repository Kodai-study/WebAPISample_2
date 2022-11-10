using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using NuGet.Protocol.Plugins;

namespace WebAPISample.Models
{
    using r = KeyValuePair<int,Result.IC>;
    using ONE = Char;
    public class CheckResult
    {
        public int workID { get; set; } = 11;
        public DateTime checkDate { get; set; } = DateTime.Now;
        public float temprature { get; set; } = 11.1f;
        public float humidity { get; set; }
        public float Brightness { get; set; }
        public Result result { set; get; } = new Result();
    }



    public class Result
    {
        public enum result
        {
            NO_CHECK = -1,
            NG,
            OK
        }

        public IC ic { get; set; } = new IC();
        public WORK work { get; set; } = new WORK();
        public R r { set; get; } = new R();
        public char D { set; get; } = Result_chars.NO_CHACK;
        public char LED { set; get; } = Result_chars.NO_CHACK;
        public struct IC
        {
            public IC() { ; }
            public ONE IC1_dir { get; set; } = Result_chars.NO_CHACK;
            public ONE IC2_dir { get; set; } = Result_chars.NO_CHACK;
            public ONE IC1_have { get; set; } = Result_chars.NO_CHACK;
            public ONE IC2_jave { get; set; } = Result_chars.NO_CHACK;
        }

        public struct WORK
        {
            public WORK() { ; }
            public char result { set; get; } = Result_chars.NO_CHACK;
            public char dir { set; get; } = Result_chars.NO_CHACK;
            public char is_OK { set; get; } = Result_chars.NO_CHACK;
        }

        public struct R
        {
            public R() { Array.Fill(results,Result_chars.NO_CHACK) ; }
            public char result { set; get; } = Result_chars.NO_CHACK;
            public char[] results { set; get; } = new char[15];
            
        }
    }
}
