using System.Windows.Input;
using Abt.Controls.SciChart;
using Abt.Controls.SciChart.ChartModifiers;

namespace RPD.SciChartControl.ChartModifiers
{
    /// <summary>
    /// При зажатом CTRL увеличение по ОСИ, инчае панорамирование
    /// </summary>
    internal class YAxisDragModifierEx : YAxisDragModifier
    {
        public override void OnModifierMouseDown(ModifierMouseArgs e)
        {
            if (e.MouseButtons == MouseButtons.Left && Keyboard.Modifiers == ModifierKeys.Control)
            {
                DragMode = AxisDragModes.Scale;
                base.OnModifierMouseDown(e);
            }
            else
            {
                DragMode = AxisDragModes.Pan;
                base.OnModifierMouseDown(e);
            }
        }
    }
}
