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
using Coding4Fun.Toolkit.Controls;
using System.Windows.Media;

namespace WinAuth
{
    public partial class ShowCode : PhoneApplicationPage
    {
        private ShowCodeViewModel coderModel;
        private ApplicationBarIconButton appBarSyncButton;
        private ApplicationBarIconButton appBarDeleteButton;
        private ApplicationBarMenuItem appBarModifyDescription;
        private InputPrompt inpModifyDescription;

        public ShowCode()
        {
            InitializeComponent();
            SetupAppBar();
            try
            {
                coderModel = new ShowCodeViewModel();
            }
            catch (Exception ex)
            {
                MessageBox.Show("同步安全令失败，请检查网络连接并稍后重试: " + ex.Message);
                try
                {
                    NavigationService.GoBack();
                }
                catch { NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.RelativeOrAbsolute)); }
            }
            this.DataContext = coderModel;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (NavigationContext.QueryString.ContainsKey("IsNew") && NavigationContext.QueryString["IsNew"] == "y")
            {
                ShowRestoreCodeButton_Tap(null, null);
                appBarDeleteButton.IsEnabled = false;
            }
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

            appBarDeleteButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/delete.png", UriKind.Relative));
            appBarDeleteButton.Text = "删除";
            appBarDeleteButton.Click += appBarDeleteButton_Click;
            ApplicationBar.Buttons.Add(appBarDeleteButton);

            appBarModifyDescription = new ApplicationBarMenuItem("修改备注名...");
            appBarModifyDescription.Click += appBarModifyDescription_Click;
            ApplicationBar.MenuItems.Add(appBarModifyDescription);
        }

        void appBarModifyDescription_Click(object sender, EventArgs e)
        {
            inpModifyDescription = new InputPrompt
            {
                Title = "修改备注名",
                Message = "输入新备注名",
                Value = App.CurrentAuthenticator.Description
            };
            inpModifyDescription.Completed += inp_Completed;
            inpModifyDescription.Show();
        }

        void inp_Completed(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
            coderModel.Description = inpModifyDescription.Value;
            App.AuthenticatorsViewModel.SaveData();
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

        void appBarDeleteButton_Click(object sender, EventArgs e)
        {
            MessagePrompt msg = new MessagePrompt()
            {
                Title = "删除确认",
                Message = "警告：此操作不可恢复！删除安全令前请确认已经解绑安全令或备份了安全令序列号和恢复码！",
                IsCancelVisible = true
            };
            msg.Completed += msg_Completed;
            msg.Show();
        }

        void msg_Completed(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
            if (e.PopUpResult == PopUpResult.Ok)
            {
                App.AuthenticatorsViewModel.Authenticators.Remove(App.CurrentAuthenticator);
                if (NavigationService.CanGoBack)
                    NavigationService.GoBack();
                else
                    NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
        }

        private void ShowRestoreCodeButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            RotateTransform rtf = ShowRestoreCodeButton.RenderTransform as RotateTransform;
            if (rtf == null)
            {
                ShowRestoreCodeButton.RenderTransform = rtf = new RotateTransform();
            }
            if (rtf.Angle == 0)
            {
                SecurityCodes.Visibility = System.Windows.Visibility.Visible;
                rtf.Angle = 90;
            }
            else
            {
                SecurityCodes.Visibility = System.Windows.Visibility.Collapsed;
                rtf.Angle = 0;
            }
        }
    }
}