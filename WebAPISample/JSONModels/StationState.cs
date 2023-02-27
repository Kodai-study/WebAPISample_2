using Microsoft.AspNetCore.Mvc;
using System;
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
        public bool isSuccessConnect { get; set; }
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
        public int numberOfWork_AssemblyStation { get; set; }

        public int numberOfNGStock { get; set; }
        public int numberOfOKStock { get; set; }
        public String systemState { get; set; }
        public String stationState_Supply { get; set; }
        public String stationState_Visual { get; set; }
        public String stationState_Function { get; set; }
        public String stationState_Assembly { get; set; }
        public bool isSystemPause { get; set; }
        public String systemPauseCause { get; set; }
        public bool isInspectedJustBefore { get; set; }
        public int resultFrequency { get; set; }
        public float resultVoltage { get; set; }
        public String visualInspectionData { get; set; }

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
                isSuccessConnect = accessor.CanRead;
                if (accessor.CanRead)
                {
                    numberOfWork_VisualStation = accessor.ReadInt32(0);
                    numberOfWork_FunctionalStation = accessor.ReadInt32(1);
                    numberOfWork_AssemblyStation = accessor.ReadInt32(2);
                    numberOfOKStock = accessor.ReadInt32(3);
                    numberOfNGStock = accessor.ReadInt32(4);


                    isSystemPause = accessor.ReadBoolean(10);
                    isInspectedJustBefore = accessor.ReadBoolean(12);
                    resultFrequency = accessor.ReadInt32(13);
                    resultVoltage = (float)accessor.ReadDouble(14);
                    // systemState = accessor.ReadInt32(5);
                    // stationState_Supply = accessor.ReadInt32(6);
                    //stationState_Visual = accessor.ReadInt32(7);
                    //stationState_Function = accessor.ReadInt32(8);
                    //stationState_Assembly = accessor.ReadInt32(9);

                    //systemPauseCause = accessor.ReadInt32(11);
                    //visualInspectionData = accessor.ReadInt32(15);

                }
                accessor.Dispose();
                share_mem.Dispose();
            }
            catch
            {
                numberOfWork_VisualStation = -1;
                numberOfWork_FunctionalStation = -1;
                numberOfWork_AssemblyStation = -1;
            }
        }

        public StationState(int numberOfWork_VisualStation, int numberOfWork_FunctionalStation, int numberOfWork_AsseblyStation)
        {
            this.numberOfWork_VisualStation = numberOfWork_VisualStation;
            this.numberOfWork_FunctionalStation = numberOfWork_FunctionalStation;
            this.numberOfWork_AssemblyStation = numberOfWork_AsseblyStation;
        }
    }
}