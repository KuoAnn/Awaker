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
            textMinute.Text = (360).ToString();
            this.FormClosing += (obj, e) => UseNormalMode();

            UseAwakeMode();
            Countdown();
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
                    updateTextMinute((Convert.ToDecimal(textMinute.Text) - 1).ToString());

                    if (Convert.ToDecimal(textMinute.Text) <= decimal.Zero)
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