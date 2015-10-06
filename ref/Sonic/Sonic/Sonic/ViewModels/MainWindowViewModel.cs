using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows.Data;
using System.Diagnostics;

namespace Sonic
{

    

    /// <summary>
    /// ViewModel for MainWindow View
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {



        #region Data

        private List<String> genres = null;
        private List<String> artistLetters = null;
        private MediaViewViewModel mediaViewVM = new MediaViewViewModel();

        private Boolean isBusy = false;

        #endregion

        #region Ctor
        public MainWindowViewModel()
        {

        }
        #endregion

        #region Public Methods

        public void Initialise()
        {
            IsBusy = true;

            //Setup Genrees
            if (genres == null)
            {
                genres = new List<string>();

                using (StreamReader sr = new StreamReader("MP3TagGenres.txt"))
                {
                    String[] allGenres = sr.ReadToEnd().Split(
                        new String[] { @"\r", "\n" },
                        StringSplitOptions.RemoveEmptyEntries);

                    foreach (String genreRead in allGenres)
                    {
                        genres.Add(genreRead.Trim());
                    }
                }
                Genres = genres;
            }
           
            //Setup Artist Letters
            if (artistLetters == null)
            {
                artistLetters = new List<string>();
                for (int i = 65; i < 91; i++)
                    artistLetters.Add(((char)i).ToString());

                ArtistLetters = artistLetters;
            }

            if (Globals.ReReadAllFiles)
            {

                try
                {
                    //use a Threadpool thread to run the query
                    ThreadPool.QueueUserWorkItem(x =>
                    {
#if DEBUG
                        Stopwatch stopWatch = new Stopwatch();
                        stopWatch.Start();
                        Console.WriteLine("=========== START OF DB SAVING ===========");
#endif
                        IsBusy = true;
                        mediaViewVM.ReadInitialFolders();
                        XMLAndSQLQueryOperations.CreateDB_MP3Files();
#if DEBUG
                        stopWatch.Stop();
                        TimeSpan ts = stopWatch.Elapsed;
                        String elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                            ts.Hours, ts.Minutes, ts.Seconds,
                            ts.Milliseconds / 10);

                        Console.WriteLine(String.Format(" It took {0} to run the query",
                            elapsedTime));
                        Console.WriteLine("=========== END OF DB SAVING ===========");
#endif
                        IsBusy = false;
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ooops, its busted " + ex.Message);
                }
                finally
                {
                    IsBusy = false;
                }
            }
            else
            {
                IsBusy = false;
            }
        }


        #endregion

        #region Public Properties

        public Boolean IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                NotifyPropertyChanged("IsBusy");
                NotifyPropertyChanged("IsNotBusy");
            }
        }

        public Boolean IsNotBusy
        {
            get { return !isBusy; }
        }


        public List<String> Genres
        {
            get { return genres; }
            set
            {
                genres = value;
                NotifyPropertyChanged("Genres");
            }
        }

 
        public List<String> ArtistLetters
        {
            get { return artistLetters; }
            set
            {
                artistLetters = value;
                NotifyPropertyChanged("ArtistLetters");
            }
        }

        public MediaViewViewModel MediaViewVM
        {
            get { return mediaViewVM; }
            set
            {

                mediaViewVM = value;
                NotifyPropertyChanged("MediaViewVM");
            }
        }
        #endregion
    }
}
