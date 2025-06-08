using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Awaker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // Default
            textMinute.Text = (180).ToString();
            this.FormClosing += (obj, e) => UseNormalMode();

            UseAwakeMode();
            Countdown();

            // 設定視窗顯示於螢幕右下角
            var screen = Screen.PrimaryScreen.WorkingArea;
            this.Location = new Point(
                screen.Right - this.Width,
                screen.Bottom - this.Height
            );
        }

        public void UseNormalMode()
        {
            SystemSleep.RestoreForCurrentThread();
        }

        public void UseAwakeMode()
        {
            SystemSleep.PreventForCurrentThread();
        }

        private void Countdown()
        {
            var timer = new System.Timers.Timer(60000);
            timer.Elapsed += (sender, e) =>
            {
                UseNormalMode();

                try
                {
                    decimal minuteValue;
                    if (!decimal.TryParse(textMinute.Text, out minuteValue))
                    {
                        minuteValue = 180;
                    }
                    updateTextMinute((minuteValue - 1).ToString());

                    if (minuteValue - 1 <= decimal.Zero)
                    {
                        if (sender != null)
                        {
                            ((System.Timers.Timer)sender).Stop();
                        }
                        Environment.Exit(0);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            };

            timer.Start();
        }
        private void updateTextMinute(string text)
        {
            if (textMinute.InvokeRequired)
            {
                textMinute.Invoke(new Action<string>(updateTextMinute), text);
            }
            else
            {
                textMinute.Text = text;
            }
        }
    }
}