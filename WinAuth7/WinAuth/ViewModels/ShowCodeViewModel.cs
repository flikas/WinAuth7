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
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WindowsPhoneAuthenticator;

namespace WinAuth.ViewModels
{
    public class ShowCodeViewModel : INotifyPropertyChanged
    {
        private Timer timer;
        private string lastCode ="";
        private int counter;
        private String restoreCode;
        private String serial;
        private DateTime NextRefresh;
        private Boolean isSyncing = false;
        public  ShowCodeViewModel()
        {
            lastCode = App.CurrentAuthenticator.CurrentCode;
            counter = (int)((App.CurrentAuthenticator.ServerTime % 30000L) / 1000L);
            restoreCode = App.CurrentAuthenticator.RestoreCode;
            serial = App.CurrentAuthenticator.Serial;
            NextRefresh = DateTime.Now;
            timer = new Timer(MyTimerCallback, App.CurrentAuthenticator, 0, 500);
        }
        public String Serial
        {
            get
            {
                return serial;
            }
        }
        public String RestoreCode
        {
            get
            {
                return restoreCode;
            }
        }
        public String Code
        {
            get
            {
                return lastCode;
            }
        }

        public int Counter
        {
            get
            {
                return 30 - counter;
            }
        }

        public Boolean IsSyncing
        {
            get
            {
                return isSyncing;
            }
            set
            {
                isSyncing = value;
                NotifyPropertyChanged("IsSyncing");
            }
        }
        

        private void MyTimerCallback(object state)
        {
            Authenticator au = state as Authenticator;
            counter = (int)((au.ServerTime % 30000L) / 1000L);
            DateTime now = DateTime.Now;
            if (counter == 0)
            {
                NextRefresh = now;
            }
            //if (au.CurrentCode != lastCode)
            //{
            //    lastCode = au.CurrentCode;
            //    Deployment.Current.Dispatcher.BeginInvoke(new Action(() =>
            //    {
            //        NotifyPropertyChanged("Code");
            //    }));
            //}
            Deployment.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                NotifyPropertyChanged("Counter");
                if (now >= NextRefresh)
                {
                    lastCode = au.CurrentCode;
                    NotifyPropertyChanged("Code");
                    NextRefresh.AddSeconds(30);
                }
            }));

        }

        public async Task  SyncCode()
        {
            IsSyncing = true;
            try
            {
                await App.CurrentAuthenticator.Sync();
                int i = App.AuthenticatorsViewModel.Authenticators.IndexOf(App.CurrentAuthenticator);
                App.AuthenticatorsViewModel.Authenticators[i] = App.CurrentAuthenticator;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                IsSyncing = false;
            }
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
