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
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WinAuth.Resources;
using WinAuth.ViewModels;
using WindowsPhoneAuthenticator;

namespace WinAuth
{
    public partial class MainPage : PhoneApplicationPage
    {
        // 构造函数
        public MainPage()
        {
            InitializeComponent();

            //this.DataContext = App.AuthenticatorsViewModel;
            //  用于本地化 ApplicationBar 的示例代码
            //  BuildLocalizedApplicationBar();
            SetupCreateApplicationBar();
        }

        // 用于生成本地化 ApplicationBar 的示例代码
        private void SetupCreateApplicationBar()
        {
            // 将页面的 ApplicationBar 设置为 ApplicationBar 的新实例。
            ApplicationBar = new ApplicationBar();

            // 创建新按钮并将文本值设置为 AppResources 中的本地化字符串。
            ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/add.png", UriKind.Relative));
            appBarButton.Text = "添加";
            appBarButton.Click += appBarButton_Click;

            //ApplicationBarIconButton appSelectButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/select.png", UriKind.Relative));
            //appSelectButton.Text = "选择";
            //appSelectButton.Click += appBarSelectButton_Click;

            ApplicationBar.Buttons.Add(appBarButton);
            //ApplicationBar.Buttons.Add(appSelectButton);

            // 使用 AppResources 中的本地化字符串创建新菜单项。
            // ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
            //  ApplicationBar.MenuItems.Add(appBarMenuItem);
        }

        //private void SetupSelectApplicationBar()
        //{
        //    // 将页面的 ApplicationBar 设置为 ApplicationBar 的新实例。
        //    ApplicationBar = new ApplicationBar();



        //    // 使用 AppResources 中的本地化字符串创建新菜单项。
        //    // ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    //  ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}


        void ClearApplicationBar()
        {
            while (ApplicationBar.Buttons.Count > 0)
            {
                ApplicationBar.Buttons.RemoveAt(0);
            }

            while (ApplicationBar.MenuItems.Count > 0)
            {
                ApplicationBar.MenuItems.RemoveAt(0);
            }
        }

        void appBarButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/NewAuthenticator.xaml", UriKind.Relative));
        }

        //void appBarSelectButton_Click(object sender, EventArgs e)
        //{
        //    this.ListAuth.IsSelectionEnabled = true;
        //}

        //导航页面以将数据上下文设置为列表中的所选项时
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.AuthenticatorsViewModel.IsDataLoaded)
            {
                this.DataContext = null;
                await App.AuthenticatorsViewModel.LoadData();
                this.DataContext = App.AuthenticatorsViewModel;
            }
        }

        //private void LongListMultiSelector_IsSelectionEnabledChanged_1(object sender, DependencyPropertyChangedEventArgs e)
        //{
        //    LongListMultiSelector llist = sender as LongListMultiSelector;
        //    ClearApplicationBar();
        //    if (llist.IsSelectionEnabled)
        //    {
        //        SetupSelectApplicationBar();
        //    }
        //    else
        //    {
        //        SetupCreateApplicationBar();
        //    }

        //}

        //private void ListAuth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    ListBox llist = sender as ListBox;
        //    if (llist.SelectedItems.Count > 0)
        //        deleteBar.IsEnabled = true;
        //    else
        //        deleteBar.IsEnabled = false;
        //}

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            //if (this.ListAuth.IsSelectionEnabled == true)
            //{
            //    this.ListAuth.IsSelectionEnabled = false;
            //    e.Cancel = true;
            //}
        }

        private void Grid_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var au = ((FrameworkElement)sender).DataContext as Authenticator;
            App.CurrentAuthenticator = au;
            NavigationService.Navigate(new Uri("/ShowCode.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}