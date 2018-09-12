using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPD.Presentation.Utils.Classes;

namespace RPD.Configurator.Messages
{
    class AppMessages: AppMessagesBase
    {
        public AppMessages(): base()
        {
          
        }

        public AppMessages(object parameter): base(parameter)
        {
            
        }

        public AppMessages(object parameter, Func<object> callback): base(parameter, callback)
        {

        }

        static public string CloseSelectMeterTypeWindow = "CloseSelectMeterTypeWindow";

        static public string CloseEditRpdChannelWindow = "EditRpdChannelWindow";

        static public string ShowSaveToFileDialog = "ShowSaveToFileDialog";

        static public string ShowLoadFromFileDialog = "ShowLoadFromFileDialog";

        static public string ShowDialog = "ShowDialog";

        static public string CloseDefaultConnectionPointsWindow = "CloseDefaultConnectionPointsWindow";
    }
}
