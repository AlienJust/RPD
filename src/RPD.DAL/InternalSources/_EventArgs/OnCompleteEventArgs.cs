using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPD.EventArgs
{
    /// <summary>
    /// Аргументы события завершения опреации
    /// </summary>
    public class OnCompleteEventArgs
    {
        /// <summary>
        /// Возможные результаты операции
        /// </summary>
        public enum CompleteResult
        {
            /// <summary>
            /// Успех
            /// </summary>
            Ok,
            /// <summary>
            /// Ошибка
            /// </summary>
            Error
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса
        /// </summary>
        /// <param name="resultCode">Результат операции</param>
        /// <param name="message">Текстовое сообщение</param>
        /// <param name="argument">Дополнительный аргумент</param>
        public OnCompleteEventArgs(CompleteResult resultCode, string message, object argument)
        {
            this.resultCode = resultCode;
            this.message = message;
            this.argument = argument;
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса
        /// </summary>
        /// <param name="resultCode">Результат операции</param>
        /// <param name="message">Текстовое сообщение</param>
        public OnCompleteEventArgs(CompleteResult resultCode, string message)
        {
            this.resultCode = resultCode;
            this.message = message;
            this.argument = null;
        }

        private CompleteResult resultCode;
        /// <summary>
        /// Результат операции
        /// </summary>
        public CompleteResult ResultCode { get { return resultCode; } }

        private string message;
        /// <summary>
        /// Текстовое сообщение
        /// </summary>
        public string Message { get { return message; } }

        private object argument;
        /// <summary>
        /// Дополнительный аргумент
        /// </summary>
        public object Argument { get { return argument; } }
    }
}
