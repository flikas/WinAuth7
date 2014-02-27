/*
 * Copyright (C) 2013 Zhongmin Wu.
 * This software is distributed under the terms of the GNU General Public License.
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsPhoneAuthenticator;

namespace WinAuth.ViewModels
{
    public class AuthenticatorsViewModel : INotifyPropertyChanged
    {
        private static string AuthenticatorsStorageFolderName = "Authenticators";
        private bool isLoaded = false;
        private Authenticator selectedAuthenticator;
        public AuthenticatorsViewModel()
        {
            Authenticators = new ObservableCollection<Authenticator>();
            SelectedAuthenticators = new ObservableCollection<Authenticator>();
            IsDataLoaded = false;
        }

        async void Authenticators_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //  if (IsDataLoaded)
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    await SaveData(e.NewItems);
                }
                else if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    IList aus = e.OldItems;
                    await DeleteData(aus);
                }
                else if (e.Action == NotifyCollectionChangedAction.Replace)
                {
                    await DeleteData(e.OldItems);
                    await SaveData(e.NewItems);

                }
            }
        }

        public ObservableCollection<Authenticator> Authenticators { get; set; }
        public ObservableCollection<Authenticator> SelectedAuthenticators { get; set; }
        public Authenticator SelectedAuthenticator
        {
            get
            {
                return selectedAuthenticator;
            }
            set
            {
                selectedAuthenticator = value;
                NotifyPropertyChanged("SelectedAuthenticator");
            }
        }

        public bool IsDataLoaded
        {
            get
            {
                return isLoaded;
            }
            private set
            {
                isLoaded = value;
                NotifyPropertyChanged("IsDataLoaded");
            }
        }

        public async Task LoadData()
        {
            Authenticators.Clear();

            //StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;
            IsolatedStorageFile local = IsolatedStorageFile.GetUserStoreForApplication();
            //var AuthenticatorsStorageFolde = await local.CreateFolderAsync(AuthenticatorsStorageFolderName, CreationCollisionOption.OpenIfExists);
            local.CreateDirectory(AuthenticatorsStorageFolderName);

            //var AuthenticatorFiles = await AuthenticatorsStorageFolde.GetFilesAsync();
            Authenticators.CollectionChanged -= Authenticators_CollectionChanged;
            await Task.Factory.StartNew(() =>
            {
                //foreach (StorageFile file in AuthenticatorFiles)
                foreach (string file in local.GetFileNames(AuthenticatorsStorageFolderName + "/*"))
                {
                    using (Stream stream = new IsolatedStorageFileStream(AuthenticatorsStorageFolderName + "/" + file, FileMode.Open, local))
                    {
                        Authenticator au = Authenticator.GetFromJSonStream(stream);
                        if (au != null)
                            Authenticators.Add(au);
                    }
                }
            });
            Authenticators.CollectionChanged += Authenticators_CollectionChanged;
            IsDataLoaded = true;
        }

        public async Task SaveData()
        {
            //StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;
            //var AuthenticatorsStorageFolde = await local.CreateFolderAsync(AuthenticatorsStorageFolderName, CreationCollisionOption.OpenIfExists);

            //await Task.Factory.StartNew(new Action(() =>
            //{
            //    foreach (Authenticator au in Authenticators)
            //    {
            //        StorageFile file = await AuthenticatorsStorageFolde.CreateFileAsync(au.Serial, CreationCollisionOption.ReplaceExisting);
            //        using (Stream stream = await file.OpenStreamForWriteAsync())
            //        {
            //            Authenticator.SetFromJSonStream(au, stream);
            //        }
            //    }
            //}));
            //IsDataLoaded = false;
            await SaveData(Authenticators);
        }

        public async Task SaveData(IList aus)
        {
            //StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;
            IsolatedStorageFile local = IsolatedStorageFile.GetUserStoreForApplication();
            //var AuthenticatorsStorageFolde = await local.CreateFolderAsync(AuthenticatorsStorageFolderName, CreationCollisionOption.OpenIfExists);
            local.CreateDirectory(AuthenticatorsStorageFolderName);

            await Task.Factory.StartNew(() =>
            {
                foreach (Authenticator au in aus)
                {
                    //StorageFile file = await AuthenticatorsStorageFolde.CreateFileAsync(au.Serial, CreationCollisionOption.ReplaceExisting);
                    using (Stream stream = new IsolatedStorageFileStream(AuthenticatorsStorageFolderName + "/" + au.Serial, FileMode.Create, local))
                    {
                        Authenticator.SetFromJSonStream(au, stream);
                    }
                }
            });
            IsDataLoaded = false;
        }

        public async Task DeleteData()
        {
            //StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;
            IsolatedStorageFile local = IsolatedStorageFile.GetUserStoreForApplication();
            try
            {
                //var AuthenticatorsStorageFolde = await local.GetFolderAsync(AuthenticatorsStorageFolderName);
                //await AuthenticatorsStorageFolde.DeleteAsync();
                await Task.Factory.StartNew(() => local.DeleteDirectory(AuthenticatorsStorageFolderName));
            }
            catch
            {
            }
            IsDataLoaded = false;
        }

        public async Task DeleteData(IList aus)
        {
            //StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;
            IsolatedStorageFile local = IsolatedStorageFile.GetUserStoreForApplication();
            //var AuthenticatorsStorageFolde = await local.CreateFolderAsync(AuthenticatorsStorageFolderName, CreationCollisionOption.OpenIfExists);

            await Task.Factory.StartNew(() =>
            {
                foreach (Authenticator au in aus)
                {
                    //StorageFile file = await AuthenticatorsStorageFolde.CreateFileAsync(au.Serial, CreationCollisionOption.ReplaceExisting);
                    //await file.DeleteAsync();
                    try
                    {
                        local.DeleteFile(AuthenticatorsStorageFolderName + "/" + au.Serial);
                    }
                    catch
                    {
                    }
                }
            });
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
