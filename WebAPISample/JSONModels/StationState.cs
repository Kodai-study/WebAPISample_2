using Microsoft.AspNetCore.Mvc;
using System.IO.MemoryMappedFiles;

namespace WebAPISample.JSONModels
{
    /// <summary>
    ///  現在のステーションの情報に関するデータのモデル。
    ///  <see cref="WebAPISample.Controllers.StationStateControll"/>
    ///  取得に失敗したときは、-1が返ってくる
    /// </summary>
    public class StationState
    {
        /// <summary>
        ///  外観検査ステーション内にあるワークの個数
        /// </summary>
        public int numberOfWork_VisualStation { get; set; }

        /// <summary>
        ///  機能検査ステーション内にあるワークの個数
        /// </summary>
        public int numberOfWork_FunctionalStation { get; set; }

        /// <summary>
        ///  組み立てステーション内にあるワークの個数
        /// </summary>
        public int numberOfWork_AsseblyStation { get; set; }

        /// <summary>
        ///  PLC監視プログラムから、ステーションごとのワークの個数を取ってきて
        ///  パラメーターに表示する。
        /// </summary>
        public StationState()
        {
            try
            {
                //PLCの接点監視のプログラムと通信して、ステーションごとのワークの個数を取得する。

                MemoryMappedFile share_mem = MemoryMappedFile.OpenExisting("shared_memory");
                MemoryMappedViewAccessor accessor = share_mem.CreateViewAccessor();

                if (accessor.CanRead)
                {
                    numberOfWork_VisualStation = accessor.ReadInt32(0);
                    numberOfWork_FunctionalStation = accessor.ReadInt32(1);
                    numberOfWork_AsseblyStation = accessor.ReadInt32(2);
                }
                accessor.Dispose();
                share_mem.Dispose();
            }
            catch
            {
                numberOfWork_VisualStation = -1;
                numberOfWork_FunctionalStation = -1;
                numberOfWork_AsseblyStation = -1;
            }
        }

        public StationState(int numberOfWork_VisualStation, int numberOfWork_FunctionalStation, int numberOfWork_AsseblyStation)
        {
            this.numberOfWork_VisualStation = numberOfWork_VisualStation;
            this.numberOfWork_FunctionalStation = numberOfWork_FunctionalStation;
            this.numberOfWork_AsseblyStation = numberOfWork_AsseblyStation;
        }
    }
}