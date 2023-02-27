using System.Security.Policy;
using WebAPISample.Data;
using WebAPISample.Modules;
using static WebAPISample.JSONModels.VisualInspectionResult;

namespace WebAPISample.JSONModels
{

    /// <summary>
    /// 検査結果を表すクラス
    /// JSONの形式の関係上クラス分けをした
    /// </summary>
    public class VisualInspectionResult
    {

        public IC ic { get; set; }

        public WORK work { get; set; }

        public R r { set; get; }

        public DipSW dipSw { set; get; }

  
        /// <summary>
        /// 検査コードにOKが返ってきたときに検査結果全てにOKを入れる
        /// </summary>
        /// <see cref="CheckResult.CheckResult(int, float, float, float, bool)"/>
        /// <param name="result"></param>
        public VisualInspectionResult(bool result)
        {
            ic = new IC(result);
            work = new WORK(result);
            r = new R(result);
            this.dipSw = new DipSW(true);
        }

        /// <summary>
        /// エラーコードから、検査結果の一覧を作成する
        /// </summary>
        /// <param name="errCodes"></param>
        public VisualInspectionResult(List<string> errCodes)
        {

            //パーツごとに、エラー項目の一覧を管理する(エラー項目のリスト、それらをまとめる配列)
            List<int>[] errors = new List<int>[(int)InspectionParameters.Parts.ALL_OK];
            foreach (var e in errCodes)
            {
                var val = InspectionParameters.ERROR_CODES[e];
                if (errors[(int)val.Item1] == null)
                {
                    errors[(int)val.Item1] = new List<int>();
                }
                errors[(int)val.Item1].Add(val.Item2);
            }
            //ICの項目の設定。エラー項目がなかったらALLOK、そうでなければエラーのリストを渡す
            if (errors[(int)InspectionParameters.Parts.IC] == null)
                ic = new IC(true);
            else
                ic = new IC(errors[(int)InspectionParameters.Parts.IC]);

            //ワークの項目の設定
            if (errors[(int)InspectionParameters.Parts.WORK] == null)
                work = new WORK(true);
            else
                work = new WORK(errors[(int)InspectionParameters.Parts.WORK]);

            //抵抗器の項目の設定
            if (errors[(int)InspectionParameters.Parts.RESISTER] == null)
                r = new R(true);
            else
                r = new R(errors[(int)InspectionParameters.Parts.RESISTER]);

            //DIPスイッチの項目の設定
            if (errors[(int)InspectionParameters.Parts.DIPSW] == null)
                dipSw = new DipSW(true);
            else
            {
                int b = (errors[(int)InspectionParameters.Parts.DIPSW])[0];
                dipSw = new DipSW((byte)b);
            }

        }




        /// <summary>
        /// ICの検査結果を表す構造体
        /// </summary>
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
            /// <summary>
            /// IC全体の合否(IC1,2のどちらもOKの時のみOK)
            /// </summary>
            public char allResult { get; set; } = Result_chars.NO_CHECK;

            /// <summary>
            /// IC1の向きが正しいかどうか。不合格になる
            /// </summary>
            public char IC1_dir { get; set; } = Result_chars.NO_CHECK;
            /// <summary>
            /// IC2の向きが正しいかどうか
            /// </summary>
            public char IC2_dir { get; set; } = Result_chars.NO_CHECK;
            /// <summary>
            /// IC1が存在するかどうか。
            /// </summary>
            public char IC1_have { get; set; } = Result_chars.NO_CHECK;
            public char IC2_have { get; set; } = Result_chars.NO_CHECK;
        }

        /// <summary>
        /// ワークの検査結果を表す構造体
        /// </summary>
        public struct WORK
        {
            public WORK() {; }
            public WORK(bool result)
            {
                if (result)
                {
                    this.allResult = Result_chars.OK;
                    this.dir = Result_chars.OK;
                    this.is_OK = Result_chars.OK;
                }
            }

            public WORK(List<int> errors) : this(true)
            {
                switch (errors[0])
                {
                    case 0: this.dir = Result_chars.NG; allResult = Result_chars.NO_GOOD; break;
                    case 1: this.is_OK = Result_chars.NG; allResult = Result_chars.NG; break;
                    default: allResult = Result_chars.NO_CHECK; break;
                }
            }
            /// <summary>
            /// ワーク自体の合否
            /// </summary>
            public char allResult { set; get; } = Result_chars.NO_CHECK;

            /// <summary>
            /// ワークの向きが正しい方向で運ばれてきたかどうか
            /// これがだめでも不合格にはならない
            /// </summary>
            public char dir { set; get; } = Result_chars.NO_CHECK;

            /// <summary>
            /// 正しいワークが流れてきたかどうか。
            /// ワークがなかった、違うワークが流れてきたときなど、
            /// 画像処理で正しく判別できなかった時に登録
            /// </summary>
            public char is_OK { set; get; } = Result_chars.NO_CHECK;
        }

        /// <summary>
        /// 抵抗器の検査結果を表す構造体
        /// </summary>
        public struct R
        {

            public R(bool result)
            {
                if (result)
                {
                    this.r05 = Result_chars.OK;
                    this.r10 = Result_chars.OK;
                    this.r11 = Result_chars.OK;
                    this.r12 = Result_chars.OK;
                    this.r18 = Result_chars.OK;
                    this.allResult = Result_chars.OK;
                }
            }

            public R(List<int> errors) : this(true)
            {
                foreach (var e in errors)
                {
                    switch (e)
                    {
                        case 0: r05 = Result_chars.NG; break;
                        case 1: r10 = Result_chars.NG; break;
                        case 2: r11 = Result_chars.NG; break;
                        case 3: r12 = Result_chars.NG; break;
                        case 4: r18 = Result_chars.NG; break;
                    }
                }
            }
            /// <summary>
            /// 
            /// </summary>
            public char allResult { set; get; } = Result_chars.NO_CHECK;

            public char r05 { set; get; } = Result_chars.NO_CHECK;
            public char r10 { set; get; } = Result_chars.NO_CHECK;
            public char r11 { set; get; } = Result_chars.NO_CHECK;
            public char r12 { set; get; } = Result_chars.NO_CHECK;
            public char r18 { set; get; } = Result_chars.NO_CHECK;


        }


        /// <summary>
        /// DIPスイッチの検査結果を表す構造体
        /// </summary>
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
            /// <summary>
            /// DIPスイッチの実装、パターンが正しいかどうか
            /// </summary>
            public char allResult { set; get; }
            /// <summary>
            /// DIPスイッチのパターン。2進数4桁
            /// </summary>
            public string? pattern { set; get; }
        }

    }
}