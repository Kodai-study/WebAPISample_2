using Microsoft.AspNetCore.Mvc;
using System.IO.MemoryMappedFiles;

namespace WebAPISample.Models
{
    public class StationState
    {
        public int numberOfWork_VisualStation { get; set; }
        public int numberOfWork_FunctionalStation { get; set; }
        public int numberOfWork_AsseblyStation { get; set; }

        public StationState()
        {
            try
            {
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