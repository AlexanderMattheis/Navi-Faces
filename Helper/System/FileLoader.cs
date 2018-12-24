using System;
using System.IO;

namespace Navi.Helper.System
{
    /// <summary>
    /// Loads files which can be read with the DataReader in this class.
    /// </summary>
    public sealed class FileLoader : IDisposable
    {
        private StreamReader reader;
        private FileStream stream;

        public StreamReader DataReader
        {
            get { return reader; }
        }

        #region [loader]
        public bool Load(string filePath)
        {
            bool success = true;

            try
            {
                stream = new FileStream(filePath, FileMode.Open);
                reader = new StreamReader(stream);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
                success = false;
            }

            return success;
        }
        #endregion

        #region [leaver]
        public void Close()
        {
            if (reader != null)  // (reader != null) only if Load(..)-method was successful
            {
                reader.Close();
                stream.Close();
            }
        }

        /// <summary>
        /// You have to implemant this method to avoids warnings.
        /// </summary>
        public void Dispose()
        {
            if (reader != null)  // (reader != null) only if Load(..)-method was successful
            {
                reader.Dispose();
                stream.Dispose();
            }
        }
        #endregion
    }
}
