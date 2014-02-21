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
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WinAuth.ViewModels;

namespace WinAuth
{
    public partial class ShowCode : PhoneApplicationPage
    {
        private ShowCodeViewModel coderModel;
        private ApplicationBarIconButton appBarSyncButton;
        public ShowCode()
        {
            InitializeComponent();
            SetupAppBar();
            coderModel = new ShowCodeViewModel();
            this.DataContext = coderModel;
            
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
           // coderModel.SyncCode();
        }

        private void SetupAppBar()
        {
            // 将页面的 ApplicationBar 设置为 ApplicationBar 的新实例。
            ApplicationBar = new ApplicationBar();

            // 创建新按钮并将文本值设置为 AppResources 中的本地化字符串。
            appBarSyncButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/sync.png", UriKind.Relative));
            appBarSyncButton.Text = "同步";
            appBarSyncButton.Click += appBarSyncButton_Sync;
            ApplicationBar.Buttons.Add(appBarSyncButton);
        }

        private async void appBarSyncButton_Sync(object sender, EventArgs e)
        {
            try
            {
                appBarSyncButton.IsEnabled = false;
                await coderModel.SyncCode();
            }
            catch (Exception ex)
            {
                MessageBox.Show("同步失败：" + ex.Message, "错误", MessageBoxButton.OK);
            }
            finally
            {
                appBarSyncButton.IsEnabled = true;
            }
        }
    }
}