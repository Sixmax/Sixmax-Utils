using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SXUtils.Abstracts
{
    abstract class BaseThread
    {
        /// <summary>
        /// Is the Thread current Active?
        /// </summary>
        public bool Running
        {
            get
            {
                if (worker == null)
                    return false;

                if (worker.ThreadState == ThreadState.Aborted
                       || worker.ThreadState == ThreadState.Unstarted
                       || worker.ThreadState == ThreadState.Stopped
                       || worker.ThreadState == ThreadState.WaitSleepJoin
                       || worker.ThreadState == ThreadState.Suspended)
                    return false;

                return true;
            }
        }

        public Thread Worker { get => worker; }
        private Thread worker = null;

        private Action PlaceholderAction;

        public BaseThread(Action worker = null)
        {
            if (worker == null)
                return;

            PlaceholderAction = worker;
        }

        ~BaseThread()
        {
            if (Running)
                Halt();
        }

        /// <summary>
        /// Start the Thread.
        /// </summary>
        protected void Execute()
        {
            if (Running)
                return;

            worker = new Thread(Task)
            {
                IsBackground = true
            };

            worker.Start();
        }

        /// <summary>
        /// Stop the Thread.
        /// </summary>
        protected void Halt()
        {
            if (worker == null)
                return;

            if (Running == false)
                return;

            try
            {
                try
                {
#pragma warning disable SYSLIB0006 
                    worker.Abort();
#pragma warning restore SYSLIB0006
                }
                catch
                {
                    worker.Join();
                }
            }
            catch { }

            worker = null;
        }

        protected virtual void Task() => PlaceholderAction?.Invoke();

        /// <summary>
        /// Start the Thread.
        /// </summary>
        public virtual void Start() => Execute();

        /// <summary>
        /// Stop the Thread.
        /// </summary>
        public virtual void Stop() => Halt();
    }
}
