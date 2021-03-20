using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Wpf;

namespace DSP
{
    /// <summary>
    /// Логика взаимодействия для ChannelAnalyzer.xaml
    /// </summary>
    public partial class ChannelAnalyzer : Window
    {
        public ChannelAnalyzer(int channel_num, int samples_num, string[] date_channel, string channels_names)
        {
            InitializeComponent();

            // j - канал
            string[] date_channels = new string[channel_num + 1];
            for (int i = 0; i < samples_num; i++)
            {
                string[] tmp = date_channel[i].Split(' ');
                for (int j = 0; j < channel_num; j++)
                {
                    date_channels[j] += tmp[j] + ',';
                }
            }

            string[] names = channels_names.Split(';');

            for (int i = 0; i < channel_num; i++) {
                string tm = date_channels[i];
                string[] results = tm.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                List<double> channel_tmp = new List<double>();
                channel_tmp.AddRange(results.Select(x => Convert.ToDouble(x)));
                TextBlock name = new TextBlock();
                name.Text = names[i];
                Grid.SetRow(name, i);
                TestGrid.Children.Add(name);
                CartesianChart ch = new CartesianChart();
                ch.Series = new SeriesCollection
                {
                    new LineSeries
                    {
                        LineSmoothness = 1,
                        StrokeThickness = 2,
                        DataLabels = false,
                        Fill = Brushes.Transparent,
                        PointGeometrySize = 0,
                        Title = names[i],
                        Values = new ChartValues<double>(channel_tmp),
                    }
                };
                //ch.AxisX.MajorGrid.Enabled = false;
                //ch.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
                RowDefinition rowDef = new RowDefinition();
                TestGrid.RowDefinitions.Add(rowDef);
                Grid.SetRow(ch, i);
                TestGrid.Children.Add(ch);
            }
        }
    }
}
