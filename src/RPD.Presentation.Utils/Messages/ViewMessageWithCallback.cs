using System;

namespace RPD.Presentation.Utils.Messages
{
    public class ViewMessageWithCallback<T> : ViewMessage
    {
        private readonly Action<T> _callback;

        public ViewMessageWithCallback(ViewAction action)
            : base(action)
        {
        }

        public ViewMessageWithCallback(ViewAction action, Action<T> callback)
            : base(action)
        {
            _callback = callback;
        }

        public Action<T> Callback {
            get { return _callback; } 
        }
    }
}