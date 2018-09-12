using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media;
using Newtonsoft.Json;

namespace RPD.Presentation
{
    internal interface IColorsStorage
    {

        Color? GetColor(Guid guid);
        void SetColor(Guid guid, Color color);

        void Save();
    }

    internal class ColorsStorage: IColorsStorage
    {
        private readonly string _fileName;
        private Dictionary<Guid, Color> _colors { get; set; }

        public ColorsStorage(string fileName)
        {
            _fileName = fileName;
            _colors = new Dictionary<Guid, Color>();

            if (File.Exists(_fileName))
                Load();
        }

        private void Load()
        {
            _colors = JsonConvert.DeserializeObject<Dictionary<Guid, Color>>(File.ReadAllText(_fileName));
        }

        public Color? GetColor(Guid guid)
        {
            if (_colors.ContainsKey(guid))
                return _colors[guid];

            return null;
        }

        public void SetColor(Guid guid, Color color)
        {
            _colors[guid] = color;
        }

        public void Save()
        {
            var outputJson = JsonConvert.SerializeObject(_colors, Formatting.Indented);
            File.WriteAllText(_fileName, outputJson);
        }
    }
}
