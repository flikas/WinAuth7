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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WindowsPhoneAuthenticator;

namespace WinAuth.ViewModels
{
    /// <summary>
		/// Inner class holding the regions information
		/// </summary>
   public class BattleNetRegion
    {
        /// <summary>
        /// Code for region, e.g. "US"
        /// </summary>
       public string Code { get; private set; }

        /// <summary>
        /// Display name of region, e.g. "Americas"
        /// </summary>
       public string Name { get; private set; }

        /// <summary>
        /// Create a new BattleNetRegion object
        /// </summary>
        /// <param name="code">region code</param>
        /// <param name="name">region name</param>
        public BattleNetRegion(string code, string name)
        {
            Code = code;
            Name = name;
        }

        /// <summary>
        /// Get the display sting for the region
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name;
        }
    }

    public class NewAuthenticatorViewModel : INotifyPropertyChanged
    {
        private String description;
        private String serial;
        private String restorecode;
        private Authenticator newAuthenticator;
        private Boolean isEnrolling;
        private Boolean isRestoring;
        private ObservableCollection<BattleNetRegion> regions = new ObservableCollection<BattleNetRegion>
		{
			new BattleNetRegion("US", "美帝"),
			new BattleNetRegion("EU", "欧萌"),
			new BattleNetRegion("CN", "天朝"),
			new BattleNetRegion("KR", "棒子"),
			new BattleNetRegion("TW", "呆湾")
		};
        public NewAuthenticatorViewModel()
        {
            SelectedRegion = regions[0];
            serial = "";
            restorecode = "";
            isEnrolling = false;
            isRestoring = false;
        }
        public BattleNetRegion SelectedRegion { get; set; }

        public Boolean IsEnrolling
        {
            get
            {
                return isEnrolling;
            }
            private set
            {
                isEnrolling = value;
                NotifyPropertyChanged("IsEnrolling");
            }
        }

        public Boolean IsRestoring
        {
            get
            {
                return isRestoring;
            }
            set
            {
                isRestoring = value;
                NotifyPropertyChanged("IsRestoring");
            }
        }

        public ObservableCollection<BattleNetRegion> BattleNetRegions
        {
            get
            {
                return regions;
            }

        }
        public String Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
            }

        }

        public String Serial
        {
            get
            {
                return serial;
            }
            set
            {
                serial = value;
            }
        }

        public String RestoreCode
        {
            get
            {
                return restorecode;
            }
            set
            {
                restorecode = value;
            }
        }

        public async Task<Authenticator> EnrollAuthenticator()
        {
            IsEnrolling = true;
            try
            {
                newAuthenticator = new Authenticator(Description);
                string region = SelectedRegion.Code;
                await newAuthenticator.Enroll(region);
            }
            catch (Exception ex)
            {

                newAuthenticator = null;
                throw ex;
            }
            finally
            {
                IsEnrolling = false;
              
            }
            return newAuthenticator;
        }

        public async Task<Authenticator> RestoreAuthenticator()
        {
            if (Serial == String.Empty || RestoreCode == String.Empty)
            {
                newAuthenticator = null;
                throw new Exception("无效的序列号或还原码");
            }
            IsRestoring = true;
            try
            {
                newAuthenticator = new Authenticator(Description);
                
                await newAuthenticator.Restore(Serial, RestoreCode);
            }
            catch (Exception ex)
            {

                newAuthenticator = null;
                throw ex;
            }
            finally
            {
                IsRestoring = false;
            }
            return newAuthenticator;
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
