using System.Timers;

namespace TestTutorial.ViewModels
{
    public class TimerViewModel : BaseViewModel
    {
        private float _tipValue;
        public float TipValue
        {
            get { return _tipValue; }
            set { _tipValue = value; RaisePropertyChanged(() => TipValue); }
        }

        System.Timers.Timer timer;

        public TimerViewModel()
        {
            timer = new System.Timers.Timer(500f);

            timer.Elapsed += HandleTimerElapsed;

            timer.Start();

            timer.Stop();
            // SubTotal = 60.0f;
            // TipPercent = 12;
            // Recalculate();

            //_mvxSubscription = Subscribe<TestChangedMessage>(OnTestChangedMessage);

        }

        void HandleTimerElapsed(object sender, ElapsedEventArgs e)
        {
            UnityEngine.Debug.Log("Time Elapsed");
        }




    }

}
