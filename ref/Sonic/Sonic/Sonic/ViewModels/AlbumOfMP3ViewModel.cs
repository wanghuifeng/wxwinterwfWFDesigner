using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.ComponentModel;

//Google .NET API, see GAPI.dll
using System.Timers;
using Gapi.Search;
using System.IO;

namespace Sonic
{




    [DebuggerDisplay("{ToString()}")]
    public class AlbumOfMP3ViewModel : ViewModelBase
    {
        #region Data
        private String album = String.Empty;
        private String artist = String.Empty;
        private List<MP3> files = new List<MP3>();
        private String albumCoverArtUrl = String.Empty;
        private Boolean isAnimatable = false;
        private Double animationDelayMs = 500;
        private Timer delayStartAnimationTimer = new Timer();
        public event EventHandler<EventArgs> AnimationStartTimerExpiredEvent;
        private List<String> allowableLocalImageFormats = new List<String>();

        #endregion

        public AlbumOfMP3ViewModel()
        {
            delayStartAnimationTimer.Enabled = true;
            delayStartAnimationTimer.Elapsed += DelayStartAnimationTimer_Elapsed;

            //add allowable local image formats
            allowableLocalImageFormats.Add("*.jpg");
            allowableLocalImageFormats.Add("*.png");
            allowableLocalImageFormats.Add("*.gif");
        }
        #region Public Methods


        public void OnAnimationStartTimerExpiredEvent()
        {
            // Copy to a temporary variable to be thread-safe.
            EventHandler<EventArgs> temp = AnimationStartTimerExpiredEvent;
            if (temp != null)
                temp(this, new EventArgs());
        }

        public void StartDelayedAnimationTimer()
        {
            delayStartAnimationTimer.Start();      
        }

        /// <summary>
        /// If the AttemptToGainWebAlbumArt setting is on will 
        /// create a google image search for the current Album name
        /// and will attempt to obtain the web page as a string that the
        /// search results url to see if the image is truly available.
        /// 
        /// If it is not available we will get a "404 File Not Found" html
        /// error code. In this case or in the case where we get a WebException,
        /// simply use a defulat application stored image
        /// 
        /// It should be noted that doing a google search takes time, and does
        /// mean there is a lag in getting the search results
        /// </summary>
        /// <returns></returns>
        public Boolean ObtainImageForAlbum()
        {

            Boolean attemptToGainWebAlbumArt = false;
            
            if (Boolean.TryParse(Sonic.Properties.Settings.Default.AttemptToGainWebAlbumArt, 
                    out attemptToGainWebAlbumArt));

            //if the setting is on, we should we use the google .NET api
            //to do a search on google for an image for the album, otherwise 
            //try and find a hard drive stored album image, and if that fails
            //finally use a default image for the album
            if (attemptToGainWebAlbumArt)
            {

                Boolean foundValidImage = false;
                String tempImageUrl = String.Empty;
                WebClient webClient = new WebClient();
                String downloadedContent = String.Empty;

                try
                {
                    SearchResults searchResults =
                        Searcher.Search(SearchType.Image,
                            String.Format("{0}", Album));

                    if (searchResults.Items.Count() > 0)
                    {
                        for (int i = 0; i < 1; i++)
                        {
                            downloadedContent =
                                webClient.DownloadString(searchResults.Items[i].Url);

                            if (!(downloadedContent.Contains("404") ||
                                downloadedContent.ToLower().Contains("file not found")))
                            {
                                tempImageUrl = searchResults.Items[i].Url;
                                foundValidImage = true;
                                break;
                            }
                            else
                            {
                                foundValidImage = false;
                                break;
                            }
                        }
                    }
                }
                catch (WebException)
                {
                    foundValidImage = false;
                }

                if (foundValidImage)
                    albumCoverArtUrl = tempImageUrl;
                else
                    albumCoverArtUrl = "../Images/NoImage.png";
            }
            //not doing web search so look locally for an image                    
            else
            {
                if (!FoundHardDiskImage())
                {
                    albumCoverArtUrl = "../Images/NoImage.png";
                }
            }
            return true;
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Signal that the animation start delay has occurred so tell View to start its 
        /// loading animation via the AnimationStartTimerExpiredEvent
        /// </summary>
        private void DelayStartAnimationTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            delayStartAnimationTimer.Enabled = false;
            delayStartAnimationTimer.Stop();
            OnAnimationStartTimerExpiredEvent();
        }

        /// <summary>
        /// finds a hard disk stoerd album image if one is available
        /// </summary>
        /// <returns></returns>
        private Boolean FoundHardDiskImage()
        {
            try
            {
                FileInfo f = new FileInfo(files[0].FileName);

                foreach (String allowableLocalImageFormat in allowableLocalImageFormats)
                {

                    String[] imageFiles = Directory.GetFiles(f.Directory.FullName, allowableLocalImageFormat);
                    if (imageFiles.Length > 0)
                    {
                        albumCoverArtUrl = imageFiles[0];
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                albumCoverArtUrl = "../Images/NoImage.png";
                return false;
            }
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


        public Boolean IsAnimatable
        {
            get { return isAnimatable; }
            set
            {
                isAnimatable = value;
                NotifyPropertyChanged("IsAnimatable");
            }
        }

        public String Album
        {
            get { return album; }
            set
            {
                album = value;
                NotifyPropertyChanged("Album");
            }
        }

        public String Artist
        {
            get { return artist; }
            set
            {
                artist = value;
                NotifyPropertyChanged("Artist");
            }
        }

        public List<MP3> Files
        {
            get { return files; }
            set
            {
                files = value;
                NotifyPropertyChanged("Files");
            }
        }

        public String AlbumCoverArtUrl
        {
            get { return albumCoverArtUrl; }
            set
            {
                albumCoverArtUrl = value;
                NotifyPropertyChanged("AlbumCoverArtUrl");
            }
        }

        public String ToolTipDisplay
        {
            get { return ToString(); }
        }

        

        #endregion

        #region Overrides
        public override string ToString()
        {
            return String.Format(
                "Album : {0}, Artist : {1}",
                Album, Artist);
        }
        #endregion

    }
}
