using ConcurrentQueue.Implementation;
using ConcurrentQueue.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrentQueue
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            Logger = new CallsLogger();
            Agents = new ObservableCollection<Agent>();
            Calls = new ConcurrentQueue<Call>();

            Agents.Add(new Agent("Антон"));
            Agents.Add(new Agent("Иван"));
            Agents.Add(new Agent("Анна"));
            Agents.Add(new Agent("Ольга"));

            AddCallCommand = new DelegateCommand()
            {
                Method = AddCall,
                CanExecuteMethod = param => true
            };

            AddAgentCommand = new DelegateCommand()
            {
                Method = AddAgent,
                CanExecuteMethod = param => !string.IsNullOrEmpty(NameOfNewAgent)
            };            
            
            RemoveAgentCommand = new DelegateCommand()
            {
                Method = RemoveAgent,
                CanExecuteMethod = param => selectedAgent != null
            };

            ThreadPool.QueueUserWorkItem(AddCallEndlessly);
            ThreadPool.QueueUserWorkItem(HandleCallEndlessly);
        }



        #region Agents
        public ObservableCollection<Agent> Agents { get; set; }

        private Agent selectedAgent;
        public Agent SelectedAgent
        {
            get => selectedAgent;
            set
            {
                selectedAgent = value;
                NotifyPropertyChanged();
            }
        }

        private string nameOfNewOperator;
        public string NameOfNewAgent
        {
            get => nameOfNewOperator;
            set
            {
                nameOfNewOperator = value;
                NotifyPropertyChanged();
            }
        }

        public DelegateCommand AddCallCommand { get; set; }
        public DelegateCommand AddAgentCommand { get; set; }
        public DelegateCommand RemoveAgentCommand { get; set; }

        public void AddAgent(object parametr)
        {
            Agent agent = new Agent(NameOfNewAgent);
            Agents.Add(agent);
            Logger.Log($"Оператор {NameOfNewAgent} добавлен.");
            NameOfNewAgent = string.Empty;
        }

        public void RemoveAgent(object parametr)
        {
            var agentName = SelectedAgent.Name;
            Agents.Remove(SelectedAgent);
            Logger.Log($"Оператор {agentName} удален.");
        }

        #endregion



        #region Calls
        public ConcurrentQueue<Call> Calls { get; set; }
        public CallsLogger Logger { get; set; }
        public void AddCall(object parametr)
        {
            Call call = new Call();
            Calls.Enqueue(call);
            Logger.Log($"Новое соединение {call.Id} добавлено в очередь.");
            Logger.Log($"Количество соединений в очереди: {Calls.Count()}.");
        }

        public void AddCallEndlessly(object parametr)
        {
            while (true)
            {
                AddCall(parametr);

                Thread.Sleep(new Random().Next(3, 5) * 1000);
            }
        }

        public void HandleCall(object parametr)
        {
            var freeAgents = Agents.Where(l => l.IsFree).ToList();

            if (freeAgents.Count != 0 && Calls.TryDequeue(out Call call))
            {
                Agent agent;
                lock (freeAgents)
                {
                    int index = new Random().Next(freeAgents.Count);
                    agent = freeAgents[index];
                }
                freeAgents = null;

                lock (agent)
                {
                    Logger.Log($"Оператор {agent.Name} начал разговор #{call.Id}.");
                    agent.IsFree = false;
                    Thread.Sleep(call.DurationSec * 1000);
                    agent.IsFree = true;
                    Logger.Log($"Оператор {agent.Name} закончил разговор длинной {call.DurationSec}с.");
                    Logger.Log($"Количество соединений в очереди: {Calls.Count()}.");
                }
            }
        }

        public void HandleCallEndlessly(object parametr)
        {
            while (true)
            {
                ThreadPool.QueueUserWorkItem(HandleCall);

                Thread.Sleep(new Random().Next(1, 3) * 1000);
            }
        }
        #endregion



        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
