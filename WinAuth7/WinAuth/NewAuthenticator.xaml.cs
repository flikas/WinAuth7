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
using System.Windows.Input;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WinAuth.ViewModels;
using WindowsPhoneAuthenticator;

namespace WinAuth
{
    public partial class NewAuthenticator : PhoneApplicationPage
    {
        private NewAuthenticatorViewModel newAuthenticator;
        private ApplicationBarIconButton appBarButton;
        public NewAuthenticator()
        {
            InitializeComponent();
            newAuthenticator = new NewAuthenticatorViewModel();
            this.DataContext = newAuthenticator;
        }


        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            NavigationService.RemoveBackEntry();
        }

        private void SetupEnrollBar()
        {
            // 将页面的 ApplicationBar 设置为 ApplicationBar 的新实例。
            ApplicationBar = new ApplicationBar();

            // 创建新按钮并将文本值设置为 AppResources 中的本地化字符串。
            appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/enroll.png", UriKind.Relative));
            appBarButton.Text = "注册";
            appBarButton.Click += appBarButton_Enroll;
            ApplicationBar.Buttons.Add(appBarButton);
        }

        private void SetupRestoreBar()
        {
            // 将页面的 ApplicationBar 设置为 ApplicationBar 的新实例。
            ApplicationBar = new ApplicationBar();

            // 创建新按钮并将文本值设置为 AppResources 中的本地化字符串。
            appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/restore.png", UriKind.Relative));
            appBarButton.Text = "还原";
            appBarButton.Click += appBarButton_Restore;
            ApplicationBar.Buttons.Add(appBarButton);
        }

        private async void appBarButton_Restore(object sender, EventArgs e)
        {
            object focusObj = FocusManager.GetFocusedElement();
            if (focusObj != null && focusObj is PhoneTextBox)
            {
                var binding = (focusObj as PhoneTextBox).GetBindingExpression(TextBox.TextProperty);
                binding.UpdateSource();
            }
            try
            {
                appBarButton.IsEnabled = false;
                Authenticator au = await newAuthenticator.RestoreAuthenticator();
                if (au != null)
                {
                    App.AuthenticatorsViewModel.Authenticators.Add(au);
                    App.CurrentAuthenticator = au;
                    NavigationService.Navigate(new Uri("/ShowCode.xaml", UriKind.RelativeOrAbsolute));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "还原失败:\n\n" + ex.Message, "错误", MessageBoxButton.OK);
            }
            finally
            {
                appBarButton.IsEnabled = true;
            }

        }


        private async void appBarButton_Enroll(object sender, EventArgs e)
        {
            object focusObj = FocusManager.GetFocusedElement();
            if (focusObj != null && focusObj is PhoneTextBox)
            {
                var binding = (focusObj as PhoneTextBox).GetBindingExpression(TextBox.TextProperty);
                binding.UpdateSource();
            }
            try
            {
                appBarButton.IsEnabled = false;
                Authenticator au = await newAuthenticator.EnrollAuthenticator();
                if (au != null)
                {
                    App.AuthenticatorsViewModel.Authenticators.Add(au);
                    App.CurrentAuthenticator = au;
                    NavigationService.Navigate(new Uri("/ShowCode.xaml", UriKind.RelativeOrAbsolute));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "注册失败:\n\n" + ex.Message, "错误", MessageBoxButton.OK);
            }
            finally
            {
                appBarButton.IsEnabled = true;
            }

        }

        private void Pivot_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            var pivot = sender as Pivot;
            if (pivot.SelectedIndex == 0)
            {
                SetupEnrollBar();
            }
            else if(pivot.SelectedIndex == 1)
            {
                SetupRestoreBar();
            }
        }
    }
}