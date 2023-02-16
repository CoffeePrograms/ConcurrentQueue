using ConcurrentQueue.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrentQueue.Implementation
{
    internal class CallsLogger : INotifyPropertyChanged, ILogger
    {

        private string logMessage;

        public StringBuilder Text { get; set; }
        public string LogMessage
        {
            get => logMessage;
            set
            {
                logMessage = value;
                NotifyPropertyChanged();
            }
        }

        public CallsLogger()
        {
            Text = new StringBuilder();
        }

        public void Log(string message)
        {
            Text.AppendLine(message);
            LogMessage = Text.ToString();
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
