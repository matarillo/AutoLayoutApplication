using System;

namespace AutoLayoutApplication
{
    public class TimerController
    {
        private TimerSwitch timerSwitch;

        public TimerController(TimerSwitch timerSwitch)
        {
            this.timerSwitch = timerSwitch;
        }

        public void ButtonClick(object sender, EventArgs e)
        {
            this.timerSwitch.Enabled = !(this.timerSwitch.Enabled);
        }
    }
}
