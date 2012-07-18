///Here's the class I use to upgrade the 
///Property- grid content. It's makes the entire process
///easier and gives the endUser a better 
///overview of the aviable setting- configuration.
///
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;

namespace SC2_External_Hack_II
{
    [DefaultPropertyAttribute("Name")]
    public class Appsettings
    {
        private int _refreshrate;           //refreshrate for timer
        private string _nickname;           //nickname for own player
        private Point _ressource;
        private Point _income;
        private Point _informations;
        private Point _workers;
        private Point _maphack;
        private Point _production;
        private Point _apm;
        private Point _army;
        private Point _notification;
        private double _Oopacity;
        private bool _autoinject;

        private int _Ressource,
                    _Income,
                    _Worker,
                    _States,
                    _Maphack,
                    _Production,
                    _Apm,
                    _Settings,
                    _Army,
                    _inject;



        // Name property with category attribute and
        // description attribute added
        [CategoryAttribute("Overlay Settings"),
        DescriptionAttribute("Enter your refresh-rate for your timer!")]
        public int Refreshrate
        {
            get
            {
                return _refreshrate;
            }
            set
            {
                _refreshrate = value;
            }
        }

        [CategoryAttribute("Overlay Settings"),
        DescriptionAttribute("Enter your own Nickname!")]
        public string Nickname
        {
            get
            {
                return _nickname;
            }
            set
            {
                _nickname = value;
            }
        }


        // Name property with category attribute and
        // description attribute added
        [CategoryAttribute("Overlay Settings"),
        DescriptionAttribute("Enter the Opacity")]
        public double Opacity
        {
            get { return _Oopacity; }
            set { _Oopacity = value; }
        }



        [CategoryAttribute("Location Panel"),
        DescriptionAttribute("Setup your Location for the Worker Panel")]
        public Point Location_Worker
        {
            get
            {
                return _workers;
            }
            set
            {
                _workers = value;
            }
        }


        [CategoryAttribute("Location Panel"),
        DescriptionAttribute("Setup your Location for the Income Panel")]
        public Point Location_Income
        {
            get
            {
                return _income;
            }
            set
            {
                _income = value;
            }
        }


        [CategoryAttribute("Location Panel"),
        DescriptionAttribute("Setup your Location for the Ressource Panel")]
        public Point Location_Ressource
        {
            get
            {
                return _ressource;
            }
            set
            {
                _ressource = value;
            }
        }

        [CategoryAttribute("Location Panel"),
        DescriptionAttribute("Setup your Location for the Information Panel")]
        public Point Location_Information
        {
            get
            {
                return _informations;
            }
            set
            {
                _informations = value;
            }
        }

        [CategoryAttribute("Location Panel"),
        DescriptionAttribute("Setup your Location for the Maphack Panel")]
        public Point Location_Maphack
        {
            get
            {
                return _maphack;
            }
            set
            {
                _maphack = value;
            }
        }

        [CategoryAttribute("Location Panel"),
        DescriptionAttribute("Setup your Location for the Production Panel")]
        public Point Location_Production
        {
            get { return _production; }
            set { _production = value; }
        }

        [CategoryAttribute("Location Panel"),
        DescriptionAttribute("Setup your Location for the APM Panel")]
        public Point Location_APM
        {
            get { return _apm; }
            set { _apm = value; }
        }

        [CategoryAttribute("Location Panel"),
        DescriptionAttribute("Setup your Location for the Army Panel")]
        public Point Location_Army
        {
            get { return _army; }
            set { _army = value; }
        }

        [CategoryAttribute("Location Panel"),
        DescriptionAttribute("Setup your Location for the Notification Panel")]
        public Point Location_Notification
        {
            get { return _notification; }
            set { _notification = value; }
        }

        [CategoryAttribute("Overlay Keys"),
        DescriptionAttribute("Setup your Key for Ressource- Panel")]
        public int Key_Ressource
        {
            get { return _Ressource; }
            set { _Ressource = value; }
        }

        [CategoryAttribute("Overlay Keys"),
        DescriptionAttribute("Setup your Key for Income- Panel")]
        public int Key_Income
        {
            get { return _Income; }
            set { _Income = value; }
        }

        [CategoryAttribute("Overlay Keys"),
        DescriptionAttribute("Setup your Key for Worker- Panel")]
        public int Key_Worker
        {
            get { return _Worker; }
            set { _Worker = value; }
        }

        [CategoryAttribute("Overlay Keys"),
        DescriptionAttribute("Setup your Key for States- Panel")]
        public int Key_States
        {
            get { return _States; }
            set { _States = value; }
        }

        [CategoryAttribute("Overlay Keys"),
        DescriptionAttribute("Setup your Key for Maphack- Panel")]
        public int Key_Maphack
        {
            get { return _Maphack; }
            set { _Maphack = value; }
        }

        [CategoryAttribute("Overlay Keys"),
        DescriptionAttribute("Setup your Key for Production- Panel")]
        public int Key_Production
        {
            get { return _Production; }
            set { _Production = value; }
        }

        [CategoryAttribute("Overlay Keys"),
        DescriptionAttribute("Setup your Key for Apm- Panel")]
        public int Key_Apm
        {
            get { return _Apm; }
            set { _Apm = value; }
        }

        [CategoryAttribute("Overlay Keys"),
        DescriptionAttribute("Setup your Key for Settings- Form")]
        public int Key_Settings
        {
            get { return _Settings; }
            set { _Settings = value; }
        }

        [CategoryAttribute("Overlay Keys"),
        DescriptionAttribute("Setup your Key for Army- Panel")]
        public int Key_Army
        {
            get { return _Army; }
            set { _Army = value; }
        }

        [CategoryAttribute("Overlay Keys"),
        DescriptionAttribute("Setup your Key for Autoinject")]
        public int Key_Inject
        {
            get { return _inject; }
            set { _inject = value; }
        }

        [CategoryAttribute("Automation"),
        DescriptionAttribute("Set up if you'd like to automatically inject into your Hatchery")]
        public bool Autoinject
        {
            get { return _autoinject; }
            set { _autoinject = value; }
        }
    }
}