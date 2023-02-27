using WebAPISample.JSONModels;

namespace WebAPISample.Modules
{
    public class UtilizationComparer_DateAsc : IComparer<Utilization>
    {
        public int Compare(Utilization? x, Utilization? y)
        {
            if (x == null || y == null) return 0;
            return x.currentDate.CompareTo(y.currentDate);
        }
    }
    public class UtilizationComparer_DateDesc : IComparer<Utilization>
    {
        public int Compare(Utilization? x, Utilization? y)
        {
            if (x == null || y == null) return 0;
            return x.currentDate.CompareTo(y.currentDate) switch
            {
                0 => 0,
                1 => -1,
                -1 => 1,
                _ => 0,
            };
        }
    }
    
    public class UtilizationComparer_OperationTimeAsc : IComparer<Utilization>
    {
        public int Compare(Utilization? x, Utilization? y)
        {
            if (x == null || y == null) return 0;
            return x.currentDate.CompareTo(y.currentDate);
        }
    }
    public class UtilizationComparer_OperationTimeDesc : IComparer<Utilization>
    {
        public int Compare(Utilization? x, Utilization? y)
        {
            if (x == null || y == null) return 0;
            return x.currentDate.CompareTo(y.currentDate) switch
            {
                0 => 0,
                1 => -1,
                -1 => 1,
                _ => 0,
            };
        }
    }
}