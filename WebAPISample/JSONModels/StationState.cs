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
        public bool isVisualInspectedJustBefore { get; set; }
        public bool isFunctionInspectedJustBefore { get; set; }
        public int resultFrequency { get; set; }
        public int resultVoltage { get; set; }
        public String visualInspectionData { get; set; }
        private readonly String[] ROBOT_STATE_STR = new string[]
        {
            "電源OFF、または通信不可",
            "運転可能状態",
            "個別運転状態",
            "連係動作状態",
            "異常状態"
        };

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
                    numberOfWork_FunctionalStation = accessor.ReadInt32(4);
                    numberOfWork_AssemblyStation = accessor.ReadInt32(8);
                    numberOfOKStock = accessor.ReadInt32(12);
                    numberOfNGStock = accessor.ReadInt32(16);
                    isSystemPause = accessor.ReadBoolean(20);
                    isVisualInspectedJustBefore = accessor.ReadBoolean(21);
                    isFunctionInspectedJustBefore = accessor.ReadBoolean(22);
                    resultFrequency = accessor.ReadInt32(23);
                    resultVoltage = accessor.ReadInt32(27);
                    systemState = ROBOT_STATE_STR[ accessor.ReadInt32(31)];
                    stationState_Supply = ROBOT_STATE_STR[accessor.ReadInt32(35)];
                    stationState_Visual = ROBOT_STATE_STR[accessor.ReadInt32(39)];
                    stationState_Function = ROBOT_STATE_STR[accessor.ReadInt32(43)];
                    stationState_Assembly = ROBOT_STATE_STR[accessor.ReadInt32(47)];
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