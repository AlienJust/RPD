using System;

namespace RPD.Presentation.Utils.Classes
{
    /// <summary>
    /// Базовый класс для сообщений приложения
    /// </summary>
    public class AppMessagesBase
    {
        public AppMessagesBase()
        {
            Parameter = null;
            Callback = null;
        }

        public AppMessagesBase(object parameter)
        {
            Parameter = parameter;
            Callback = null;
        }

        public AppMessagesBase(object parameter, Func<object> callback)
        {
            Parameter = parameter;
            Callback = callback;
        }

        /// <summary>
        /// Параметр сообщения. Может быть null.
        /// </summary>
        public object Parameter { get; set; }

        /// <summary>
        /// Колбэк, который будет вызван по результату. Может быть null.
        /// </summary>
        public Func<object>  Callback {get; set;}
    }
}
