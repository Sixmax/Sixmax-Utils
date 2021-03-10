using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SXUtils.Helpers
{
    public sealed class Dispatch
    {
        /// <summary/
        /// Dispatch a Code Block in the Main Thread.
        /// </summary>
        /// <param name="action"></param>
        public Dispatch(Action action)
        {
            // This throws an exception when the Application is closed and a Thread is still running.
            if (Application.Current == null)
                return;

            Application.Current.Dispatcher.Invoke(action);
        }
    }
}
