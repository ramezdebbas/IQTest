using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using IQMindTest.Common;
using Microsoft.WindowsAzure.MobileServices;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using Windows.Storage.Streams;
// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace IQMindTest
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class Questions : IQMindTest.Common.LayoutAwarePage
    {
        private IMobileServiceTable<IQ> itemsTable = App.MobileService.GetTable<IQ>();
        private List<IQ> ques = new List<IQ>();
        private static IQ curQuestion = new IQ();
        private int curIndex = 0;
        private int total = 0;

        public Questions()
        {
            this.InitializeComponent();
            loadData();
        }

        private async void loadData()
        {
            ques = await itemsTable.ToListAsync();
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
            if (Cques.UserAns == 1) Opt1.IsChecked = true;
            else if (Cques.UserAns == 2) Opt2.IsChecked = true;
            else if (Cques.UserAns == 3) Opt3.IsChecked = true;
            else if (Cques.UserAns == 4) Opt4.IsChecked = true;
            else if (Cques.UserAns == 5) Opt5.IsChecked = true;
            else Opt1.IsChecked = true;

            questionNo.Text = c + "/" + t;
        }
        
        private async void ShowDlg()
        {
            var msgDlg = new MessageDialog("You IQ score is " + CalculateScore(), "IQ Score");
            await msgDlg.ShowAsync();
            
            Store();
           
        }

        private void DoNothing()
        {
            
        }

        private int CalculateScore()
        {
            int score = 0;
            int weight = (int)(200 / total);
            foreach (IQ q in ques)
            {

                if (q.UserAns == q.ans)
                {
                    score += weight;
                }
            }

            return score;
        }

        private async void Store()
        {

            LocalStorage.Data = ques;
            await LocalStorage.Save<IQ>();
            this.Frame.Navigate(typeof(Answers));
        }

        async public Task<string> ReadAllTextAsync(StorageFile storageFile)
        {
            string content;
            var inputStream = await storageFile.OpenAsync(FileAccessMode.Read);
            var readStream = inputStream.GetInputStreamAt(0);
            var reader = new DataReader(readStream);
            uint fileLength = await reader.LoadAsync((uint)inputStream.Size);
            content = reader.ReadString(fileLength);
            return content;
        } 

        async public Task WriteAllTextAsync(StorageFile storageFile, string content)
        {
            var inputStream = await storageFile.OpenAsync(FileAccessMode.ReadWrite);
            var writeStream = inputStream.GetOutputStreamAt(0);
            DataWriter writer = new DataWriter(writeStream);
            writer.WriteString(content);
            await writer.StoreAsync();
            await writeStream.FlushAsync();
        }

     
        private void Opt_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)Opt1.IsChecked) ques[curIndex].UserAns = 1;
            else if ((bool)Opt2.IsChecked) ques[curIndex].UserAns = 2;
            else if ((bool)Opt3.IsChecked) ques[curIndex].UserAns = 3;
            else if ((bool)Opt4.IsChecked) ques[curIndex].UserAns = 4;
            else if ((bool)Opt5.IsChecked) ques[curIndex].UserAns = 5;
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

        private void Prev_OnClick(object sender, RoutedEventArgs e)
        {
            if (curIndex > 0)
            {
                curIndex--;
                curQuestion = ques[curIndex];
                setData(curQuestion, (curIndex + 1), total);
            }
        }

        private async void Submit_OnClick(object sender, RoutedEventArgs e)
        {
            Action cmdAction = null;
            var msgDlg = new MessageDialog("Are you sure you want to submit", "Confirmation");
            msgDlg.Commands.Add(new UICommand("OK", (x) =>
            {
                cmdAction = () => ShowDlg();
            }));
            msgDlg.Commands.Add(new UICommand("Cancel", (x) =>
            {
                cmdAction = () => DoNothing();
            }));

            msgDlg.DefaultCommandIndex = 0;
            msgDlg.CancelCommandIndex = 1;

            await msgDlg.ShowAsync();
            cmdAction.Invoke();
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
    }
}
