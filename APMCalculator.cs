using Timer = System.Windows.Forms.Timer;

namespace APMCalculator
{
    public class APMCalculator
    {
        private static int INTERVAL_MS = 1000; // set an interval for a timer to run every so often
        private static long ROLLING_AVG_WINDOW_MS = 10000; // 4 GCDs in FFXIV (assuming 2.5s GCD)
        private List<KeyboardEvent> keyboardEvents;
        public int totalKeypresses;
        public long lastProcessedAt;
        public long startedAt;
        private Timer timer;
        public APMCalculator()
        {
            this.keyboardEvents = new List<KeyboardEvent>();
            this.startedAt = this.lastProcessedAt = NowInMs();
            this.totalKeypresses = 0;
            this.timer = new Timer();
            this.timer.Interval = INTERVAL_MS;
            this.timer.Tick += ProcessTimerTick;
        }

        public void StartTimer() { this.timer.Start(); }
        public void StopTimer() { this.timer.Stop(); }
        public void OnTimerTick(EventHandler eventHandler)
        {
            this.timer.Tick += eventHandler;
        }

        // process all keyboard events when the timer ticks
        private void ProcessTimerTick(object? sender, EventArgs e)
        {
            long now = NowInMs();
            int numberOfKeypressesInTick = 0; // use this for something? idk
            this.keyboardEvents.ForEach(ke =>
            {
                numberOfKeypressesInTick += 1;
                this.totalKeypresses += 1;
            });
            // delete all keyboard events, as they have now been processed
            this.keyboardEvents.Clear();
            this.lastProcessedAt = now;
        }

        // Handles when a keyboard button is pressed
        public void Handler(KeyboardEvent evt)
        {
            this.keyboardEvents.Add(evt);
        }

        public double GetApm()
        {
            long msElapsed = GetMsElapsedSinceStart();
            double keyPressesPerMs = (double)totalKeypresses / msElapsed;
            return keyPressesPerMs * 1000 * 60; // convert to keypresses per minute
        }

        public long GetMsElapsedSinceStart()
        {
            return lastProcessedAt - startedAt; 
        }

        private long NowInMs()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }
    }
}