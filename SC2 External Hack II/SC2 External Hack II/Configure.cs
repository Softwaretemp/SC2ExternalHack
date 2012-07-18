using System;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Xml;

namespace SC2_External_Hack_II
{
    public partial class Configure : Form
    {
        readonly Form1 _from = null;
        public Configure(Form1 frm)
        {
            InitializeComponent();
            _from = frm;
        }

        private bool _bRessource,
                     _bIncome,
                     _bWorker,
                     _bStates,
                     _bMaphack,
                     _bProduction,
                     _bApm,
                     _bArmy,
                     _bNoti;

        public readonly Appsettings _asEinstellungen = new Appsettings();
        private void BtnSaveClick(object sender, EventArgs e)
        {
            Close();
        }

        //Catch points
        private Point CatchPoint(string definition)
        {
            //Line 5, 6, 7, 8
            //{X=25,Y=25}
            //Read the textfile- informations

            string strresult = definition;

            string[] strsplit = strresult.Split('=');
            string[] strsplit2 = strsplit[1].Split(',');
            string[] strsplit3 = strsplit[2].Split('}');

            string a = strsplit2[0];
            string b = strsplit3[0];

            Point ptr = new Point(int.Parse(a), int.Parse(b));

            return ptr;
        }

        //Close form and call WriteSettings
        private void ConfigureFormClosing(object sender, FormClosingEventArgs e)
        {
            XmlWrite();

            Process.Start("SC2 External Hack_0.10.6.1.exe");
           
            Environment.Exit(1);
        }

        //Load settings
        private void ConfigureLoad(object sender, EventArgs e)
        {
            if (!File.Exists("Settings.xml"))
            {
                _asEinstellungen.Refreshrate = 1;
                _asEinstellungen.Nickname = "TestNick";
                _asEinstellungen.Location_Ressource = new Point(1297, 46);
                _asEinstellungen.Location_Income = new Point(1297, 294);
                _asEinstellungen.Location_Worker = new Point(1300, 815);
                _asEinstellungen.Location_Information = new Point(10, 501);
                _asEinstellungen.Location_Maphack = new Point(27, 805);
                _asEinstellungen.Location_Production = new Point(15, 15);
                _asEinstellungen.Location_APM = new Point(3, 2);
                _asEinstellungen.Location_Army = new Point(1297, 528);
                _bStates = true;

                _asEinstellungen.Key_Ressource = 0x61;
                _asEinstellungen.Key_Income = 0x62;
                _asEinstellungen.Key_Worker = 0x63;
                _asEinstellungen.Key_States = 0x64;
                _asEinstellungen.Key_Maphack = 0x65;
                _asEinstellungen.Key_Production = 0x66;
                _asEinstellungen.Key_Apm = 0x67;
                _asEinstellungen.Key_Army = 0x68;
                _asEinstellungen.Key_Inject = 0x69;
                _asEinstellungen.Key_Settings = 0xA5;

                _asEinstellungen.Opacity = 0.8;

                _asEinstellungen.Autoinject = false;
                _asEinstellungen.Location_Notification = new Point(781,814);
            }

            else
                XmlRead();
            

            pgGrid.SelectedObject = _asEinstellungen;

            _from.Hide();
            _from.TopMost = false;
            _from.Ignore = true;
            _from.ToggleTimer = false;

            TopMost = true;
        }

        private void ConfigureFormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        //Call adjustment
        private void BtnAdjustClick(object sender, EventArgs e)
        {
            if (cmbPanel.SelectedIndex < 0) 
                return;

            var aps = new AdjustPanelPosition(this, cmbPanel.SelectedItem.ToString());
            aps.ShowDialog();

        }

        //Save settings in .XML File
        private void XmlWrite()
        {
            var myWrite = new XmlTextWriter("Settings.xml", Encoding.UTF8) { Formatting = Formatting.Indented };

            myWrite.WriteStartDocument(false);

            myWrite.WriteStartElement("SC2ExternalHack");

            myWrite.WriteElementString("TimerRefreshRate", _asEinstellungen.Refreshrate.ToString(CultureInfo.InvariantCulture));
            myWrite.WriteElementString("PlayerNickName", _asEinstellungen.Nickname);

            //Save opacity
            var str = new string[2];
            if (_asEinstellungen.Opacity != 1)
            {
                str = _asEinstellungen.Opacity.ToString(CultureInfo.InvariantCulture).Split('.');

                myWrite.WriteElementString("Opacity", str[0] + "," + str[1]);
            }
            else
                myWrite.WriteElementString("Opacity", _asEinstellungen.Opacity.ToString(CultureInfo.InvariantCulture));

            myWrite.WriteElementString("RessourcePanelPos", _asEinstellungen.Location_Ressource.ToString());
            myWrite.WriteElementString("IncomePanelPos", _asEinstellungen.Location_Income.ToString());
            myWrite.WriteElementString("WorkerPanelPos", _asEinstellungen.Location_Worker.ToString());
            myWrite.WriteElementString("InformationPanelPos", _asEinstellungen.Location_Information.ToString());
            myWrite.WriteElementString("MaphackPanelPos", _asEinstellungen.Location_Maphack.ToString());
            myWrite.WriteElementString("ProductionPanelPos", _asEinstellungen.Location_Production.ToString());
            myWrite.WriteElementString("ApmPanelPos", _asEinstellungen.Location_APM.ToString());
            myWrite.WriteElementString("ArmyPanelPos", _asEinstellungen.Location_Army.ToString());
            myWrite.WriteElementString("NotificationPanelPos", _asEinstellungen.Location_Notification.ToString());

            myWrite.WriteElementString("RessourcePanelKey", "0x" + Convert.ToString(_asEinstellungen.Key_Ressource, 16));
            myWrite.WriteElementString("IncomePanelKey", "0x" + Convert.ToString(_asEinstellungen.Key_Income, 16));
            myWrite.WriteElementString("WorkerPanelKey", "0x" + Convert.ToString(_asEinstellungen.Key_Worker, 16));
            myWrite.WriteElementString("InformationPanelKey", "0x" + Convert.ToString(_asEinstellungen.Key_States, 16));
            myWrite.WriteElementString("MaphackPanelKey", "0x" + Convert.ToString(_asEinstellungen.Key_Maphack, 16));
            myWrite.WriteElementString("ProductionPanelKey", "0x" + Convert.ToString(_asEinstellungen.Key_Production, 16));
            myWrite.WriteElementString("ApmPanelKey", "0x" + Convert.ToString(_asEinstellungen.Key_Apm, 16));
            myWrite.WriteElementString("ArmyPanelKey", "0x" + Convert.ToString(_asEinstellungen.Key_Army, 16));
            myWrite.WriteElementString("SettingsFormKey", "0x" + Convert.ToString(_asEinstellungen.Key_Settings, 16));

            myWrite.WriteElementString("RessourcePanelOnline", _bRessource.ToString(CultureInfo.InvariantCulture));
            myWrite.WriteElementString("IncomePanelOnline", _bIncome.ToString(CultureInfo.InvariantCulture));
            myWrite.WriteElementString("WorkerPanelOnline", _bWorker.ToString(CultureInfo.InvariantCulture));
            myWrite.WriteElementString("InformationPanelOnline", _bStates.ToString(CultureInfo.InvariantCulture));
            myWrite.WriteElementString("MaphackPanelOnline", _bMaphack.ToString(CultureInfo.InvariantCulture));
            myWrite.WriteElementString("ProductionPanelOnline", _bProduction.ToString(CultureInfo.InvariantCulture));
            myWrite.WriteElementString("ApmPanelOnline", _bApm.ToString(CultureInfo.InvariantCulture));
            myWrite.WriteElementString("ArmyPanelOnline", _bArmy.ToString(CultureInfo.InvariantCulture));
            myWrite.WriteElementString("NotificationPanelOnline", _bNoti.ToString(CultureInfo.InvariantCulture));



            myWrite.WriteEndElement();



            myWrite.Flush();
            myWrite.Close();
        }

        //Read settings from .XML File
        private void XmlRead()
        {
            XmlTextReader xReader = null;

            try
            {
                xReader = new XmlTextReader("Settings.xml");
                xReader.MoveToContent();
                var strElement = "";
                if ((xReader.NodeType == XmlNodeType.Element) && (xReader.Name == "SC2ExternalHack"))
                {
                    while (xReader.Read())
                    {
                        if (xReader.NodeType == XmlNodeType.Element)
                        {
                            strElement = xReader.Name;
                        }

                        else
                        {
                            if ((xReader.NodeType == XmlNodeType.Text) && (xReader.HasValue))
                            {
                                switch (strElement)
                                {
                                    case "TimerRefreshRate":
                                        _asEinstellungen.Refreshrate = int.Parse(xReader.Value);
                                        break;

                                    case "PlayerNickName":
                                        _asEinstellungen.Nickname = xReader.Value;
                                        break;

                                    case "Opacity":
                                        _asEinstellungen.Opacity = Convert.ToDouble(xReader.Value);
                                        break;


                                    case "RessourcePanelPos":
                                        _asEinstellungen.Location_Ressource = CatchPoint(xReader.Value);
                                        break;

                                    case "IncomePanelPos":
                                        _asEinstellungen.Location_Income = CatchPoint(xReader.Value);
                                        break;

                                    case "WorkerPanelPos":
                                        _asEinstellungen.Location_Worker = CatchPoint(xReader.Value);
                                        break;

                                    case "InformationPanelPos":
                                        _asEinstellungen.Location_Information = CatchPoint(xReader.Value);
                                        break;

                                    case "MaphackPanelPos":
                                        _asEinstellungen.Location_Maphack = CatchPoint(xReader.Value);
                                        break;

                                    case "ProductionPanelPos":
                                        _asEinstellungen.Location_Production = CatchPoint(xReader.Value);
                                        break;

                                    case "ApmPanelPos":
                                        _asEinstellungen.Location_APM = CatchPoint(xReader.Value);
                                        break;

                                    case "ArmyPanelPos":
                                        _asEinstellungen.Location_Army = CatchPoint(xReader.Value);
                                        break;

                                    case "NotificationPanelPos":
                                        _asEinstellungen.Location_Notification = CatchPoint(xReader.Value);
                                        break;


                                    case "RessourcePanelKey":
                                        _asEinstellungen.Key_Ressource = Convert.ToInt32(xReader.Value, 16);
                                        break;

                                    case "IncomePanelKey":
                                        _asEinstellungen.Key_Income = Convert.ToInt32(xReader.Value, 16);
                                        break;

                                    case "WorkerPanelKey":
                                        _asEinstellungen.Key_Worker = Convert.ToInt32(xReader.Value, 16);
                                        break;

                                    case "InformationPanelKey":
                                        _asEinstellungen.Key_States = Convert.ToInt32(xReader.Value, 16);
                                        break;

                                    case "MaphackPanelKey":
                                        _asEinstellungen.Key_Maphack = Convert.ToInt32(xReader.Value, 16);
                                        break;

                                    case "ProductionPanelKey":
                                        _asEinstellungen.Key_Production = Convert.ToInt32(xReader.Value, 16);
                                        break;

                                    case "ApmPanelKey":
                                        _asEinstellungen.Key_Apm = Convert.ToInt32(xReader.Value, 16);
                                        break;

                                    case "ArmyPanelKey":
                                        _asEinstellungen.Key_Army = Convert.ToInt32(xReader.Value, 16);
                                        break;

                                    case "SettingsFormKey":
                                        _asEinstellungen.Key_Settings = Convert.ToInt32(xReader.Value, 16);
                                        break;


                                    case "RessourcePanelOnline":
                                        _bRessource = Convert.ToBoolean(xReader.Value);
                                        break;

                                    case "IcomePanelOnline":
                                        _bIncome = Convert.ToBoolean(xReader.Value);
                                        break;

                                    case "WorkerPanelOnline":
                                        _bWorker = Convert.ToBoolean(xReader.Value);
                                        break;

                                    case "InformationPanelOnline":
                                        _bStates = Convert.ToBoolean(xReader.Value);
                                        break;

                                    case "MaphackPanelOnline":
                                        _bMaphack = Convert.ToBoolean(xReader.Value);
                                        break;

                                    case "ProductionPanelOnline":
                                        _bProduction = Convert.ToBoolean(xReader.Value);
                                        break;

                                    case "ApmPanelOnline":
                                        _bApm = Convert.ToBoolean(xReader.Value);
                                        break;

                                    case "ArmyPanelOnline":
                                        _bArmy = Convert.ToBoolean(xReader.Value);
                                        break;

                                    case "NotificationPanelOnline":
                                        _bNoti = Convert.ToBoolean(xReader.Value);
                                        break;
                                }
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            finally
            {
                if (xReader != null)
                    xReader.Close();
            }
        }


    }
}
