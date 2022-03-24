using System;
using System.Threading.Tasks;

namespace Alteracia.Patterns
{
    public static class AltTasks
    {
        public static async Task WaitFrames(ulong count)
        {
            while (count > 0)
            {
                await Task.Yield();
                count--;
            }
        }

        public static async Task WaitWhile(Func<Boolean> condition)
        {
            while (condition())
            {
                await Task.Yield();
            }
        }
    }
}