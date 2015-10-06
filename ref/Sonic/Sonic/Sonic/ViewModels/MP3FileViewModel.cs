using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.ComponentModel;
using System.Timers;

namespace Sonic
{

    [DebuggerDisplay("{ToString()}")]
    public class MP3FileViewModel : ViewModelBase
    {
        #region Data
        private String title = String.Empty;
        private String fileName = String.Empty;
        private Boolean isSelected = false;
        private Double animationDelayMs = 500;
        private Timer delayStartAnimationTimer = new Timer();
        public event EventHandler<EventArgs> AnimationStartTimerExpiredEvent;

        #endregion

        #region Ctor
        public MP3FileViewModel()
        {
            delayStartAnimationTimer.Enabled = true;
            delayStartAnimationTimer.Elapsed += DelayStartAnimationTimer_Elapsed;
        }
        #endregion

        #region Private Methods
        private void DelayStartAnimationTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            delayStartAnimationTimer.Enabled = false;
            delayStartAnimationTimer.Stop();
            OnAnimationStartTimerExpiredEvent();

        }
        #endregion

        #region Events
        public void OnAnimationStartTimerExpiredEvent()
        {
            // Copy to a temporary variable to be thread-safe.
            EventHandler<EventArgs> temp = AnimationStartTimerExpiredEvent;
            if (temp != null)
                temp(this, new EventArgs());
        }
        #endregion

        #region Public Methods

        public void StartDelayedAnimationTimer()
        {
            delayStartAnimationTimer.Start();      
        }

        #endregion

        #region Public Properties

        public Double AnimationDelayMs
        {
            private get { return animationDelayMs; }
            set
            {
                animationDelayMs = value;
                delayStartAnimationTimer.Interval = animationDelayMs;
            }
        }


        public Boolean IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                NotifyPropertyChanged("IsSelected");
            }
        }

        public String FileName
        {
            get { return fileName; }
            set
            {
                fileName = value;
                NotifyPropertyChanged("FileName");
            }
        }

        public String Title
        {
            get { return title; }
            set
            {
                title = value;
                NotifyPropertyChanged("Title");
            }
        }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return FileName;
        }
        #endregion

    }
}
