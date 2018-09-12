using System.Windows.Input;
using Abt.Controls.SciChart.ChartModifiers;

namespace RPD.SciChartControl.ChartModifiers
{
    /// <summary>
    /// Панорамирование при перетаскивании мышью и НЕ зажатом CTRL
    /// </summary>
    internal class ZoomPanModifierEx : ZoomPanModifier
    {
        public override void OnModifierMouseDown(ModifierMouseArgs e)
        {
            if (Keyboard.Modifiers != ModifierKeys.Control)
            {
                base.OnModifierMouseDown(e);
            }
        }
    }
}
