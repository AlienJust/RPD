using System.Windows.Input;
using Abt.Controls.SciChart.ChartModifiers;

namespace RPD.SciChartControl.ChartModifiers
{
    /// <summary>
    /// Увеличение при зажатом CTRL
    /// </summary>
    internal class RubberBandXyZoomModifierEx : RubberBandXyZoomModifier
    {
        public override void OnModifierMouseDown(ModifierMouseArgs e)
        {
            if (e.MouseButtons == MouseButtons.Left && Keyboard.Modifiers == ModifierKeys.Control)
            {
                base.OnModifierMouseDown(e);
            }
        }
    }
}
