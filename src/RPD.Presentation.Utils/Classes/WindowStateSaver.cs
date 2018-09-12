using System;
using System.ComponentModel;
using System.Windows;
using System.Xml.Serialization;
using System.IO;

namespace RPD.Presentation.Utils.Classes
{
    /// <summary>
    /// Автоматически сохраняет/загружает состояние окна (размер, положение, состояние).
    /// </summary>
    public class WindowStateSaver: IDisposable
    {
        readonly Window _window;
        SerializableDictionary<string, string> dict = new SerializableDictionary<string, string>();
        readonly string _fileName;


        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="window">Окно, состояние которого нужно сохранять/загружать.</param>
        /// <param name="fileName">Файл состояния.</param>
        public WindowStateSaver(Window window, string fileName)
        {
            _window = window;
            _fileName = fileName;

            _window.Closing += WindowOnClosing;
            _window.Loaded += WindowOnLoaded;  
        }

        private void WindowOnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            Load();
        }

        private void WindowOnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            Save();
        }

        private void Save()
        {
            dict.Clear();

            dict.Add("width", _window.ActualWidth.ToString());
            dict.Add("height", _window.ActualHeight.ToString());
            dict.Add("left", _window.Left.ToString());
            dict.Add("top", _window.Top.ToString());
            dict.Add("state", ((int)_window.WindowState).ToString());

            var serializer = new XmlSerializer(typeof(SerializableDictionary<string, string>));
            var writer = new StreamWriter(_fileName);
            serializer.Serialize(writer, dict);
            writer.Close();
        }

        private void Load()        
        {
            if (!File.Exists(_fileName))
                return;

            var serializer = new XmlSerializer(typeof(SerializableDictionary<string, string>));
            var reader = new StreamReader(_fileName);            

            dict = (SerializableDictionary<string, string>)serializer.Deserialize(reader);

            reader.Close();
            
            if (dict.ContainsKey("width"))
                _window.Width = double.Parse(dict["width"]);

            if (dict.ContainsKey("height"))
                _window.Height = double.Parse(dict["height"]);

            if (dict.ContainsKey("left"))
                _window.Left = double.Parse(dict["left"]);

            if (dict.ContainsKey("top"))
                _window.Top = double.Parse(dict["top"]);

            if (dict.ContainsKey("state"))
                _window.WindowState = (WindowState)int.Parse(dict["state"]);            
        }

        public void Dispose()
        {
            _window.Closing -= WindowOnClosing;
            _window.Loaded -= WindowOnLoaded;
        }
    }
}
