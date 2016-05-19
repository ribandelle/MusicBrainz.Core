using System;
using System.Threading;
using System.Threading.Tasks;

namespace MusicBrainz.Core
{
    /// <summary>
    /// This class helps running Async methods in a sync environment.
    /// This is necessary because many of the DNX Core classes are only available as async, 
    /// but for backward-compability of this library it's better to keep it sync.
    /// 
    /// Note that this code originally comes from Microsoft: http://stackoverflow.com/a/25097498/61625
    /// </summary>
    internal static class AsyncHelper
    {
        private static readonly TaskFactory _myTaskFactory = new
          TaskFactory(CancellationToken.None,
                      TaskCreationOptions.None,
                      TaskContinuationOptions.None,
                      TaskScheduler.Default);

        public static TResult RunSync<TResult>(Func<Task<TResult>> func)
        {
            return AsyncHelper._myTaskFactory
              .StartNew<Task<TResult>>(func)
              .Unwrap<TResult>()
              .GetAwaiter()
              .GetResult();
        }

        public static void RunSync(Func<Task> func)
        {
            AsyncHelper._myTaskFactory
              .StartNew<Task>(func)
              .Unwrap()
              .GetAwaiter()
              .GetResult();
        }
    }
}
