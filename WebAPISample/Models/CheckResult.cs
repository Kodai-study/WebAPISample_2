using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.JSInterop;
using NuGet.Protocol.Plugins;

namespace WebAPISample.Models
{
    using r = KeyValuePair<int, Result.IC>;
    using ONE = Char;

    public class CheckResult
    {

        public CheckResult(int workID)
        {
            this.workID = workID;
        }
        public CheckResult(int workID, float temprature, float humidity, float brightness, bool allResult)
        {
            this.workID = workID;
            this.workID = workID;
            this.temprature = temprature;
            this.humidity = humidity;
            this.brightness = brightness;
            this.result = new Result(true);
        }
        public CheckResult(int workID, float temprature, float humidity, float brightness, List<string> errCodes)
        {
            this.workID = workID;
            this.temprature = temprature;
            this.humidity = humidity;
            this.brightness = brightness;
            this.result = new Result(errCodes);
        }

        public int? workID { get; set; } = null;
        public float? temprature { get; set; } = null;
        public float? humidity { get; set; } = null;
        public float? brightness { get; set; } = null;
        public Result? result { set; get; } = null;
    }

    public class Result
    {
        public Result()
        {
            ic = new IC();
            work = new WORK();
            r = new R();
            diode = new Diode();
            led = new LED();
        }
        public Result(bool result)
        {
            ic = new IC(result);
            work = new WORK(result);
            r = new R(result);
            diode = new Diode(result);
            tr = new TR(true);
            this.dipSw = new DipSW(true);
            led = new LED(result);
        }

        public Result(List<string> errCodes)
        {
            List<int>[] errors = new List<int>[(int)Parameters.Parts.ALL_OK];
            foreach (var e in errCodes)
            {
                var val = Parameters.ERROR_CODES[e];
                if (errors[(int)val.Item1] == null)
                {
                    errors[(int)val.Item1] = new List<int>();
                }
                errors[(int)val.Item1].Add(val.Item2);
            }

            if (errors[(int)Parameters.Parts.IC] == null)
                ic = new IC(true);
            else
                ic = new IC(errors[(int)Parameters.Parts.IC]);


            if (errors[(int)Parameters.Parts.WORK] == null)
                work = new WORK(true);
            else
                work = new WORK(errors[(int)Parameters.Parts.WORK]);



            if (errors[(int)Parameters.Parts.RESISTER] == null)
                r = new R(true);
            else
                r = new R(errors[(int)Parameters.Parts.RESISTER]);


            if (errors[(int)Parameters.Parts.TR] == null)
                tr = new TR(true);
            else
                tr = new TR(errors[(int)Parameters.Parts.TR]);

            if (errors[(int)Parameters.Parts.DIPSW] == null)
                dipSw = new DipSW(true);
            else
            {
                int b = (errors[(int)Parameters.Parts.DIPSW])[0];
                dipSw = new DipSW((byte)b);
            }

            if (errors[(int)Parameters.Parts.LED] == null)
                led = new LED(true);
            else
                led = new LED(errors[(int)Parameters.Parts.LED]);

            if (errors[(int)Parameters.Parts.DIODE] == null)
                diode = new Diode(true);
            else
                diode = new Diode(errors[(int)Parameters.Parts.DIODE]);
        }

        public enum result
        {
            NO_CHECK = -1,
            NG,
            OK
        }

        public IC ic { get; set; }
        public WORK work { get; set; }
        public R r { set; get; }
        public Diode diode { set; get; }
        public LED led { set; get; }
        public TR tr { set; get; }
        public DipSW dipSw { set; get; }
        public struct IC
        {
            public IC() {; }
            public IC(bool result)
            {
                if (result)
                {
                    IC1_dir = Result_chars.OK;
                    IC2_dir = Result_chars.OK;
                    IC1_have = Result_chars.OK;
                    IC2_have = Result_chars.OK;
                    allResult = Result_chars.OK;
                }
            }

            public IC(List<int> errors) : this(true)
            {
                allResult = Result_chars.NG;
                foreach (var e in errors)
                {
                    switch (e)
                    {
                        case 0: IC1_dir = Result_chars.NG; break;
                        case 1: IC2_dir = Result_chars.NG; break;
                        case 2: IC1_have = Result_chars.NG; break;
                        case 3: IC2_have = Result_chars.NG; break;
                        default: allResult = Result_chars.NO_CHECK; break;
                    }
                }
            }

            public void setError(int errIndex)
            {
                this.allResult = Result_chars.NG;
                switch (errIndex)
                {
                    case 1: IC1_dir = Result_chars.NG; break;
                    case 2: IC2_dir = Result_chars.NG; break;
                    case 3: IC1_have = Result_chars.NG; break;
                    case 4: IC2_have = Result_chars.NG; break;
                    default: break;
                }
            }
            public ONE allResult { get; set; } = Result_chars.NO_CHECK;
            public ONE IC1_dir { get; set; } = Result_chars.NO_CHECK;
            public ONE IC2_dir { get; set; } = Result_chars.NO_CHECK;
            public ONE IC1_have { get; set; } = Result_chars.NO_CHECK;
            public ONE IC2_have { get; set; } = Result_chars.NO_CHECK;
        }

        public struct WORK
        {
            public WORK() {; }
            public WORK(bool result)
            {
                if (result)
                {
                    this.result = Result_chars.OK;
                    this.dir = Result_chars.OK;
                    this.is_OK = Result_chars.OK;
                }
            }

            public WORK(List<int> errors) : this(true)
            {
                switch (errors[0])
                {
                    case 0: this.dir = Result_chars.NG; result = Result_chars.NO_GOOD; break;
                    case 1: this.is_OK = Result_chars.NG; result = Result_chars.NG; break;
                    default: result = Result_chars.NO_CHECK; break;
                }
            }

            public char result { set; get; } = Result_chars.NO_CHECK;
            public char dir { set; get; } = Result_chars.NO_CHECK;
            public char is_OK { set; get; } = Result_chars.NO_CHECK;
        }

        /*TODO hoge*/
        public struct R
        {
            public R() { Array.Fill(results, Result_chars.NO_CHECK); }

            public R(bool result)
            {
                if (result)
                {
                    Array.Fill(results, Result_chars.OK);
                    this.result = Result_chars.OK;
                }
            }

            public R(List<int> errors) : this(true)
            {
                foreach (var e in errors)
                {
                    if (e % 2 == 0)
                    {
                        results[e / 2] = Result_chars.NO_GOOD;
                        if (result == Result_chars.OK)
                            result = Result_chars.NO_GOOD;
                    }
                    else
                    {
                        result = Result_chars.NG;
                        results[e / 2] = Result_chars.NG;
                    }
                }
            }
            public char result { set; get; } = Result_chars.NO_CHECK;
            public char[] results { set; get; } = new char[15];
        }

        public struct TR
        {
            public TR(bool result)
            {
                if (result)
                {
                    allResult = Result_chars.OK;
                    dir = Result_chars.OK;
                    is_OK = Result_chars.OK;
                }
            }

            public TR(List<int> errors) : this(true)
            {
                allResult = Result_chars.OK;
                foreach (int e in errors)
                {
                    switch (e)
                    {
                        case 0: dir = Result_chars.NG; break;
                        case 1: is_OK = Result_chars.NG; break;
                        default: allResult = Result_chars.NO_CHECK; break;
                    }
                }
            }

            public char allResult { set; get; } = Result_chars.NO_CHECK;
            public char dir { set; get; } = Result_chars.NO_CHECK;
            public char is_OK { set; get; } = Result_chars.NO_CHECK;
        }

        public struct DipSW
        {
            public DipSW(bool result)
            {
                allResult = Result_chars.OK;
                pattern = "1000";
            }

            public DipSW(byte pattern)
            {
                allResult = Result_chars.NG;
                string bits = Convert.ToString((pattern), 2).PadLeft(4, '0');
                this.pattern = bits;
            }

            public char allResult { set; get; }
            public string? pattern { set; get; }
        }

        public struct LED
        {
            public LED(bool result)
            {

                this.allResult = Result_chars.OK;
                this.redDir = Result_chars.OK;
                this.greenDir = Result_chars.OK;
                this.whiteDir = Result_chars.OK;
                this.redHave = Result_chars.OK;
                this.greenHave = Result_chars.OK;
                this.whiteHave = Result_chars.OK;
            }

            public LED(List<int> code) : this(true)
            {
                allResult = Result_chars.NG;
                foreach (var e in code)
                {
                    switch (e)
                    {
                        case 0: this.redDir = Result_chars.NG; break;
                        case 1: this.redHave = Result_chars.NG; break;
                        case 2: this.greenDir = Result_chars.NG; break;
                        case 3: this.greenHave = Result_chars.NG; break;
                        case 4: this.whiteDir = Result_chars.NG; break;
                        case 5: this.whiteHave = Result_chars.NG; break;
                        default: this.allResult = Result_chars.NO_CHECK; break;
                    }
                }
            }

            public char allResult { set; get; }
            public char redDir { set; get; }
            public char greenDir { set; get; }
            public char whiteDir { set; get; }
            public char redHave { set; get; }
            public char greenHave { set; get; }
            public char whiteHave { set; get; }

        }

        public struct Diode
        {
            public Diode(bool result)
            {
                this.allResult = Result_chars.OK;
                this.dir = Result_chars.OK;
                this.have = Result_chars.OK;
            }

            public Diode(List<int> codes) : this(true)
            {
                this.allResult = Result_chars.NG;
                foreach (var e in codes)
                {
                    switch (e)
                    {
                        case 0: this.dir = Result_chars.NG; break;
                        case 1: this.have = Result_chars.NG; break;
                        default: this.allResult = Result_chars.NO_CHECK; break;
                    }
                }
            }

            public char allResult { set; get; }
            public char dir { set; get; }
            public char have { set; get; }
        }

    }
}