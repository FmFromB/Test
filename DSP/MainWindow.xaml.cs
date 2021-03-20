using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using Microsoft.Win32;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveCharts;
using System.Collections;
using System.Globalization;
using System.Text.RegularExpressions;

namespace DSP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int channel_num;
        public int samples_num;
        public long sampling_rate;
        public string start_date;
        public string start_time;
        public string channels_names;
        public string[] date_channel;
        string[] data_all;


        public MainWindow()
        {
            InitializeComponent();
        }

        private void loading_file(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Текстовый файл (*.txt)|*.txt|Звуковой файл (*.wav)|*.wav|Dat-файл (*.dat)|*.dat";
            if (openFileDialog.ShowDialog() == true)
            {

                System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                data_all = File.ReadAllLines(openFileDialog.FileName);

                // Важные переменные
                channel_num = Convert.ToInt32(data_all[1]);
                samples_num = Convert.ToInt32(data_all[3]);
                double k = Double.Parse(data_all[5]);
                sampling_rate = Convert.ToInt32(k);
                start_date = data_all[7];
                start_time = data_all[9];
                channels_names = data_all[11];
                string[] date_channel = new string[samples_num];
                Array.Copy(data_all, 12, date_channel, 0, samples_num);

                ChannelAnalyzer analyzer = new ChannelAnalyzer(channel_num, samples_num, date_channel, channels_names);
                analyzer.Show();
            }

        }
        private void signal_information(object sender, RoutedEventArgs e)
        {
            Signal_information signal_Information = new Signal_information();
            signal_Information.Show();

            signal_Information.chanels_num.Text = channel_num.ToString();
            signal_Information.samples_num.Text = samples_num.ToString();
            signal_Information.sampling_rate.Text = sampling_rate.ToString();
            signal_Information.start.Text = start_date + " " + start_time;
            
            // длительность в секундах
            var ts = TimeSpan.FromSeconds(samples_num);
            
            int hours = ts.Hours;
            int minutes = ts.Minutes % 60;
            int seconds = ts.Seconds % 60;
            signal_Information.duration.Text = ts.Days.ToString() + "суток " +
                                               hours.ToString() + "часов " +
                                               minutes.ToString() + "минут " +
                                               seconds.ToString() + "секунд";

            
            string[] date_start = start_date.Split('-');
            string[] time_end = start_time.Split(':');
            DateTime date_s = new DateTime(Convert.ToInt32(date_start[2]),
                                          Convert.ToInt32(date_start[1]),
                                          Convert.ToInt32(date_start[0]),
                                          Convert.ToInt32(time_end[0]),
                                          Convert.ToInt32(time_end[1]),
                                          (int)Double.Parse(time_end[2]));

            DateTime date_e = date_s + ts;
            signal_Information.end.Text = date_e.ToString();
        }
    }
}
