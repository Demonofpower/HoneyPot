using System.Threading;

namespace HoneyPot.Scene.Steps
{
    class IdleStep : IStep
    {
        public event StepFinishedEventHandler StepFinished;

        public int IdleTimeInMs { get; }

        public IdleStep(int idleTimeInMs)
        {
            IdleTimeInMs = idleTimeInMs;
        }
        
        public void InvokeStep()
        {
            new Thread(Idle).Start();
        }

        private void Idle()
        {
            Thread.Sleep(IdleTimeInMs);
            StepFinished?.Invoke();
        }
    }
}
