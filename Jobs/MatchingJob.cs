using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haketon.Jobs
{
    public class MatchingJob: IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine("executing...");
        }
    }
}
