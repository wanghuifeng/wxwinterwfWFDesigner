using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Windows.ApplicationModel.DataTransfer;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace PuzzleMetro
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage : Page
    {
        private readonly int[] _bordersNums = { 0, 4, 8, 12, 3, 7, 11, 15 };
        private readonly Random _rnd;
        private readonly DispatcherTimer _timer;

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        private int _moves;
        private DateTime _startTime;


        public BlankPage()
        {
            InitializeComponent();

            _rnd = new Random();
            _timer = new DispatcherTimer();
            _timer.Tick +=_timer_Tick;
            _timer.Interval = new TimeSpan(0, 0, 0, 1);

            _dataTransferManager = DataTransferManager.GetForCurrentView();
            _dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(_dataTransferManager_DataRequested);
        }

        void _timer_Tick(object sender, object e)
        {
            var time = DateTime.Now - _startTime;
            txtTime.Text = string.Format(Const.TimeFormat, time.Hours, time.Minutes, time.Seconds);
        }

        /// <summary>
        /// The Range of all Stackpanels is between 15 and 0, when 15 is the first (top left) and 0 is last (right bottom).
        /// 15 , 14 , 13 , 12
        /// 11 , 10 , 09 , 08
        /// 07 , 06 , 05 , 04
        /// 03 , 02 , 01 , 00
        /// The values are 1 to 16, meaning that 15 equals 1 and 00 equals 16
        /// </summary>
        public void NewGame()
        {
            _moves = 0;
            txtMoves.Text = "0";
            txtTime.Text = Const.DefaultTimeValue;

            Scrambles();
            while (!CheckIfSolvable())
            {
                Scrambles();
            }

            _startTime = DateTime.Now;
            _timer.Start();


        }

        /// <summary>
        /// Find the parent of image with a specific Tag value
        /// </summary>
        /// <param name="tag">The tag value of image you want to find.</param>
        /// <returns>Return the Stackpanel contains the image.</returns>
        Border FindStackPanelByTagId(int tag)
        {
            if (tag == 16)
            {
                return (from stackPanel in ContentPanel.Children.OfType<Border>() where stackPanel.Child == null select stackPanel).First();
            }
            else
            {
                foreach (Border b in ContentPanel.Children)
                {
                    if (b.Child != null && Convert.ToInt32(((TextBlock)b.Child).Text) == tag)
                        return b;
                }

                //return (from stackPanel in ContentPanel.Children.OfType<Border>()
                //        where Convert.ToInt32((TextBlock)stackPanel.Child) == tag
                //        select stackPanel).First();
            }
            return null;
        }

        /// <summary>
        /// Find the position of stackpanel without childrens.
        /// </summary>
        /// <returns></returns>
        int FindEmptyItemPosition()
        {
            int index = 15;
            for (int i = 0; i < 15; i++)
            {
                if (((Border)ContentPanel.Children[i]).Child != null)
                    return index;

                index--;
            }
            return 0;
        }

        /// <summary>
        /// Get the Tag value by StackPanel position.
        /// </summary>
        /// <param name="position">position of StackPanel</param>
        /// <returns>The Image Tag value, if there is no images then returns - 16</returns>
        int FindItemValueByPosition(int position)
        {
            return ((Border)ContentPanel.Children[position]).Child != null ?
                Convert.ToInt32(((TextBlock)((Border)ContentPanel.Children[position]).Child).Tag) : 16;
        }

        /// <summary>
        /// Runs n times and generate random numbers from 1 to 16, for each number find the current stackpanel that hold him.(FindStackPanelByTagId)
        /// If First and Second number are smaller then 16 then - swipe the images and tag values.
        /// If One of the values is 16 the swipe  - One Spackpanel will be cleared of Items.
        /// </summary>
        void Scrambles()
        {
            var count = 0;
            while (count < 25)
            {
                var a = _rnd.Next(1, 17);
                var b = _rnd.Next(1, 17);

                if (a == b) continue;

                var stack1 = FindStackPanelByTagId(a);
                var stack2 = FindStackPanelByTagId(b);

                if (a == 16)
                {
                    var image2 = stack2.Child;
                    stack2.Child = null;
                    stack1.Child = image2;
                }
                else if (b == 16)
                {
                    var image1 = stack1.Child;
                    stack1.Child = null;
                    stack2.Child = image1;
                }
                else
                {
                    var image1 = stack1.Child;
                    var image2 = stack2.Child;

                    stack1.Child = null;
                    stack2.Child = null;

                    stack1.Child = image2;
                    stack2.Child = image1;
                }

                count++;
            }
        }

        /// <summary>
        /// Each move the user do, perform a loop and checks values from 1 to 16.
        /// if the numbers are not in the currect order than nothing happeds.
        /// </summary>
        void CheckBoard()
        {
            var index = 1;
            for (var i = 15; i > 0; i--)
            {
                if (FindItemValueByPosition(i) != index) return;
                index++;
            }

            _timer.Stop();
            DisplayToastWithImage();
            WinGrid.Visibility = Windows.UI.Xaml.Visibility.Visible;
        }

        /// <summary>
        /// Check if the current Scramble is solveable.
        /// </summary>
        /// <returns></returns>
        bool CheckIfSolvable()
        {
            var n = 0;
            for (var i = 15; i > 0; i--)
            {
                if (!(ContentPanel.Children[i] is StackPanel)) continue;

                var num1 = FindItemValueByPosition(i);
                var num2 = FindItemValueByPosition(i - 1);

                if (num1 > num2)
                {
                    n++;
                }
            }

            var emptyPos = FindEmptyItemPosition();
            return n % 2 == (emptyPos + emptyPos / 4) % 2 ? true : false;

            //Mod 2 = (ecell + ecell \ 4) Mod 2 Then CheckIfSolvable = True Else CheckIfSolvable = False

        }

        /// <summary>
        /// Move Item From One SpackPanel to Another.
        /// </summary>
        /// <param name="item">Gets the Image item you want to move</param>
        /// <param name="targetPanel">Destination StackPanel</param>
        void MoveItem(TextBlock item, Border targetPanel)
        {
            foreach (var stackPanel in
                ContentPanel.Children.OfType<Border>().Where(stackPanel => stackPanel.Child != null && ((TextBlock)stackPanel.Child).Text == ((TextBlock)item).Text))
            {
                stackPanel.Child = null;
            }

            targetPanel.Child = item;
        }

        /// <summary>
        /// Bug Fix - if both of the items you want to swipe are in the Board borders do nothing.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        bool IsBorderSwich(int a, int b)
        {
            return _bordersNums.Contains(a) && _bordersNums.Contains(b);
        }

        /// <summary>
        /// Check if the Item Can move, Checking all panels around the specific item with -1 +1 -4 +4, if one of them is empty then he can move.
        /// </summary>
        /// <param name="itemToMove">The Item that has been click by user.</param>
        /// <returns></returns>
        Border CanMove(UIElement itemToMove)
        {
            var val = ((TextBlock)itemToMove).Text;
            var count = ContentPanel.Children.Count;
            for (var i = 0; i < count; i++)
            {
                if (!(ContentPanel.Children[i] is Border)) continue;

                var stakePanel = (Border)ContentPanel.Children[i];
                if (stakePanel.Child != null && ((TextBlock)stakePanel.Child).Text != val) continue;

                if (!IsBorderSwich(i, i + 1) && i + 1 <= 15 && ContentPanel.Children[i + 1] != null && ((Border)ContentPanel.Children[i + 1]).Child == null)
                    return ((Border)ContentPanel.Children[i + 1]);

                if (!IsBorderSwich(i, i - 1) && i - 1 > -1 && ContentPanel.Children[i - 1] != null && ((Border)ContentPanel.Children[i - 1]).Child == null)
                    return ((Border)ContentPanel.Children[i - 1]);

                if (i + 4 <= 15 && ContentPanel.Children[i + 4] != null && ((Border)ContentPanel.Children[i + 4]).Child == null)
                    return ((Border)ContentPanel.Children[i + 4]);

                if (i - 4 > -1 && ContentPanel.Children[i - 4] != null && ((Border)ContentPanel.Children[i - 4]).Child == null)
                    return ((Border)ContentPanel.Children[i - 4]);

            }
            return null;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            NewGame();
        }

        private void UserControl_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerEventArgs e)
        {
            var item = (UIElement)e.OriginalSource;
            TextBlock work = null;
            Border border = null;

            if (item is Border)
            {
                border = item as Border;
                if (border.Child != null)
                {
                    work = border.Child as TextBlock;
                }
                else
                    return;
            }
            else if (item is TextBlock)
            {
                work = item as TextBlock;
                border = work.Parent as Border;
            }
            else
                return;

            var to = CanMove(work);

            if (to != null)
            {
                _moves++;
                txtMoves.Text = _moves.ToString();
                CreateFadeOutAnimation(work);
                MoveItem(work, to);
                CreateFadeInAnimation(work);

                CheckBoard();
            }

            e.Handled = true;
        }

        private void CreateFadeOutAnimation(UIElement btn)
        {
            Duration dur = new Duration(TimeSpan.FromSeconds(1));

            var da = new DoubleAnimation();
            da.AutoReverse = false;
            da.From = 1;
            da.To = 0;

            var sb = new Storyboard();
            sb.Duration = dur;
            sb.Children.Add(da);

            Storyboard.SetTarget(da, btn);
            Storyboard.SetTargetProperty(da, "(Opacity)");

            sb.Begin();            
        }

        private void CreateFadeInAnimation(UIElement btn)
        {
            Duration dur = new Duration(TimeSpan.FromSeconds(1));

            var da = new DoubleAnimation();
            da.AutoReverse = false;
            da.From = 0;
            da.To = 1;

            var sb = new Storyboard();
            sb.Duration = dur;
            sb.Children.Add(da);

            Storyboard.SetTarget(da, btn);
            Storyboard.SetTargetProperty(da, "(Opacity)");

            sb.Begin();

        }

        private void btnnewGame_Click(object sender, RoutedEventArgs e)
        {
            NewGame();
        }

        void DisplayToastWithImage()
        {
            // GetTemplateContent returns a Windows.Data.Xml.Dom.XmlDocument object containing
            // the toast XML
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText01);

            // You can use the methods from the XML document to specify all of the
            // required parameters for the toast
            XmlNodeList imageElements = toastXml.GetElementsByTagName("image");
            XmlElement imageElement = (XmlElement)imageElements.Item(0);
            imageElement.SetAttribute("src", "package://images\\Winner.png");
            imageElement.SetAttribute("alt", "Placeholder image");

            XmlNodeList textElements = toastXml.GetElementsByTagName("text");
            for (uint i = 0; i < textElements.Length; i++)
            {
                textElements.Item(i).AppendChild(toastXml.CreateTextNode("Congratulations You Won!!!"));
            }

            // Create a toast from the Xml, then create a ToastNotifier object to show
            // the toast
            ToastNotification toast = new ToastNotification(toastXml);

            // If you have other applications in your package, you can specify the AppId of
            // the app to create a ToastNotifier for that application
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }

        private DataTransferManager _dataTransferManager;

        void _dataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            args.Request.Data.Properties.Title = "Metro Puzzle";
            if (_timer.IsEnabled)
            {
                args.Request.Data.Properties.Description = "Share Metro Application";
                args.Request.Data.SetText(string.Format("Got Windows 8? You Should Download Metro Puzzle - {0}", Const.DownloadLink));
            }
            else
            {
                args.Request.Data.Properties.Description = "Share Win";
                args.Request.Data.SetText(string.Format("I've just finish Metro Puzzle in {0} moves in {1}, think you can beat me? {2}", txtMoves.Text, txtTime.Text, Const.DownloadLink));
            }
        }

        private void btnGame_Click(object sender, RoutedEventArgs e)
        {
            NewGame();
            WinGrid.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private void btnShare_Click(object sender, RoutedEventArgs e)
        {
            DataTransferManager.ShowShareUI();
            WinGrid.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }
    }
}
