using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml.Linq;
using System.Windows.Input;
using System.Linq.Expressions;
using System.Windows.Threading;
using System.Threading;
using System.Diagnostics;

namespace Sonic
{

    public enum QueryTypes : int
    {
        [LocalizableDescription(@"ByArtistName", typeof(Resources))]
        ByArtistName = 1,

        [LocalizableDescription(@"BySongName", typeof(Resources))]
        BySongName = 2,
    }


    public class MediaViewViewModel : ViewModelBase
    {
        #region Data
        private Boolean isBusy = false;

        
        //Query option related items
        private String currentArtistLetter = String.Empty;
        private QueryTypes currentQueryType = QueryTypes.ByArtistName;
        private String currentGenre = String.Empty;
        private String currentKeyWord = String.Empty;

        //Query related items
        private enum OverallQueryType { ByArtistLetter, BySearchType, ByGenre, ByKeyWord };
        private OverallQueryType QueryToPerform = OverallQueryType.ByKeyWord;
        private Expression<Func<MP3, Boolean>> queryExpression = null;
        private ObservableCollection<AlbumOfMP3ViewModel> albumsReturned = 
            new ObservableCollection<AlbumOfMP3ViewModel>();
        

        //Commands
        private ICommand runQueryCommand = null;

        #endregion

        #region Ctor
        public MediaViewViewModel()
        {


            //wire up command
            runQueryCommand = new SimpleCommand
            {
                CanExecuteDelegate = x => !IsBusy && queryExpression != null,
                ExecuteDelegate = x => RunQuery(queryExpression)
            };


        }
        #endregion

        #region Public Properties

        public ICommand RunQueryCommand
        {
            get { return runQueryCommand; }
        }

        public String CurrentArtistLetter
        {
            get { return currentArtistLetter; }
            set
            {
                currentArtistLetter = value;
                NotifyPropertyChanged("CurrentArtistLetter");

                //create the correct Query type and Expression for the query
                QueryToPerform = OverallQueryType.ByArtistLetter;
                queryExpression = 
                    mp3 => mp3.Artist.ToLower().
                        StartsWith(currentArtistLetter.ToLower());
            }
        }


        public QueryTypes CurrentQueryType
        {
            get { return currentQueryType; }
            set
            {
                currentQueryType = value;
                QueryToPerform = OverallQueryType.BySearchType;
                NotifyPropertyChanged("CurrentQueryType");
                CreateCombinedQuery();
            }
        }

        public String CurrentGenre
        {
            get { return currentGenre; }
            set
            {
                currentGenre = value;
                QueryToPerform = OverallQueryType.ByGenre;
                NotifyPropertyChanged("CurrentGenre");

                //create the correct Query type and Expression for the query
                QueryToPerform = OverallQueryType.ByGenre;
                queryExpression =
                    mp3 => mp3.GenreName.ToLower().
                        Equals(currentGenre.ToLower());

            }
        }

        public String CurrentKeyWord
        {
            get { return currentKeyWord; }
            set
            {
                currentKeyWord = value;
                QueryToPerform = OverallQueryType.ByKeyWord;
                NotifyPropertyChanged("CurrentKeyWord");
                CreateCombinedQuery();
            }
        }

        public Boolean IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                NotifyPropertyChanged("IsBusy");
            }
        }


        public ObservableCollection<AlbumOfMP3ViewModel> AlbumsReturned
        {
            get { return albumsReturned; }
            set
            {
                albumsReturned = value;
                NotifyPropertyChanged("AlbumsReturned");
            }
        }

        

        #endregion

        #region Private Methods
        /// <summary>
        /// Runs the Query using the Expression tree for the Query
        /// and then does some LINQ Grouping into Albums, such that
        /// the Album cover image can be searched for and all related
        /// MP3s that go with the album are kept together
        /// </summary>
        /// <param name="expr">The Expression tree for the Query</param>
        private void RunQuery(Expression<Func<MP3, Boolean>> expr)
        {
            try
            {
                //use a Threadpool thread to run the query
                ThreadPool.QueueUserWorkItem(x =>
                {
        #if DEBUG
                    Stopwatch stopWatch = new Stopwatch();
                    stopWatch.Start();
                    Console.WriteLine("=========== START OF QUERY ===========");
        #endif
                    IsBusy = true;
                    
                    MP3s MP3 = new MP3s();
                    IQueryable<MP3> query = MP3.Files.Where<MP3>(expr);

                    //Create a concrete list
                    var mp3sMatched = query.ToList();

                    //group the result of the matched MP3s into albums
                    var albumsOfMP3s =
                      from mp3 in mp3sMatched
                      group mp3 by mp3.Album;

                    //Now create a ObservableCollection of the grouped results
                    //just because an ObservableCollection is easier to bind to
                    //then a Dictionary which isnt even Observable

                    //This grouping is the grouping of tracks to Albums
                    ObservableCollection<AlbumOfMP3ViewModel> albums = 
                        new ObservableCollection<AlbumOfMP3ViewModel>();

                    double animationOffset = 100;
                    double currentAnimationTime = 0;

                    //Didn't want to use for loop here, as its grouped,
                    //foreach is better, for grouped LINQ objects

                    //Allocate Albums with tracks
                    foreach (var album in albumsOfMP3s)
                    {
                        List<MP3> albumFiles = album.ToList();
                        AlbumOfMP3ViewModel albumOfMP3s = new AlbumOfMP3ViewModel
                                {
                                    Album = albumFiles.First().Album,
                                    Artist = albumFiles.First().Artist,
                                    Files = albumFiles
                                };
                        albumOfMP3s.ObtainImageForAlbum();
                        albumOfMP3s.AnimationDelayMs = currentAnimationTime += animationOffset;
                        albums.Add(albumOfMP3s);
                    }
                    //Store the Albums
                    AlbumsReturned = albums;

        #if DEBUG
                    stopWatch.Stop();
                    TimeSpan ts = stopWatch.Elapsed;
                    String elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                        ts.Hours, ts.Minutes, ts.Seconds,
                        ts.Milliseconds / 10);

                    Console.WriteLine(String.Format(" It took {0} to run the query", 
                        elapsedTime));
                    Console.WriteLine("=========== END OF QUERY ===========");
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


        /// <summary>
        /// Run when the user picks a Query type from MainWindow
        /// or the Keyword changes
        /// </summary>
        private void CreateCombinedQuery()
        {
            if (!currentKeyWord.Equals(String.Empty))
            {
                switch (CurrentQueryType)
                {
                    case QueryTypes.ByArtistName:
                        queryExpression =
                            mp3 => mp3.Artist.ToLower().
                                Contains(currentKeyWord.ToLower());
                        break;

                    case QueryTypes.BySongName:
                        queryExpression =
                            mp3 => mp3.Title.ToLower().
                                Contains(currentKeyWord.ToLower());
                        break;
                }
            }
        }
        #endregion

        #region Public Methods
        public void ReadInitialFolders()
        {
            try
            {
                IsBusy = true;
                XMLAndSQLQueryOperations.PopulateXmlWithLibraryDirectories();
                IsBusy = false;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Ooops, its busted " + ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }
        #endregion

    }
}
