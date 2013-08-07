using System;
using System.Threading;

namespace Common.Utils
{
    public class CommonUtils
    {
        public static void Sleep(int waitTimeInSeconds = 1)
        {
            Thread.Sleep(new TimeSpan(0, 0, waitTimeInSeconds));
        }
    }
}