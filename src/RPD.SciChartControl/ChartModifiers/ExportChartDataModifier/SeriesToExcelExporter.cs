using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows;
using Abt.Controls.SciChart.Model.DataSeries;
using Microsoft.Win32;
using OfficeOpenXml;

namespace RPD.SciChartControl.ChartModifiers.ExportChartDataModifier
{
    internal class SeriesToExcelExporter : ISeriesDataExporter
    {
        private class Entry
        {
            public XyDataSeries<DateTime, double> Series { get; set; }
            public int StartIndex { get; set; }
            public int EndIndex { get; set; }

        }

        IList<Entry> _list = new List<Entry>();

        public void AddSeries(IDataSeries series, int startIndex, int endIndex)
        {
            _list.Add(new Entry()
            {
                Series = (XyDataSeries<DateTime, double>)series, 
                StartIndex = startIndex, 
                EndIndex = endIndex
            });
        }

        public void ExportAll()
        {
            var dialog = new SaveFileDialog();
            dialog.OverwritePrompt = true;
            dialog.DefaultExt = ".xlsx";
            dialog.Filter = "Файлы Excel |*.xlsx";

            var showDialog = dialog.ShowDialog();
            if (showDialog != null && showDialog.Value == false)
                return;

            if (File.Exists(dialog.FileName) && dialog.OverwritePrompt)
                File.Delete(dialog.FileName);

            using (var excel = new ExcelPackage(new FileInfo(dialog.FileName)))
            {
                var ws = excel.Workbook.Worksheets.Add("Данные");

                int row = 1;
                foreach (var entry in _list)
                {
                    InsertHeader(ws, entry.Series.SeriesName, row);
                    row++;

                    ws.Column(1).BestFit = true;
                    ws.Column(1).AutoFit(25);
                    ws.Column(1).Style.Indent = 3;

                    ws.Column(2).BestFit = true;
                    ws.Column(2).AutoFit(20);
                    ws.Column(2).Style.Indent = 3;

                    for (int i = entry.StartIndex; i <= entry.EndIndex; i++)
                    {
                        ws.Cells["A" + row].Value = entry.Series.XValues[i].ToString("dd.MM.yyyy HH:mm:ss.fff");
                        ws.Cells["B" + row].Value = entry.Series.YValues[i].ToString("");
                        row++;
                    }
                }

                excel.Save();
            }

            _list.Clear();

            MessageBox.Show("Данные сохранены в файл " + dialog.FileName);
        }

        private static void InsertHeader(ExcelWorksheet ws, string seriesName, int row)
        {
            ws.Cells["A" + row].Value = seriesName;
            using (ExcelRange r = ws.Cells["A" + row + ":N" + row])
            {
                r.Merge = true;
                r.Style.Font.Color.SetColor(Color.White);
                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
            }
        }
    }
}