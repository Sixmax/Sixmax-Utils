using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SXUtils.Helpers
{
    public static class Helper
    {
        /// <summary>
        /// Get the Resource Dictionary from an External Assembly.
        /// </summary>
        /// <param name="AssemblyName"></param>
        /// <param name="Path"></param>
        /// <returns></returns>
        public static ResourceDictionary GetExternalResources(string AssemblyName, string Path)
        {
            if (string.IsNullOrEmpty(AssemblyName) || string.IsNullOrEmpty(Path))
                return null;

            ResourceDictionary r = new ResourceDictionary()
            {
                Source = new Uri("pack://application:,,,/" + AssemblyName + ";component" + Path)
            };

            return r;
        }
    }
}
