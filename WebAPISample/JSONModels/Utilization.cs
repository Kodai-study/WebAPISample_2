using Microsoft.AspNetCore.Mvc;
using System.IO.MemoryMappedFiles;
using WebAPISample.Controllers;
using WebAPISample.JSONModels;

namespace WebAPISample.JSONModels
{
    /// <summary>
    ///  日ごとのロボットの稼働時間、停止時間、稼働率などの
    ///  稼働データを表すモデルクラス
    /// </summary>
    public class Utilization
    {
        /// <summary>
        ///  対象となる検査日付の値
        /// </summary>
        public DateTime currentDate { get; set; } = DateTime.MinValue;

        /// <summary>
        ///  ロボットが稼働していた合計時間
        /// </summary>
        public TimeSpan timeOfOperation { get; set; } = TimeSpan.Zero;

        /// <summary>
        ///  ロボットが停止(ストッカオーバー等による一時停止)
        ///  していた時間の合計
        /// </summary>
        public TimeSpan timeOfStopSum { get; set; } = TimeSpan.Zero;

        /// <summary>
        ///  供給ロボットが供給動作を停止していた時間の合計
        /// </summary>
        public TimeSpan timeOfSupplyPause { get; set; } = TimeSpan.Zero;

        /// <summary>
        ///  組み立てステーション等の要因でシステムが停止状態になっていた
        ///  時間の合計
        /// </summary>
        public TimeSpan timeOfPause { get; set; } = TimeSpan.Zero;

        public Utilization() { }

        /// <summary>
        ///  1日毎の、システムの状態変化の記録から稼働時間の情報を作成する
        ///  </summary>
        /// <param name="currentDate">
        ///  対象の日にち。時刻が00:00のDateTimeのデータを渡す
        /// </param>
        /// <param name="stateChangeDatas">
        ///  状態変化の履歴のリストデータ。
        ///  <see cref="UtilizationControll.STATECODE_EMERGENCY"/>
        /// </param>
        public Utilization(DateTime currentDate, List<Tuple<String, DateTime>> stateChangeDatas)
        {
            DateTime startTime = DateTime.MinValue; //稼働が開始した時刻
            DateTime pauseTime = DateTime.MinValue; //一時停止した時刻
            String lastStopCause = "";              //最後に停止した時に要因を保存して、再開したときの処理を変える
            this.currentDate = currentDate;

            foreach (var changeData in stateChangeDatas)
            {
                /* 稼働開始したときの処理 */
                if (changeData.Item1.Equals(UtilizationControll.STATECODE_START))
                {
                    startTime = changeData.Item2;
                }
                /* 稼働終了したときの処理 */
                else if (changeData.Item1.Equals(UtilizationControll.STATECODE_END))
                {
                    if (startTime.Equals(DateTime.MinValue))
                    {
                        Console.WriteLine("ロボットのストップ前にスタートがかかった");
                        return;
                    }
                    timeOfOperation += (changeData.Item2 - startTime);
                    startTime = DateTime.MinValue;
                }
                /* 供給停止したときの処理 */
                else if (changeData.Item1.Equals(UtilizationControll.STATECODE_PAUSE))
                {
                    lastStopCause = UtilizationControll.STATECODE_PAUSE;
                    pauseTime = changeData.Item2;
                }
                /* 一時停止状態に移行したときの処理 */
                else if (changeData.Item1.Equals(UtilizationControll.STATECODE_STANDBY))
                {
                    lastStopCause = UtilizationControll.STATECODE_STANDBY;
                    pauseTime = changeData.Item2;
                }
                /* 一時停止状態が解除し、再開したときの処理 */
                else if (changeData.Item1.Equals(UtilizationControll.STATECODE_RESTART))
                {
                    /* 供給停止、システム停止によって停止時間の種類を分ける */
                    if (lastStopCause.Equals(UtilizationControll.STATECODE_PAUSE))
                    {
                        timeOfSupplyPause += (changeData.Item2 - pauseTime);
                    }
                    else if (lastStopCause.Equals(UtilizationControll.STATECODE_STANDBY))
                    {
                        timeOfPause += (changeData.Item2 - pauseTime);
                    }
                    else
                    {
                        Console.WriteLine("ロボットのストップ前にリスタートがかかった");
                    }
                    pauseTime = DateTime.MinValue;
                    lastStopCause = "";
                }
                /* 非常停止した時の処理 */
                else if (changeData.Item1.Equals(UtilizationControll.STATECODE_EMERGENCY))
                {
                    if (startTime != DateTime.MinValue)
                    {
                        timeOfOperation += (changeData.Item2 - startTime);
                        startTime = DateTime.MinValue;
                    }

                    if (pauseTime != DateTime.MinValue)
                    {
                        timeOfOperation += (changeData.Item2 - pauseTime);
                        pauseTime = DateTime.MinValue;
                    }
                }
            }

            if (currentDate != DateTime.Now.Date)
                return;
            // 今日(APIを呼び出して日付)のデータで、稼働開始後で停止していない時は、
            // まだ稼働中と考える。
            if (startTime != DateTime.MinValue)
            {
                timeOfOperation += (startTime - DateTime.Now);
            }

            //一時停止後再開していない時は、まだ停止中だと考える
            if (pauseTime != DateTime.MinValue)
            {
                if (lastStopCause.Equals(UtilizationControll.STATECODE_PAUSE))
                {
                    timeOfSupplyPause += (DateTime.Now - pauseTime);
                }
                else if (lastStopCause.Equals(UtilizationControll.STATECODE_STANDBY))
                {
                    timeOfPause += (DateTime.Now - pauseTime);
                }
            }
        }
    }
}