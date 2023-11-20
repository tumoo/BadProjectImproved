using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadProject.Services.Helper
{
    public static class ErrorQueueHelper
    {
        public static int CountErrorsInLastHour(Queue<DateTime> errors)
        {
            while (errors.Count > 20) errors.Dequeue();
            int errorCount = 0;
            foreach (var dat in errors)
            {
                if (dat > DateTime.Now.AddHours(-1))
                {
                    errorCount++;
                }
            }
            return errorCount;
        }

    }
}
