using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SXUtils.Abstracts
{
    public abstract class BaseFile
    {
        private readonly object lock1 = new object();
        private readonly object lock2 = new object();
        private readonly object lock3 = new object();

        public string FilePath { get; set; } = null;
        protected void CheckFile()
        {
            if (File.Exists(FilePath) == false)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(FilePath));
                File.Create(FilePath).Close();
            }
        }

        /// <summary>
        /// Check if the File at the Path exists.
        /// </summary>
        /// <returns></returns>
        public bool FileExists() => FilePath == null ? false : File.Exists(FilePath);

        public BaseFile(string FilePath = null)
        {
            if (FilePath == null)
                return;

            this.FilePath = FilePath;
        }

        /// <summary>
        /// Get all the File Contents.
        /// </summary>
        /// <returns></returns>
        public string ReadFile()
        {
            if (FileExists() == false)
                throw new FileNotFoundException("The File does not exist or the Path has not been defined.");

            lock (lock1)
                return File.ReadAllText(FilePath);
        }

        /// <summary>
        /// Change the entire File Content..
        /// </summary>
        /// <param name="NewContents"></param>
        public void OverrideFile(string NewContents)
        {
            if (string.IsNullOrEmpty(NewContents))
                throw new ArgumentNullException("The new Filecontents cannot be empty or null!");

            CheckFile();

            if (FileExists() == false)
                throw new FileNotFoundException("The File does not exist or the Path has not been defined.");

            lock (lock2)
                File.WriteAllText(FilePath, NewContents);
        }

        /// <summary>
        /// Append a String to the Log.
        /// </summary>
        /// <param name="Data"></param>
        public void AppendString(string Data)
        {
            if (string.IsNullOrEmpty(Data))
                throw new ArgumentNullException("The new Filecontents cannot be empty or null!");

            CheckFile();

            if (FileExists() == false)
                throw new FileNotFoundException("The File does not exist or the Path has not been defined.");

            lock (lock3)
                File.AppendAllText(FilePath, Data);
        }
    }
}
