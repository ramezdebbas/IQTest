using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IQMindTest.Common;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace IQMindTest
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class Answers : IQMindTest.Common.LayoutAwarePage
    {
        private List<IQ> ques = new List<IQ>();
        private static IQ curQuestion = new IQ();
        private int curIndex = 0;
        private int total = 0;

        public Answers()
        {
            this.InitializeComponent();
            load();
        }

        private async void load()
        {

            try
            {

                await LocalStorage.Restore<IQ>();
                ques = LocalStorage.Data;

            }
            catch
            {

            }

            total = ques.Count;
            curQuestion = ques[curIndex];

            setData(curQuestion, (curIndex + 1), total);

        }

        private void setData(IQ Cques, int c, int t)
        {

            question.Text = Cques.question;
            Opt1.Content = Cques.opt1;
            Opt2.Content = Cques.opt2;
            Opt3.Content = Cques.opt3;
            Opt4.Content = Cques.opt4;
            Opt5.Content = Cques.opt5;
            if (Cques.ans == 1)
            {
                Opt1.IsChecked = true;
                imgAns.Source = Cques.UserAns == 1 ? new BitmapImage(new Uri("ms-appx:/Assets/c.png", UriKind.RelativeOrAbsolute)) : new BitmapImage(new Uri("ms-appx:/Assets/w.png", UriKind.RelativeOrAbsolute));
            }
            else if (Cques.ans == 2)
            {
                Opt2.IsChecked = true;
                imgAns.Source = Cques.UserAns == 2 ? new BitmapImage(new Uri("ms-appx:/Assets/c.png", UriKind.RelativeOrAbsolute)) : new BitmapImage(new Uri("ms-appx:/Assets/w.png", UriKind.RelativeOrAbsolute));
            }
            else if (Cques.ans == 3)
            {
                Opt3.IsChecked = true;
                imgAns.Source = Cques.UserAns == 3 ? new BitmapImage(new Uri("ms-appx:/Assets/c.png", UriKind.RelativeOrAbsolute)) : new BitmapImage(new Uri("ms-appx:/Assets/w.png", UriKind.RelativeOrAbsolute));
            }
            else if (Cques.ans == 4)
            {
                Opt4.IsChecked = true;
                imgAns.Source = Cques.UserAns == 4 ? new BitmapImage(new Uri("ms-appx:/Assets/c.png", UriKind.RelativeOrAbsolute)) : new BitmapImage(new Uri("ms-appx:/Assets/w.png", UriKind.RelativeOrAbsolute));
            }
            else if (Cques.ans == 5)
            {
                Opt5.IsChecked = true;
                imgAns.Source = Cques.UserAns == 5 ? new BitmapImage(new Uri("ms-appx:/Assets/c.png", UriKind.RelativeOrAbsolute)) : new BitmapImage(new Uri("ms-appx:/Assets/w.png", UriKind.RelativeOrAbsolute));
            }

            questionNo.Text = c + "/" + t;

        }

        private void Prev_OnClick(object sender, RoutedEventArgs e)
        {
            if (curIndex > 0)
            {
                curIndex--;
                curQuestion = ques[curIndex];
                setData(curQuestion, (curIndex + 1), total);
            }
        }

        private void Next_OnClick(object sender, RoutedEventArgs e)
        {
            if ((curIndex + 1) < total)
            {
                curIndex++;
                curQuestion = ques[curIndex];
                setData(curQuestion, (curIndex + 1), total);
            }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        private void BackButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof (Home));
        }
    }
}
