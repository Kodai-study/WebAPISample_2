using WebAPISample.Modules;
using static WebAPISample.JSONModels.VisualInspectionResult;

namespace WebAPISample.JSONModels
{
    /// <summary>
    /// サイクルタイムを、ステーションへの搬入、搬出時間で表したモデル
    /// </summary>
    public class FunctionalInspectionResult
    {
        public char voltage_result { get; set; }
        public char frequency_result { get; set; }
        public float voltage_value { get; set; }
        public int frequency_value { get; set; }

        public FunctionalInspectionResult(float voltage_value, int frequency_value)
        {
            this.voltage_value = voltage_value;
            this.frequency_value = frequency_value;

            if(voltage_result < 0)
            {
                voltage_result = Result_chars.NO_CHECK;
            }
            else if (voltage_value >= 1.55 && voltage_value <= 1.7)
            {
                voltage_result = Result_chars.OK;
            }
            else
            {
                voltage_result = Result_chars.NG;
            }

            if (frequency_result < 0)
            {
                frequency_result = Result_chars.NO_CHECK;
            }
            else if (frequency_value >= 300 && frequency_value <= 400)
            {
                frequency_result = Result_chars.OK;
            }
            else
            {
                frequency_result = Result_chars.NG;
            }
        }

    }
}