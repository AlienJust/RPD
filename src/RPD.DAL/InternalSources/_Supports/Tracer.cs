using System;
using System.Collections.Generic;
using System.Text;

namespace RPD.Supports
{
    /// <summary>
    /// Класс трассировки в файл (разработал давно, применяется, когда нужно трассировать события, которые я не в состоянии отследить по каким либо причинам)
    /// </summary>
    public class Tracer
    {
        /// <summary>
        /// Создает новый экземпляр класса
        /// </summary>
        /// <param name="fileName">Имя файла, в который будет производиться трасировка</param>
        /// <param name="append">Флаг необходимости работы с файлом в режиме дозаписи</param>
        /// <param name="logLevel">Уровень детализации трассировки (см. св-во Tracer.MaxLogLevel)</param>
        public Tracer(string fileName, bool append, int logLevel)
        {
            this.fileName = fileName;
            this.append = append;

            this.MaxLogLevel = logLevel;
        }

        /// <summary>
        /// Уровень детализации лога. Если этот параметр меньше предаваемого в вызываемом методе Tracer.Log(), то трассировка не будет производиться.
        /// </summary>
        public int MaxLogLevel { get; set; }

        private System.IO.StreamWriter streamWriter;
        private string fileName;
        private bool append;
        private void WriteMessage(string message)
        {
            try
            {
                streamWriter = new System.IO.StreamWriter(fileName, append);
                streamWriter.WriteLine(message);
                streamWriter.Close();

                if (!append) append = true;
            }
            catch/*(Exception exc)*/
            {
                //do nothing
            }
        }
        private object objSync = new object();

        /// <summary>
        /// Протоколирует текстовое сообщение с DateTimeStamp
        /// </summary>
        /// <param name="text">Текстовое сообщение</param>
        /// <param name="logLevel">Уровень детализации</param>
        public void Log(string text, int logLevel)
        {
            lock (objSync)
            {
                if (this.MaxLogLevel >= logLevel)
                {
                    this.WriteMessage(DateTime.Now.ToString() + " >> " + text);
                }
            }
        }

        /// <summary>
        /// Протоколирует текстовое сообщение от попределенного источника с DateTimeStamp
        /// </summary>
        /// <param name="source">Источник</param>
        /// <param name="text">Сообщение</param>
        /// <param name="logLevel">Уровень детализации</param>
        public void Log(string source, string text, int logLevel)
        {
            lock (objSync)
            {
                if (this.MaxLogLevel >= logLevel)
                {
                    this.WriteMessage(DateTime.Now.ToString() + " >> " + source + " >> " + text);
                }
            }
        }
    }
}
