using Abt.Controls.SciChart.Numerics;

namespace RPD.SciChartControl.ViewModel
{
    /// <summary>
    /// Настройки чарта.
    /// </summary>
    internal interface IChartSettings {
        bool IsToZoomXAxisOnly { get; set; }

        /// <summary>
        /// Если true, то при включенном IsToZoomXAxisOnly при зуме высота графика по оси У будет растягиваться по максимуму
        /// </summary>
        bool ZoomExtentsY { get; set; }
        bool IsAnnotationsVisible { get; set; }
        ResamplingMode SelectedResamplingMode { get; set; }
        int StrokeThickness { get; set; }
        string SelectedTheme { get; set; }
        bool IsAntialiasingEnabled { get; set; }
        bool IsMarkersVisible { get; set; }

        void SaveSettings(string fileName);
        void LoadSetings(string fileName);
    }
}