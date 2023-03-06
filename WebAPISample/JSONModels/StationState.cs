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
        public String resultVisualInspection { get; set; }

        private readonly String[] ROBOT_STATE_STR = new string[]
        {
            "電源OFF、または通信不可",
            "運転可能状態",
            "個別運転状態",
            "連係動作状態",
            "異常状態"
        };

        private readonly String[] SYSTEM_STETE_STR = new string[]
        {
            "連係動作中",
            "動作開始待機中",
            "全ステーション通信可能",
            "異常状態",
            "供給停止状態",
            "個別運転状態"
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
                int headPosition = 0;
                if (accessor.CanRead)
                {
                    numberOfWork_VisualStation = accessor.ReadInt32(headPosition);
                    numberOfWork_FunctionalStation = accessor.ReadInt32(headPosition += 4);
                    numberOfWork_AssemblyStation = accessor.ReadInt32(headPosition += 4);
                    numberOfOKStock = accessor.ReadInt32(headPosition += 4);
                    numberOfNGStock = accessor.ReadInt32(headPosition += 4);
                    isSystemPause = accessor.ReadBoolean(headPosition += 4);
                    isVisualInspectedJustBefore = accessor.ReadBoolean(headPosition += 1);
                    isFunctionInspectedJustBefore = accessor.ReadBoolean(headPosition += 1);
                    resultVisualInspection = accessor.ReadBoolean(headPosition += 1) ? "合格" : "不合格";
                    resultFrequency = accessor.ReadInt32(headPosition += 1);
                    resultVoltage = accessor.ReadInt32(headPosition += 4);
                    systemState = SYSTEM_STETE_STR[accessor.ReadInt32(headPosition += 4)];
                    stationState_Supply = ROBOT_STATE_STR[accessor.ReadInt32(headPosition += 4)];
                    stationState_Visual = ROBOT_STATE_STR[accessor.ReadInt32(headPosition += 4)];
                    stationState_Function = ROBOT_STATE_STR[accessor.ReadInt32(headPosition += 4)];
                    stationState_Assembly = ROBOT_STATE_STR[accessor.ReadInt32(headPosition += 4)];
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
    }
}