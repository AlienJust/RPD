
namespace RPD.Presentation.Utils.Messages
{
    public enum ViewAction
    {
        Show,
        ShowModal,
        Close
    }

    public class ViewMessage
    {
        private readonly ViewAction _action = ViewAction.Show;

        public ViewMessage(ViewAction action)
        {
            _action = action;
        }

        public ViewAction Action
        {
            get { return _action; }
        }
    }
}
