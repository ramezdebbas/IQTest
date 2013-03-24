using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Storage;
using Windows.Storage.Streams;

namespace IQMindTest
{
    class LocalStorage
    {
        private static List<IQ> _data = new List<IQ>();

        public static List<IQ> Data
        {
            get { return _data; }
            set { _data = value; }
        }

        public static StorageFile File { get; set; }

        private const string Filename = "ans.xml";

        static async public Task Save<T>()
        {
            if (await DoesFileExistAsync(Filename))
            {
                await Windows.System.Threading.ThreadPool.RunAsync((sender) => LocalStorage.SaveAsync<T>().Wait(), Windows.System.Threading.WorkItemPriority.Normal);
            }
            else
            {
                File = await ApplicationData.Current.LocalFolder.CreateFileAsync(Filename, CreationCollisionOption.ReplaceExisting);
            }
        }

        static async public Task Restore<T>()
        {
            if (await DoesFileExistAsync(Filename))
            {
                await Windows.System.Threading.ThreadPool.RunAsync((sender) => LocalStorage.RestoreAsync<T>().Wait(), Windows.System.Threading.WorkItemPriority.Normal);
            }
            else
            {
                File = await ApplicationData.Current.LocalFolder.CreateFileAsync(Filename);
            }
        }

        static async Task<bool> DoesFileExistAsync(string fileName)
        {
            try
            {
                await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        static async private Task SaveAsync<T>()
        {
            StorageFile sessionFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(Filename, CreationCollisionOption.ReplaceExisting);
            IRandomAccessStream sessionRandomAccess = await sessionFile.OpenAsync(FileAccessMode.ReadWrite);
            IOutputStream sessionOutputStream = sessionRandomAccess.GetOutputStreamAt(0);
            var serializer = new XmlSerializer(typeof(List<IQ>), new Type[] { typeof(T) });

            //Using DataContractSerializer , look at the cat-class
            //var sessionSerializer = new DataContractSerializer(typeof(List<object>), new Type[] { typeof(T) });
            //sessionSerializer.WriteObject(sessionOutputStream.AsStreamForWrite(), _data);

            //Using XmlSerializer , look at the Dog-class
            serializer.Serialize(sessionOutputStream.AsStreamForWrite(), _data);
            sessionRandomAccess.Dispose();
            await sessionOutputStream.FlushAsync();
            sessionOutputStream.Dispose();
        }

        static async private Task RestoreAsync<T>()
        {
            StorageFile sessionFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(Filename, CreationCollisionOption.OpenIfExists);
            if (sessionFile == null)
            {
                return;
            }
            IInputStream sessionInputStream = await sessionFile.OpenReadAsync();

            //Using DataContractSerializer , look at the cat-class
            // var sessionSerializer = new DataContractSerializer(typeof(List<object>), new Type[] { typeof(T) });
            //_data = (List<object>)sessionSerializer.ReadObject(sessionInputStream.AsStreamForRead());

            //Using XmlSerializer , look at the Dog-class
            var serializer = new XmlSerializer(typeof(List<IQ>), new Type[] { typeof(T) });
            _data = (List<IQ>)serializer.Deserialize(sessionInputStream.AsStreamForRead());
            sessionInputStream.Dispose();
        }
    }

}
