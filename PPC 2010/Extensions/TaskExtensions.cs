using System;
using System.Threading.Tasks;

namespace PPC_2010.Extensions
{
    public static class TaskExtensions
    {
        public static void RunInBackground(Func<Task> action)
        {
            Task.Run(async () =>
            {
                try
                {
                    await action();
                }
                catch (Exception ex)
                {
                    Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                }
            });
        }
    }
}