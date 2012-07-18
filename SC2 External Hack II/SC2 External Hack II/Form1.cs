// Starcraft 2 External Overlay Hack II
// Version 0.10
// Written by bellaPatricia
// Last edit.: July-03-2012
// 
// Thanks to:
// Beaving
// MrNukealizer
// People who download this and give suggestions
// 
// Things that work with this version:
// #############################################
// 1.) View Ressources (Minerals, Gas, Supply)
// 2.) View Income (Minerals, Gas, Workers)
// 3.) View your workeramount (to get better and know when you have enough)
// 4.) View Units (what Units and how many) <--- Disabled due to I DUN LIKE YET
// 5.) View APM (To know how fast you are)
// 6.) View Armysize (So you know if your Army is better)
// 7.) View Minimap (You can see enemy Units and Camera position)
// 8.) Autoinject <--- With trouble D:
// 
// Things that are planned:
// ##############################################
//
// 1.) Autogroup Creep Tumors
// 2.) Detect Unitstance of a unit (to color on the minimap)
// 3.) Detect units in Nexus and CC if there are more than 1/ 2 make a notification
//
// Things that were planned but cancelled
// ##############################################
// 1.) Display all units on the actual map so you have somne kind of real maphack
//     but without the possibility to get caugh by Blizzard
//     REASON: Too much to calculate + Nonsense for me because
//             you already have the Minimap- hack.
// 
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Xml;
using bellaPatricia_Lib;

namespace SC2_External_Hack_II
{
    public partial class Form1 : Form
    {
        //Classes
        //++++++++++++++++++++
        private PointerScan _pcPointer;
        private readonly ProcessMemoryReader _pReader = new ProcessMemoryReader(Process.GetProcessesByName("SC2")[0]);
        private ProcessView _pvView = new ProcessView("SC2");
        private readonly MapInfo _mInfor = new MapInfo();


        #region Chat- Offsets

        private const int Chat = 0x16F4C28,
            Offset1Chat = 0x14,
            Offset2Chat = 0x4,
            Offset3Chat = 0x4,
            Offset4Chat = 0x208,
            Offset5Chat = 0x3E8;

        #endregion

        #region Standart Variables

        Image _imgIconsMinerals,    //Image-variables for the Minerals ect icons for each race
                      _imgIconsGas,
                      _imgIconsSupply,
                      _imgIconsWorker;
        string _strresult;               //Define Resultstring for the Colorcatch
        string _strBackup = "";          //This Variable backs up the input by the user
        bool _bchanged;          //Here we proof if something already happened (input)


        private bool _bdone1,
                     //Variables for Panelposition
                     _bdone2,
                     _bdone3,
                     _bdone4,
                     _bdone5,
                     _bdone6,
                     _bdone7,
                     _bdone8,
                     _bdone9;
        public bool Ignore;     //Variable to proof the call of the Settings form
        private bool _bNotification = true; //Variable to handle the Notification, handle as WinProc
        private bool _Autoinject = false;
        private bool _OkayToInject = false;


        public string _strNickname = "";


        //Keyvariables
        private int _uRessource = 0,
                    _uIncome = 0,
                    _uWorker = 0,
                    _uStates = 0,
                    _uMaphack = 0,
                    _uProduction = 0,
                    _uApm = 0,
                    _uSettings = 0,
                    _uArmy = 0,
                    _uInject = 0;


        private IntPtr iWindow;



        private PlayerInfo _pInfor;
        private List<PlayerInfo> _pInfoPlayer;
        private UnitInfo _uInfor;
        private List<UnitInfo> _uInfoUnit; 
        private Thread _thr;
        private Thread _thrInput;
        private Thread _thrUpdate;
        #endregion

        #region DLL-Imports

        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vkey);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);


        #endregion

        #region Load images and save them

        private readonly Image _imgScv = Properties.Resources.twbfg,
                      _imgMarine = Properties.Resources.tspacemarine,
                      _imgMarauder = Properties.Resources.tmarodeur,
                      _imgGhost = Properties.Resources.tghost,
                      _imgReaper = Properties.Resources.treaperx,
                      _imgHellion = Properties.Resources.thellion,
                      _imgSiegeTank = Properties.Resources.tpanzer,
                      _imgThor = Properties.Resources.tthor,
                      _imgViking = Properties.Resources.tviking,
                      _imgBanshee = Properties.Resources.tbunshee,
                      _imgMedivac = Properties.Resources.tmedivac,
                      _imgRaven = Properties.Resources.traven,
                      _imgBattlecruiser = Properties.Resources.tkreuzer,
                      _imgMule = Properties.Resources.tmule,
                      _imgProbe = Properties.Resources.P_Probe,
                      _imgZealot = Properties.Resources.P_Zealot,
                      _imgStalker = Properties.Resources.P_Stalker,
                      _imgSentry = Properties.Resources.P_Sentry,
                      _imgDarkTemplar = Properties.Resources.P_DarkTemplar,
                      _imgHighTemplar = Properties.Resources.P_HighTemplar,
                      _imgArchon = Properties.Resources.P_Archon,
                      _imgImmortal = Properties.Resources.P_Immortal,
                      _imgColossus = Properties.Resources.P_Colossus,
                      _imgOberver = Properties.Resources.P_Observer,
                      _imgWarpPrism = Properties.Resources.P_WarpPrism,
                      _imgPhoenix = Properties.Resources.P_Phoenix,
                      _imgVoidRay = Properties.Resources.P_VoidRay,
                      _imgCarrier = Properties.Resources.P_Carrier,
                      _imgMothership = Properties.Resources.P_MotherShip,
                      _imgZergling = Properties.Resources.z_zergling,
                      _imgLarva = Properties.Resources.z_larve_queen,
                      _imgBaneling = Properties.Resources.z_berstling,
                      _imgRoach = Properties.Resources.z_schabe,
                      _imgHydra = Properties.Resources.z_hydralisk,
                      _imgMutalisk = Properties.Resources.z_mutalisk,
                      _imgCorruptor = Properties.Resources.z_schaender,
                      _imgUltra = Properties.Resources.z_ultralisk,
                      _imgQueen = Properties.Resources.z_queen,
                      _imgBroodlord = Properties.Resources.z_brutlord,
                      _imgDrone = Properties.Resources.z_drohne,
                      _imgInfestor = Properties.Resources.z_verseucher,
                      _imgOverlord = Properties.Resources.z_overlord,
                      _imgOverseer = Properties.Resources.z_overseer;


        
        #endregion

        //Look for updates
        private void UpdateAvailable()
        {
            //http://www.youtube.com/watch?v=eS688uFAKPA
            Version newVersion = null;
            const string strXmlUrl = "https://dl.dropbox.com/u/62845853/SC2ExternalHackFileVersion.xml";
            XmlTextReader xReader = null;

            try
            {
                xReader = new XmlTextReader(strXmlUrl);
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
                                    case "version":
                                        newVersion = new Version(xReader.Value);
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

            //This version
            var verThiosVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

            if (verThiosVersion >= newVersion)
            {
                _thrUpdate.Abort();
                return;
            }

            const MessageBoxButtons msgButton = MessageBoxButtons.YesNo;

            //Dialogresult call
            var rs =
                MessageBox.Show("Version " + newVersion.Major + "." + newVersion.Minor + "." + newVersion.Build + "." + newVersion.MinorRevision +
                                " of SC2 External OverlayHack is available!\n\nDo you wish to download it?",
                                "New Version available", msgButton);

            //Catch the dialogresult
            if (rs != DialogResult.Yes)
            {
                _thrUpdate.Abort();
                return;
            }

            //Downloadlink to newest Version
            var downloadUrl = "https://dl.dropbox.com/u/62845853/SC2%20External%20Hack_" + newVersion.Major + "." +
                              newVersion.Minor + "." + newVersion.Build + "." + newVersion.MinorRevision + ".rar";
            //Start download
            Process.Start(downloadUrl);

            //Close Application
            Environment.Exit(1);
        }

        public Form1()
        {
            InitializeComponent();
        }

        //Make Param for the mouse-location
        public int MakeLParam(Point ptr)
        {
            return ((ptr.Y << 16) | (ptr.X & 0xffff));
        }

        //Make the form unclickable
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x20;
                return cp;
            }
        }

        //Mainform, load everything when the Application starts
        private void Form1Load(object sender, EventArgs e)
        {
            iWindow = FindWindow(null, "StarCraft II");

            //Start another thread to exclude lag
            _thr = new Thread(ThreadFunction);
            _thr.Start();

            //Start thread to catch input
            _thrInput = new Thread(InputThread);
            _thrInput.Start();

            //Update thread
            //_thrUpdate = new Thread(UpdateAvailable);
            //_thrUpdate.Start();

            //Calls Exception when SC2 is not found
            CallException();

            //Standart Setup for the form, size, location and so on!
            Form1Setup();

            CheckForIllegalCrossThreadCalls = false;
        }

        //Method for the Mainform, better to have it splitted!
        private void Form1Setup()
        {
            //Call Config-form
            if (!File.Exists("Settings.xml"))
            {
                var cfg = new Configure(this);
                cfg.ShowDialog();
            }

            tmrTick.Enabled = true;

            FormBorderStyle = FormBorderStyle.None;

            Width = Screen.PrimaryScreen.Bounds.Width;
            Height = Screen.PrimaryScreen.Bounds.Height;

            Location = new Point(0, 0);

            //Read stuff from xmlfile
            XmlRead();

            pnlIncome.Enabled = pnlIncome.Visible;
            pnlRessources.Enabled = pnlRessources.Visible;
            pnlWorkers.Enabled = pnlWorkers.Visible;
            pnlStates.Enabled = pnlStates.Visible;
            pnlMaphack.Enabled = pnlMaphack.Visible;
            pnlProduction.Enabled = pnlProduction.Visible;
            pnlAPM.Enabled = pnlAPM.Visible;
            pnlArmy.Enabled = pnlArmy.Visible;

        }

        //Write the settings when closing
        private void WriteSettings()
        {
            if (Ignore) return;


            //Write textfile- informations
            if (File.Exists("Settings.xml"))
                File.Delete("Settings.xml");

            XmlWrite();
        }

        //Method to give out the information we need!
        /// <summary>
        /// 'Heart'- method , grabs all the values we want
        /// </summary>
        /// <param name="ioffset">The address where our value is</param>
        /// <param name="iLenght">The lenght of that value</param>
        /// <returns></returns>
        private byte[] DoHandle(int ioffset, uint iLenght)
        {
            //if (_pvView.ProcessAviable())
            //{
                Process[] psc2 = Process.GetProcessesByName("SC2");

                _pReader.ReadProcess = psc2[0];

                _pReader.OpenProcess();


                int bytesread;

                var buffer = _pReader.ReadProcessMemory((IntPtr)ioffset, iLenght, out bytesread);
                _pReader.CloseHandle();

                return buffer;

            //}
            //return null;
        }

        //Method to Write to memory
        private void WriteMemory(int iaddress, byte[] lWantedValue)
        {
            try
            {
                Process[] myProcesses = Process.GetProcessesByName("SC2");
                _pReader.ReadProcess = myProcesses[0];
            }

            catch { new Exception(); }


            /*Open pr to read process*/
            int byteswritten;

            _pReader.OpenProcess();

            //Initialisierte Gewünschten Wert und Wandle ihn in Byte Array um

         //   memoryvalue = BitConverter.GetBytes(lWantedValue);

            _pReader.WriteProcessMemory((IntPtr)iaddress, lWantedValue, out byteswritten);

            _pReader.CloseHandle();
        }

        //This paintmethod will draw text on the PanelRessources
        private void PnlRessourcesPaint(object sender, PaintEventArgs e)
        {
            if (!pnlRessources.Enabled) return;
            if (!_pvView.ProcessInForeground()) return;
            if (!_mInfor.InGame) return;


            const int ivalue = 70;
            for (var i = 0; i < _pInfor.PlayerAmount; i++)
            {
                try
                {
                    //Stop drawing when there is no Maxfood anymore (e.g. player death)
                    if (_pInfoPlayer[i].FoodMax == "0")
                        continue;


                    //Create colors for the Background
                    var clBackground = i%2 == 1 ? Color.Gray : Color.DimGray;

                    #region Draw Lines

                    //Here we check if the content is not our Nickname and watch out, if 
                    //it has already called
                    //e.g. if our Nickname was called 'behind' us.
                    //If so, we change the positions only (because the values are okay)

                    if (Opacity != 1)
                    {
                        //Draw Background
                        e.Graphics.FillRectangle(new SolidBrush(clBackground), 1, 67 + 36*i, pnlRessources.Width - 2,
                                                 33);

                        //Draw scare around Ressources
                        e.Graphics.DrawRectangle(new Pen(new SolidBrush(_pInfoPlayer[i].PlayerColor), 2), 0,
                                                 66 + 36*i, pnlRessources.Width - 1, 34);
                    }


                    //Nickname of player
                    e.Graphics.DrawString(_pInfoPlayer[i].PlayerName,
                                          new Font("Century Gothic", 14.25f, FontStyle.Bold),
                                          new SolidBrush(_pInfoPlayer[i].PlayerColor),
                                          new Point(0, 70 + 36*i));

                    //Team of players
                    e.Graphics.DrawString("#" + _pInfoPlayer[i].PlayerTeam,
                                          new Font("Century Gothic", 14.25f, FontStyle.Regular),
                                          new SolidBrush(Color.White),
                                          new Point(152, 70 + 36*i));

                    //Minerals of Player
                    e.Graphics.DrawString(_pInfoPlayer[i].MineralsCurrent,
                                          new Font("Century Gothic", 14.25f, FontStyle.Regular),
                                          new SolidBrush(Color.White),
                                          new Point(222, 70 + 36*i));

                    //Gas of Player
                    e.Graphics.DrawString(_pInfoPlayer[i].GasCurrent,
                                          new Font("Century Gothic", 14.25f, FontStyle.Regular),
                                          new SolidBrush(Color.White),
                                          new Point(344, 70 + 36*i));

                    //Supply of Player
                    e.Graphics.DrawString(_pInfoPlayer[i].Food + "/" +
                                          _pInfoPlayer[i].FoodMax,
                                          new Font("Century Gothic", 14.25f, FontStyle.Regular),
                                          new SolidBrush(Color.White),
                                          new Point(467, 70 + 36*i));


                    #endregion

                    #region Draw Pictures

                    //Reset values to avoid stack 
                    // => result is, that after a game,
                    //the Icosn won't change
                    _imgIconsMinerals = null;
                    _imgIconsGas = null;
                    _imgIconsSupply = null;
                    _imgIconsWorker = null;

                    #region Icon/ Race detection

                    _strresult = _pInfoPlayer[i].Race;
                    if (_strresult.Contains("Terr"))
                    {
                        _imgIconsMinerals = Properties.Resources.Mineral_Terran;
                        _imgIconsGas = Properties.Resources.Gas_Terran;
                        _imgIconsSupply = Properties.Resources.Supply_Terran;
                        _imgIconsWorker = Properties.Resources.T_SCV;
                    }

                    if (_strresult.Contains("Zerg"))
                    {
                        _imgIconsMinerals = Properties.Resources.Mineral_Zerg;
                        _imgIconsGas = Properties.Resources.Gas_Zerg;
                        _imgIconsSupply = Properties.Resources.Supply_Zerg;
                        _imgIconsWorker = Properties.Resources.Z_Drone;
                    }

                    if (_strresult.Contains("Prot"))
                    {
                        _imgIconsMinerals = Properties.Resources.Mineral_Protoss;
                        _imgIconsGas = Properties.Resources.Gas_Protoss;
                        _imgIconsSupply = Properties.Resources.Supply_Protoss;
                        _imgIconsWorker = Properties.Resources.P_Probe;
                    }

                    #endregion

                    //Mineral Drawing
                    e.Graphics.DrawImage(_imgIconsMinerals, new Point(191, ivalue + 36*i));

                    //Gas Drawing
                    e.Graphics.DrawImage(_imgIconsGas, new Point(314, ivalue + 36*i));

                    //Supply Drawing
                    e.Graphics.DrawImage(_imgIconsSupply, new Point(437, ivalue + 36*i));


                    #endregion

                }
                catch
                {
                    new Exception();
                }

            }
        }

        //In here, we paint the worker- information on PanelWorkers
        private void PnlWorkersPaint(object sender, PaintEventArgs e)
        {
            if (!pnlWorkers.Enabled) return;

            if (!_pvView.ProcessInForeground()) return;
            Color clBackground;
            
            if (_mInfor.InGame)
                clBackground = Color.Gray;

            else
                clBackground = Color.FromArgb(255, 255, 192, 192);
            
            
            //clBackground = Color.DimGray;

            try
            {
                if (Opacity != 1)
                {
                    //Draw Background
                    e.Graphics.FillRectangle(new SolidBrush(clBackground), 1, 1, pnlWorkers.Width - 2,
                                             pnlWorkers.Height - 2);

                    //Draw scare around Ressources
                    e.Graphics.DrawRectangle(new Pen(new SolidBrush(_pInfoPlayer[_pInfoPlayer[0].LocalPlayer - 1].PlayerColor), 2), 1,
                                             1, pnlWorkers.Width - 2, pnlWorkers.Height - 2);
                }


                //Draw scv amount for the Player
                e.Graphics.DrawString("»" + _pInfoPlayer[_pInfoPlayer[0].LocalPlayer - 1].WorkerAmount + "« Workers",
                                      new Font("Century Gothic", 16f, FontStyle.Bold),
                                      new SolidBrush(_pInfoPlayer[_pInfoPlayer[0].LocalPlayer - 1].PlayerColor),
                                      new Point(10, 10));
            }

            catch
            {
                new Exception();
            }

        }

        //Display of the current Income by the players
        private void PnlIncomePaint(object sender, PaintEventArgs e)
        {
            if (!pnlIncome.Enabled) return;
            if (!_pvView.ProcessInForeground()) return;
            if (!_mInfor.InGame) return;

            label1.ForeColor = _mInfor.InGame ? Color.Gray : Color.FromArgb(255, 255, 192, 192);

            const int ivalue = 70;
            for (var i = 0; i < _pInfor.PlayerAmount; i++)
            {
                try
                {
                    //Stop drawing when there is no Maxfood anymore (e.g. player death)
                    if (_pInfoPlayer[i].FoodMax == "0" && _pInfoPlayer[i].Food == "0")
                        continue;

                    //Reset values to avoid stack 
                    // => result is, that after a game,
                    //the Icosn won't change
                    _imgIconsMinerals = null;
                    _imgIconsGas = null;
                    _imgIconsSupply = null;
                    _imgIconsWorker = null;

                    //Create colors for the Background
                    var clBackground = i % 2 == 1 ? Color.Gray : Color.DimGray;

                    #region Icon/ Race detection

                    _strresult = _pInfoPlayer[i].Race;

                    if (_strresult.Contains("Terr"))
                    {
                        _imgIconsMinerals = Properties.Resources.Mineral_Terran;
                        _imgIconsGas = Properties.Resources.Gas_Terran;
                        _imgIconsSupply = Properties.Resources.Supply_Terran;
                        _imgIconsWorker = Properties.Resources.T_SCV;
                    }

                    if (_strresult.Contains("Zerg"))
                    {
                        _imgIconsMinerals = Properties.Resources.Mineral_Zerg;
                        _imgIconsGas = Properties.Resources.Gas_Zerg;
                        _imgIconsSupply = Properties.Resources.Supply_Zerg;
                        _imgIconsWorker = Properties.Resources.Z_Drone;
                    }

                    if (_strresult.Contains("Prot"))
                    {
                        _imgIconsMinerals = Properties.Resources.Mineral_Protoss;
                        _imgIconsGas = Properties.Resources.Gas_Protoss;
                        _imgIconsSupply = Properties.Resources.Supply_Protoss;
                        _imgIconsWorker = Properties.Resources.P_Probe;
                    }

                    #endregion

                    #region Draw Lines

                    if (Opacity != 1)
                    {
                        //Draw Background
                        e.Graphics.FillRectangle(new SolidBrush(clBackground), 1, 67 + 36 * i, pnlIncome.Width - 2,
                                                 33);

                        //Draw scare around Ressources
                        e.Graphics.DrawRectangle(new Pen(new SolidBrush(_pInfoPlayer[i].PlayerColor), 2), 0,
                                                 66 + 36 * i, pnlRessources.Width - 1, 34);
                    }

                    //Nickname of players
                    e.Graphics.DrawString(_pInfoPlayer[i].PlayerName,
                                          new Font("Century Gothic", 14.25f, FontStyle.Bold),
                                          new SolidBrush(_pInfoPlayer[i].PlayerColor),
                                          new Point(0, 70 + 36*i));

                    //Team of players
                    e.Graphics.DrawString("#" + _pInfoPlayer[i].PlayerTeam,
                                          new Font("Century Gothic", 14.25f, FontStyle.Regular),
                                          new SolidBrush(Color.White),
                                          new Point(152, 70 + 36*i));

                    //Minerals of players
                    e.Graphics.DrawString(_pInfoPlayer[i].IncomeMinerals,
                                          new Font("Century Gothic", 14.25f, FontStyle.Regular),
                                          new SolidBrush(Color.White),
                                          new Point(222, 70 + 36*i));

                    //Gas of players
                    e.Graphics.DrawString(_pInfoPlayer[i].IncomeGas,
                                          new Font("Century Gothic", 14.25f, FontStyle.Regular),
                                          new SolidBrush(Color.White),
                                          new Point(344, 70 + 36*i));

                    //Harvesters of player 
                    e.Graphics.DrawString(_pInfoPlayer[i].WorkerAmount,
                                          new Font("Century Gothic", 14.25f, FontStyle.Regular),
                                          new SolidBrush(Color.White),
                                          new Point(467, 70 + 36*i));

                    #endregion

                    #region Draw Pictures

                    //Mineral Drawing
                    if (_imgIconsMinerals != null)
                        e.Graphics.DrawImage(_imgIconsMinerals, new Point(191, ivalue + 36*i));

                    //Gas Drawing
                    if (_imgIconsGas != null)
                        e.Graphics.DrawImage(_imgIconsGas, new Point(314, ivalue + 36*i));

                    //Worker drawing
                    if (_imgIconsWorker != null)
                        e.Graphics.DrawImageUnscaledAndClipped(_imgIconsWorker,
                                                               new Rectangle(new Point(437, ivalue + 36*i),
                                                                             new Size(29, 28)));

                    #endregion
                }

                catch { new Exception(); }
            }
        }

        //Draw the Minimap 
        private void PnlMaphackPaint(object sender, PaintEventArgs e)
        {
            /* This part was only possible 
             * to work properly with the help
             * of MrNukealizer and Qazzy */

            if (!pnlMaphack.Enabled) return;
            if (!_pvView.ProcessInForeground()) return;
            if (!_mInfor.InGame) return;


            #region Variables


            var iPlayableWidht = _mInfor.PlayAbleWidht;
            var iPlayableHeight = _mInfor.PlayAbleHeight;


            float iScale = 0,
                  iX = 0,
                  iY = 0;

            #endregion

            #region Get minimap Bounds

            if (pnlMaphack.Height/pnlMaphack.Width >= iPlayableHeight/iPlayableWidht)
            {
                iScale = (float) pnlMaphack.Width/(float) iPlayableWidht;
                iX = 0;
                iY = (pnlMaphack.Height - iScale*iPlayableHeight)/2;
            }
            else
            {
                iScale = (float) pnlMaphack.Height/(float) iPlayableHeight;
                iY = 0;
                iX = (pnlMaphack.Width - iScale*iPlayableWidht)/2;
            }

            #endregion

            #region Draw Minimap Bounds

            //Draw some recangle to mark the minimap
            e.Graphics.DrawRectangle(new Pen(new SolidBrush(Color.Red)), 0, 0, 261, 258);

            //Draw the rectangle to mark the playable minimap
            e.Graphics.DrawRectangle(new Pen(new SolidBrush(Color.LawnGreen)), iX, iY,
                                     (pnlMaphack.Width - iX*2 - 1),
                                     (pnlMaphack.Height - iY*2 - 1));

            #endregion


            try
            {
                //Get the Camera position and Draw a trapez
                for (var i = 0; i < _pInfor.PlayerAmount; i++)
                {
                    //Stop drawing when there is no Maxfood anymore (e.g. player death)
                    if (_pInfoPlayer[i].FoodMax == "0" && _pInfoPlayer[i].Food == "0")
                        continue;

                    if (_pInfoPlayer[0].LocalPlayer - 1 == i)
                        continue;

                    #region Catch the position

                    var cx = (_pInfoPlayer[i].CameraX - _mInfor.MapLeft)*iScale + iX;
                    var cy = (_mInfor.MapTop - _pInfoPlayer[i].CameraY)*iScale + iY;

                    //Points to draw a trapez
                    var ptPoints = new Point[4];
                    ptPoints[0] = new Point((int) cx - 35, (int) cy - 25);
                    ptPoints[1] = new Point((int) cx + 35, (int) cy - 25);
                    ptPoints[2] = new Point((int) cx + 25, (int) cy + 10);
                    ptPoints[3] = new Point((int) cx - 25, (int) cy + 10);

                    #endregion

                    #region Draw the trapez

                    //Draw the trapez
                    e.Graphics.DrawPolygon(new Pen(new SolidBrush(_pInfoPlayer[i].PlayerColor), 2), ptPoints);

                    #endregion
                }

                //Get the Unit positions and Draw them
                for (var i = 0; i < _uInfor.TotalUnits; i++)
                {
                    //Format the actual point
                    var fx = (_uInfoUnit[i].PositionX - _mInfor.MapLeft)*iScale + iX;
                    var fy = (_mInfor.MapTop - _uInfoUnit[i].PositionY)*iScale + iY;

                    #region Draw the Unit-points

                    //If the Unit is death, go to the next one
                    if (!UnitIsAlive(i))
                        continue;

                    //If the Localplayer (you) is the owner go to the next unit
                    if (_uInfoUnit[i].Owner == _pInfoPlayer[0].LocalPlayer)
                    {
                        #region Draw the Unitpositions as Notification

                        //Draw the Notofication
                        if (_uInfoUnit[i].UnitName.Contains("Orbital"))
                        {
                            if (_uInfoUnit[i].UnitEnergy >= 50)
                            {
                                e.Graphics.FillRectangle(new SolidBrush(Color.Orange), fx - 5.0f, fy - 5.0f, 10, 10);
                                e.Graphics.DrawRectangle(new Pen(Color.Black), fx - 5.0f, fy - 5.0f, 10, 10);
                            }
                        }

                        if (_uInfoUnit[i].UnitName.Contains("Nexus"))
                        {
                            if (_uInfoUnit[i].UnitEnergy >= 25)
                            {
                                e.Graphics.FillRectangle(new SolidBrush(Color.Orange), fx - 5.0f, fy - 5.0f, 10, 10);
                                e.Graphics.DrawRectangle(new Pen(Color.Black), fx - 5.0f, fy - 5.0f, 10, 10);
                            }
                        }

                        if (_uInfoUnit[i].UnitName.Contains("Queen"))
                        {
                            if (_uInfoUnit[i].UnitEnergy >= 25)
                            {
                                e.Graphics.FillRectangle(new SolidBrush(Color.Orange), fx - 2.5f, fy - 2.5f, 5, 5);
                                e.Graphics.DrawRectangle(new Pen(Color.Black), fx - 2.5f, fy - 2.5f, 5, 5);
                            }
                        }

                        #endregion

                        continue;
                    }


                    var iplayerow = _uInfoUnit[i].Owner;
                    var ik = 0;

                    var clUnit = Color.White;
                    var clUnit2 = Color.White;

                    //Size of the current unit.
                    var fsize = _uInfoUnit[i].Size;

                    if (iplayerow > 16)
                    {
                        clUnit2 = Color.Transparent;
                        clUnit = Color.Transparent;
                        ik = 0;
                    }

                    else if (iplayerow < 1)
                    {
                        ik = 0;
                        clUnit2 = Color.Transparent;
                        clUnit = Color.Transparent;
                    }

                    else
                    {
                        clUnit2 = Color.Black;
                        clUnit = _pInfoPlayer[(int) iplayerow - 1].PlayerColor;
                        if (_uInfoUnit[i].UnitName.Contains("Creep"))
                            clUnit = Color.Black;

                        ik = 1;
                    }



                    //Draw the point/Rectangle
                    e.Graphics.FillRectangle(new SolidBrush(clUnit), fx - 2, fy - 2, _uInfoUnit[i].Size*iScale,
                                             _uInfoUnit[i].Size*iScale);
                    e.Graphics.DrawRectangle(new Pen(clUnit2), fx - 2, fy - 2, _uInfoUnit[i].Size*iScale,
                                             _uInfoUnit[i].Size*iScale);

                    #endregion

                    #region Recolor defensive structures

                    /* Defensive structures are:
                     * Bunker
                     * Turrent
                     * Cannon
                     * Spore Crawler 
                     * Spine Crawler */

                    if (_uInfoUnit[i].Owner != _pInfoPlayer[0].LocalPlayer)
                    {

                        if (_uInfoUnit[i].UnitName.Contains("Bunker") ||
                            _uInfoUnit[i].UnitName.Contains("Turret") ||
                            _uInfoUnit[i].UnitName.Contains("Photon") ||
                            _uInfoUnit[i].UnitName.Contains("Spore") ||
                            _uInfoUnit[i].UnitName.Contains("Spine") ||
                            _uInfoUnit[i].UnitName.Contains("Planetary"))
                        {
                            e.Graphics.FillRectangle(new SolidBrush(Color.Yellow), fx - 2, fy - 2, 4, 4);
                            e.Graphics.DrawRectangle(new Pen(Color.Black), fx - 2, fy - 2, 4, 4);
                        }
                    }

                    #endregion

                    #region Draw the lines to destination Point

                    if (ik != 1) continue;

                    if (_uInfoUnit[i].DestinationX < 10)
                        continue;

                    //Format the actual point
                    var fdx = (_uInfoUnit[i].DestinationX - _mInfor.MapLeft)*iScale + iX;
                    var fdy = (_mInfor.MapTop - _uInfoUnit[i].DestinationY)*iScale + iY;

                    //Draw Line
                    e.Graphics.DrawLine(new Pen(new SolidBrush(Color.LawnGreen)), fx, fy, fdx, fdy);

                    #endregion


                }
            }
            catch
            {
                new Exception();
            }
        }

        //Draw states of other Panels
        private void PnlStatesPaint(object sender, PaintEventArgs e)
        {
            if (!pnlStates.Enabled) return;
            if (!_pvView.ProcessInForeground()) return;

            #region Panel Ressources

            e.Graphics.DrawString(
                pnlRessources.Visible ? "Panel 'Ressources': Active" : "Panel 'Ressources': Inactive",
                new Font("Century Gothic", 14.25f, FontStyle.Regular), new SolidBrush(Color.White),
                new Point(0, 20));

            #endregion

            #region Panel Income

            e.Graphics.DrawString(
                pnlIncome.Visible ? "Panel 'Income': Active" : "Panel 'Income': Inactive",
                new Font("Century Gothic", 14.25f, FontStyle.Regular), new SolidBrush(Color.White),
                new Point(0, 50));

            #endregion

            #region Panel Workers

            e.Graphics.DrawString(
                pnlWorkers.Visible ? "Panel 'Workers': Active" : "Panel 'Workers': Inactive",
                new Font("Century Gothic", 14.25f, FontStyle.Regular), new SolidBrush(Color.White),
                new Point(0, 80));

            #endregion

            #region Panel Maphack

            e.Graphics.DrawString(
                pnlMaphack.Visible ? "Panel 'Maphack': Active" : "Panel 'Maphack': Inactive",
                new Font("Century Gothic", 14.25f, FontStyle.Regular), new SolidBrush(Color.White),
                new Point(0, 110));

            #endregion

            #region Panel Unit 

            e.Graphics.DrawString(
                pnlProduction.Visible ? "Panel 'Production': Active" : "Panel 'Production': Inactive",
                new Font("Century Gothic", 14.25f, FontStyle.Regular), new SolidBrush(Color.White),
                new Point(0, 140));

            #endregion

            #region Panel APM

            e.Graphics.DrawString(
                pnlAPM.Visible ? "Panel 'APM': Active" : "Panel 'APM': Inactive",
                new Font("Century Gothic", 14.25f, FontStyle.Regular), new SolidBrush(Color.White),
                new Point(0, 170));

            #endregion

            #region Panel Army

            e.Graphics.DrawString(
                pnlArmy.Visible ? "Panel 'Army': Active" : "Panel 'Army': Inactive",
                new Font("Century Gothic", 14.25f, FontStyle.Regular), new SolidBrush(Color.White),
                new Point(0, 200));

            #endregion

            #region Autoinject

            e.Graphics.DrawString(
                _Autoinject ? "Autoinject: Active" : "Autoinject: Inactive",
                new Font("Century Gothic", 14.25f, FontStyle.Strikeout), new SolidBrush(Color.White),
                new Point(0, 230));

            #endregion
        }

        //Draw the current Units of the opponent
        private void PnlProductionPaint(object sender, PaintEventArgs e)
        {
            
        }

        //Draw the APM of all players
        private void PnlApmPaint(object sender, PaintEventArgs e)
        {
            if (!pnlAPM.Enabled) return;
            if (!_pvView.ProcessInForeground()) return;
            if (!_mInfor.InGame) return;

            label3.ForeColor = _mInfor.InGame ? Color.Gray : Color.FromArgb(255, 255, 192, 192);

            for (var i = 0; i < _pInfor.PlayerAmount; i++)
            {
                //Stop drawing when there is no Maxfood anymore (e.g. player death)
                if (_pInfoPlayer[i].FoodMax == "0" && _pInfoPlayer[i].Food == "0")
                    continue;

                //Create colors for the Background
                var clBackground = i % 2 == 1 ? Color.Gray : Color.DimGray;

                try
                {
                    var cPlayercolor = _pInfoPlayer[i].PlayerColor;


                    #region Draw Lines

                    if (Opacity != 1)
                    {
                        //Draw Background
                        e.Graphics.FillRectangle(new SolidBrush(clBackground), 1, 67 + 36 * i, pnlAPM.Width - 2,
                                                 33);

                        //Draw scare around Ressources
                        e.Graphics.DrawRectangle(new Pen(new SolidBrush(_pInfoPlayer[i].PlayerColor), 2), 0,
                                                 66 + 36 * i, pnlAPM.Width - 1, 34);
                    }

                    //Nickname of players
                    e.Graphics.DrawString(_pInfoPlayer[i].PlayerName,
                                          new Font("Century Gothic", 14.25f, FontStyle.Bold),
                                          new SolidBrush(cPlayercolor),
                                          new Point(0, 70 + 36*i));

                    //APM of players
                    e.Graphics.DrawString(_pInfoPlayer[i].ApmCurrent + '[' + _pInfoPlayer[i].RealApmCurrent + ']',
                                          new Font("Century Gothic", 14.25f, FontStyle.Regular),
                                          new SolidBrush(Color.White),
                                          new Point(152, 70 + 36*i));


                    #endregion

                }

                catch { new Exception(); }
            }
        }

        //Here we'll paint the Armysize
        private void PnlArmyPaint(object sender, PaintEventArgs e)
        {
            if (!pnlArmy.Enabled) return;

            if (!_pvView.ProcessInForeground()) return;

            label2.ForeColor = _mInfor.InGame ? Color.Gray : Color.FromArgb(255, 255, 192, 192);

            const int ivalue = 70;
            for (var i = 0; i < _pInfor.PlayerAmount; i++)
            {
                //Create colors for the Background
                var clBackground = i%2 == 1 ? Color.Gray : Color.DimGray;

                try
                {
                    //Stop drawing when there is no Maxfood anymore (e.g. player death)
                    if (_pInfoPlayer[i].FoodMax == "0" && _pInfoPlayer[i].Food == "0")
                        continue;



                    #region Draw Lines

                    //Here we check if the content is not our Nickname and watch out, if 
                    //it has already called
                    //e.g. if our Nickname was called 'behind' us.
                    //If so, we change the positions only (because the values are okay)
                    if (Opacity != 1)
                    {
                        //Draw Background
                        e.Graphics.FillRectangle(new SolidBrush(clBackground), 1, 67 + 36*i, pnlArmy.Width - 2,
                                                 33);

                        //Draw scare around Ressources
                        e.Graphics.DrawRectangle(new Pen(new SolidBrush(_pInfoPlayer[i].PlayerColor), 2), 0,
                                                 66 + 36*i, pnlArmy.Width - 1, 34);
                    }

                    //Nickname of player
                    e.Graphics.DrawString(_pInfoPlayer[i].PlayerName,
                                          new Font("Century Gothic", 14.25f, FontStyle.Bold),
                                          new SolidBrush(_pInfoPlayer[i].PlayerColor),
                                          new Point(0, 70 + 36*i));

                    //Team of players
                    e.Graphics.DrawString("#" + _pInfoPlayer[i].PlayerTeam,
                                          new Font("Century Gothic", 14.25f, FontStyle.Regular),
                                          new SolidBrush(Color.White),
                                          new Point(152, 70 + 36*i));

                    //Minerals of Player
                    e.Graphics.DrawString(_pInfoPlayer[i].ArmySizeMinerals,
                                          new Font("Century Gothic", 14.25f, FontStyle.Regular),
                                          new SolidBrush(Color.White),
                                          new Point(222, 70 + 36*i));

                    //Gas of Player
                    e.Graphics.DrawString(_pInfoPlayer[i].ArmySizeGas,
                                          new Font("Century Gothic", 14.25f, FontStyle.Regular),
                                          new SolidBrush(Color.White),
                                          new Point(344, 70 + 36*i));

                    //Supply of Player
                    e.Graphics.DrawString(_pInfoPlayer[i].Food + "/" +
                                          _pInfoPlayer[i].FoodMax,
                                          new Font("Century Gothic", 14.25f, FontStyle.Regular),
                                          new SolidBrush(Color.White),
                                          new Point(467, 70 + 36*i));


                    #endregion

                    #region Draw Pictures

                    //Reset values to avoid stack 
                    // => result is, that after a game,
                    //the Icosn won't change
                    _imgIconsMinerals = null;
                    _imgIconsGas = null;
                    _imgIconsSupply = null;
                    _imgIconsWorker = null;

                    #region Icon/ Race detection

                    _strresult = _pInfoPlayer[i].Race;
                    if (_strresult.Contains("Terr"))
                    {
                        _imgIconsMinerals = Properties.Resources.Mineral_Terran;
                        _imgIconsGas = Properties.Resources.Gas_Terran;
                        _imgIconsSupply = Properties.Resources.Supply_Terran;
                        _imgIconsWorker = Properties.Resources.T_SCV;
                    }

                    if (_strresult.Contains("Zerg"))
                    {
                        _imgIconsMinerals = Properties.Resources.Mineral_Zerg;
                        _imgIconsGas = Properties.Resources.Gas_Zerg;
                        _imgIconsSupply = Properties.Resources.Supply_Zerg;
                        _imgIconsWorker = Properties.Resources.Z_Drone;
                    }

                    if (_strresult.Contains("Prot"))
                    {
                        _imgIconsMinerals = Properties.Resources.Mineral_Protoss;
                        _imgIconsGas = Properties.Resources.Gas_Protoss;
                        _imgIconsSupply = Properties.Resources.Supply_Protoss;
                        _imgIconsWorker = Properties.Resources.P_Probe;
                    }

                    #endregion


                    //Mineral Drawing
                    e.Graphics.DrawImage(_imgIconsMinerals, new Point(191, ivalue + 36*i));

                    //Gas Drawing
                    e.Graphics.DrawImage(_imgIconsGas, new Point(314, ivalue + 36*i));

                    //Supply Drawing
                    e.Graphics.DrawImage(_imgIconsSupply, new Point(437, ivalue + 36*i));


                    #endregion

                }
                catch
                {
                    new Exception();
                }
            }
        }
        
        //Notification Panel throws notifications if you are about to be supplyblocked
        private void PnlNotificationPaint(object sender, PaintEventArgs e)
        {
            if (!pnlNotification.Enabled) return;
            if (!_pvView.ProcessInForeground()) return;
            if (!_mInfor.InGame)
                return;

            for (var i = 0; i < _pInfor.PlayerAmount; i++)
            {
                if (i != _pInfoPlayer[0].LocalPlayer - 1) continue;

                if (int.Parse(_pInfoPlayer[i].Food) + 3 < int.Parse(_pInfoPlayer[i].FoodMax)) continue;

                //Background
                e.Graphics.FillRectangle(new SolidBrush(Color.Gray), 0, 0, pnlNotification.Width, pnlNotification.Height);

                //Bounds
                e.Graphics.DrawRectangle(new Pen(new SolidBrush(_pInfoPlayer[i].PlayerColor), 2), 1, 1,
                                         pnlNotification.Width - 2, pnlNotification.Height - 2);

                //Draw Text
                if (_pInfoPlayer[i].Race == "Terran")
                    e.Graphics.DrawString("build more Supplydepots!", new Font("Arial", 15f, FontStyle.Bold),
                                          new SolidBrush(_pInfoPlayer[i].PlayerColor), 80, 10);

                if (_pInfoPlayer[i].Race == "Protoss")
                    e.Graphics.DrawString("build more Pylons!", new Font("Arial", 15f, FontStyle.Bold),
                                          new SolidBrush(_pInfoPlayer[i].PlayerColor), 100, 10);

                if (_pInfoPlayer[i].Race == "Zerg")
                    e.Graphics.DrawString("build more Overlords!", new Font("Arial", 15f, FontStyle.Bold),
                                          new SolidBrush(_pInfoPlayer[i].PlayerColor), 100, 10);
            }
        }  

        //Autoinject, problems inc, lol :D
        private void AutoInject()
        { 
            if (!_Autoinject)
                return;

            #region Variables


            var iPlayableWidht = _mInfor.PlayAbleWidht;
            var iPlayableHeight = _mInfor.PlayAbleHeight;


            float iScale = 0,
                  iX = 0,
                  iY = 0;

            #endregion

            #region Get minimap Bounds

            if (pnlMaphack.Height/pnlMaphack.Width >= iPlayableHeight/iPlayableWidht)
            {
                iScale = (float) pnlMaphack.Width/(float) iPlayableWidht;
                iX = 0;
                iY = (pnlMaphack.Height - iScale*iPlayableHeight)/2;
            }
            else
            {
                iScale = (float) pnlMaphack.Height/(float) iPlayableHeight;
                iY = 0;
                iX = (pnlMaphack.Width - iScale*iPlayableWidht)/2;
            }

            #endregion

            for (var i = _uInfoUnit[0].TotalUnits - 1; i >= 0; i--)
            {
                //If Hatchery/Lair/Hive is full with puke- shit, go to next unit
                if (_uInfoUnit[i].UnitName.Contains("Hatchery") || _uInfoUnit[i].UnitName.Contains("Lair") ||
                    _uInfoUnit[i].UnitName.Contains("Hive"))
                {
                    if (_uInfoUnit[i].UnitPuked == 3 || _uInfoUnit[i].UnitPuked == 0)
                        continue;
                }

                //Format the actual point
                var fx = (_uInfoUnit[i].PositionX - _mInfor.MapLeft) * iScale + iX + pnlMaphack.Location.X;
                var fy = (_mInfor.MapTop - _uInfoUnit[i].PositionY) * iScale + iY + pnlMaphack.Location.Y;

                //If its an enemy player-unit, go to next unit
                if (_uInfoUnit[i].Owner != _pInfoPlayer[0].LocalPlayer)
                    continue;

                if (_uInfoUnit[i].UnitName.Contains("Queen"))
                    _OkayToInject = _uInfoUnit[i].UnitEnergy > 25;



                if (_uInfoUnit[i].UnitName.Contains("Hatchery") || _uInfoUnit[i].UnitName.Contains("Lair") ||
                    _uInfoUnit[i].UnitName.Contains("Hive"))
                {
                    if (!_OkayToInject)
                        continue;

                    //Save last position
                    //0x11 = CRTL
                    //0x74 = F5
                    PostMessage(iWindow, 0x100, 0x11, 0);
                    PostMessage(iWindow, 0x100, 0x74, 0);

                    PostMessage(iWindow, 0x101, 0x74, 0);
                    PostMessage(iWindow, 0x101, 0x11, 0);

                    

                    //Call Queengroup '0' + jump to it
                    SendMessage(iWindow, 0x100, (IntPtr) 0x30, (IntPtr) 0x101);

                    //Inject 'V'
                    SendMessage(iWindow, 0x100, (IntPtr) 0x56, (IntPtr) 0x101);

                    //Click on Hatchery
                    SendMessage(iWindow, 0x201, (IntPtr) 0, (IntPtr) MakeLParam(new Point((int) fx, (int) fy)));
                    SendMessage(iWindow, 0x202, (IntPtr) 0, (IntPtr) MakeLParam(new Point((int) fx, (int) fy)));

                    //Call group 1 (assume it's your main group)+ jump to it
                    SendMessage(iWindow, 0x100, (IntPtr) 0x31, (IntPtr) 0x101);


                    //Call Position
                    SendMessage(iWindow, 0x100, (IntPtr)0x74, (IntPtr)0x101);
                }
            }
        }     

        //Autogroup Creeptumors actually, this is an autostart but I don't like it.
        private void AutoCreepGroup()
        {
            //if (!_mInfor.InGame)
            //{
            //    _autostart = false;
            //    return;
            //}

            //if (_autostart)
            //    return;


            //#region Variables


            //var iPlayableWidht = _mInfor.PlayAbleWidht;
            //var iPlayableHeight = _mInfor.PlayAbleHeight;


            //float iScale = 0,
            //      iX = 0,
            //      iY = 0,
            //      iScale2 = 0,
            //      iX2 = 0,
            //      iY2 = 0;

            //#endregion

            //#region Get minimap Bounds

            //if (pnlMaphack.Height/pnlMaphack.Width >= iPlayableHeight/iPlayableWidht)
            //{
            //    iScale = (float) pnlMaphack.Width/(float) iPlayableWidht;
            //    iX = 0;
            //    iY = (pnlMaphack.Height - iScale*iPlayableHeight)/2;
            //}
            //else
            //{
            //    iScale = (float) pnlMaphack.Height/(float) iPlayableHeight;
            //    iY = 0;
            //    iX = (pnlMaphack.Width - iScale*iPlayableWidht)/2;
            //}

            //#endregion

            //#region Get Normal Bounds

            //if (Screen.PrimaryScreen.Bounds.Height / Screen.PrimaryScreen.Bounds.Width >= iPlayableHeight / iPlayableWidht)
            //{
            //    iScale2 = (float)Screen.PrimaryScreen.Bounds.Width / (float)iPlayableWidht;
            //    iX2 = 0;
            //    iY2 = (Screen.PrimaryScreen.Bounds.Height - iScale2 * iPlayableHeight) / 2;
            //}
            //else
            //{
            //    iScale2 = (float)Screen.PrimaryScreen.Bounds.Height / (float)iPlayableHeight;
            //    iY2 = 0;
            //    iX2 = (Screen.PrimaryScreen.Bounds.Width - iScale2 * iPlayableWidht) / 2;
            //}

            //#endregion

            //PostMessage(iWindow, (uint) WMessages.WmKeydown, (int) VKeys.VkShift, 0);
            //PostMessage(iWindow, (uint)WMessages.WmKeydown, (int)VKeys.VkF1, 0);
            //PostMessage(iWindow, (uint)WMessages.WmKeyup, (int)VKeys.VkF1, 0);
            //PostMessage(iWindow, (uint)WMessages.WmKeyup, (int)VKeys.VkShift, 0);

            //for (var i = _uInfoUnit[0].TotalUnits - 1; i >= 0; i--)
            //{
                

            //    if (_uInfoUnit[i].UnitName.Contains("Mineral"))
            //    {
            //         if (_uInfoUnit[i].PositionY >= 125)
            //         {
            //             PostMessage(iWindow, (uint)WMessages.WmKeydown, (int)VKeys.VkG, 0);
            //             PostMessage(iWindow, (uint)WMessages.WmKeyup, (int)VKeys.VkG, 0);

            //             var fx = (_uInfoUnit[i].PositionX - _mInfor.MapLeft) * iScale + iX + pnlMaphack.Location.X;
            //             var fy = (_mInfor.MapTop - _uInfoUnit[i].PositionY) * iScale + iY + pnlMaphack.Location.Y;

            //             SendMessage(iWindow, (int)WMessages.WmLbuttondown, (IntPtr)0, (IntPtr)MakeLParam(new Point((int)fx, (int)fy)));
            //             SendMessage(iWindow, (int)WMessages.WmLbuttonup, (IntPtr)0, (IntPtr)MakeLParam(new Point((int)fx, (int)fy)));

            //         }
            //    }
            //}

            //_autostart = true;
            //return;

            //for (var i = _uInfoUnit[0].TotalUnits - 1; i >= 0; i--)
            //{

            //    if (_uInfoUnit[i].UnitName.Contains("Roach"))
            //    {
            //        //Unitpositions
            //        var fx = (_uInfoUnit[i].PositionX - _mInfor.MapLeft) * iScale + iX;
            //        var fy = (_mInfor.MapTop - _uInfoUnit[i].PositionY) * iScale + iY;

            //        var fx2 = (_uInfoUnit[i].PositionX - _mInfor.MapLeft) * iScale2 + iX2;
            //        var fy2 = (_mInfor.MapTop - _uInfoUnit[i].PositionY) * iScale2 + iY2;

            //        //Click on Minimap to this location
            //        SendMessage(iWindow, (int)WMessages.WmLbuttondown, (IntPtr)0, (IntPtr)MakeLParam(new Point(pnlMaphack.Location.X + (int)fx, pnlMaphack.Location.Y + (int)fy)));
            //        SendMessage(iWindow, (int)WMessages.WmLbuttonup, (IntPtr)0, (IntPtr)MakeLParam(new Point(pnlMaphack.Location.X + (int)fx, pnlMaphack.Location.Y + (int)fy)));

            //        Thread.Sleep(1);

            //        //Save last position
            //        //0x11 = CRTL
            //        //0x74 = F5
            //        //PostMessage(iWindow, 0x100, (int)VKeys.VK_LSHIFT, 0);
            //        PostMessage(iWindow, 0x100, (int)VKeys.VkControl, 0);
            //        PostMessage(iWindow, 0x100, (int) VKeys.Vk1, 0);


            //        //Click on that unit
            //        SendMessage(iWindow, (int)WMessages.WmLbuttondown, (IntPtr)0, (IntPtr)MakeLParam(new Point(Screen.PrimaryScreen.Bounds.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2 - 140)));
            //        SendMessage(iWindow, (int)WMessages.WmLbuttonup, (IntPtr)0, (IntPtr)MakeLParam(new Point(Screen.PrimaryScreen.Bounds.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2 - 140)));


            //        PostMessage(iWindow, 0x101, (int)VKeys.VkControl, 0);
            //        PostMessage(iWindow, 0x101, (int)VKeys.Vk1, 0);

            //        break;
            //    }
            //}

            //string name = _uInfoUnit[157].UnitName;
            
        }

        //Refresh the form
        private void TmrTickTick(object sender, EventArgs e)
        {
            try
            {
                #region PlayerInfo Intro

                //Catch the Playeramount, call the method with 0 players
                _pInfor = new PlayerInfo(0);
                //Create new pInfo and fill it with data (to call it only once)
                _pInfoPlayer = new List<PlayerInfo>();

                //Fill with Data
                for (var i = 0; i < _pInfor.PlayerAmount; i++)
                    _pInfoPlayer.Add(new PlayerInfo(i));

                #endregion

                #region UnitInfo Intro

                if (pnlMaphack.Enabled || pnlProduction.Enabled)
                {
                    //Catch the Unitamount
                    _uInfor = new UnitInfo(0);
                    //Create new uInfo and fill the Data
                    _uInfoUnit = new List<UnitInfo>();

                    //Fill with Data
                    for (var r = 0; r < _uInfor.TotalUnits; r++)
                        _uInfoUnit.Add(new UnitInfo(r));
                }

                #endregion

                _pvView = new ProcessView("SC2");
                if (!_pvView.ProcessAviable())
                    Application.Exit();

                if (_pvView.ProcessInForeground())
                    TopMost = true;


                //Proof for the panels (to safe ressources)
                if (pnlWorkers.Enabled) 
                    pnlWorkers.Invalidate();

                if (pnlRessources.Enabled)
                    pnlRessources.Invalidate();
                
                if (pnlIncome.Enabled)
                    pnlIncome.Invalidate();

                if (pnlStates.Enabled)
                    pnlStates.Invalidate();

                if (pnlMaphack.Enabled)
                    pnlMaphack.Invalidate();

                if (pnlAPM.Enabled)
                    pnlAPM.Invalidate();

                if (pnlArmy.Enabled)
                    pnlArmy.Invalidate();

                if (pnlNotification.Enabled)
                    pnlNotification.Invalidate();
  

                AdjustPanelHeight();    //Adjust the height of the Panels 
                //AutoInject();           //Calls the Autoinject- method
                //AutoCreepGroup();
            }

            catch { new Exception(); }
        }

        //Catch the informations for the player. 
        private void CatchInput()
        {
            //Here we proof
            //if we get some commands by the user
            //usually something starting with a slash!
            //Normal Shortcuts


            //Workercount Panel
            CatchKeyboardInput("/wc", pnlWorkers);

            //Ressources Panel
            CatchKeyboardInput("/tg", pnlRessources);

            //Income
            CatchKeyboardInput("/ic", pnlIncome);

            //Information Panel
            CatchKeyboardInput("/ip", pnlStates);

            //Maphack
            CatchKeyboardInput("/mh", pnlMaphack);

            //Unit Panel
            CatchKeyboardInput("/ut", pnlProduction);

            //APM Panel
            CatchKeyboardInput("/ap", pnlAPM);

            //ArmyPanel
            CatchKeyboardInput("/ar", pnlArmy);

            //Notification Panel
            CatchKeyboardInput("/np", pnlNotification);

            //Settings call
            CatchKeyboardInputForSettings("/setting");

            


            //Position Adjustment
            //Workercount Panel
            CatchKeyboardInputAdjustment("/awc", pnlWorkers, _bdone1);

            //Ressources Panel
            CatchKeyboardInputAdjustment("/atg", pnlRessources, _bdone2);

            //Income Panel
            CatchKeyboardInputAdjustment("/aic", pnlIncome, _bdone3);

            //Information Panel
            CatchKeyboardInputAdjustment("/aip", pnlStates, _bdone4);

            //Maphack Panel
            CatchKeyboardInputAdjustment("/amh", pnlMaphack, _bdone5);

            //Unit Panel
            CatchKeyboardInputAdjustment("/aut", pnlProduction, _bdone6);

            //APM Panel
            CatchKeyboardInputAdjustment("/aap", pnlAPM, _bdone7);

            //Army Panel
            CatchKeyboardInputAdjustment("/aar", pnlArmy, _bdone8);

            //Notificationpanel
            CatchKeyboardInputAdjustment("/anp", pnlNotification, _bdone9);

        }

        //Method for the Keyboard input
        /// <summary>
        /// Here we catch the simple input ('/tg')
        /// </summary>
        /// <param name="shortcut">Our input- code</param>
        /// <param name="dbpanel">The panel where the stuff should affect on</param>
        private void CatchKeyboardInput(string shortcut, DoubleBufferPanel dbpanel)
        {
            var strInput = Encoding.UTF8.GetString(DoHandle(_pcPointer.PointerOffset(Chat, Offset1Chat, Offset2Chat, Offset3Chat, Offset4Chat, Offset5Chat), (uint)shortcut.Length));

         


            if (_strBackup != strInput)
                _bchanged = true;

            _strBackup = strInput;


            //Proof for shortcut
            if (strInput == shortcut)
            {
                if (_bchanged)
                {
                    if (dbpanel.Visible)
                    {
                        dbpanel.Visible = false;
                        dbpanel.Enabled = false;
                    }

                    else if (dbpanel.Visible == false)
                    {
                        dbpanel.Visible = true;
                        dbpanel.Enabled = true;
                    }

                    WriteMemory(
                        _pcPointer.PointerOffset(Chat, Offset1Chat, Offset2Chat, Offset3Chat, Offset4Chat, Offset5Chat),
                        Encoding.UTF8.GetBytes(" "));
                    _bchanged = false;
                }
            }
        }

        //Method for the Keyboard input, adjust Panel positions
        /// <summary>
        /// Here the user can adjust his positions (for the panels)
        /// </summary>
        /// <param name="shortcut">The code for the input (e.g. '/atg')</param>
        /// <param name="dbpanel">The panel where we want to take affect on</param>
        /// <param name="bdone">Check if it's already done, if so, disable</param>
        private void CatchKeyboardInputAdjustment(string shortcut, DoubleBufferPanel dbpanel, bool bdone)
        {
            var strInput = Encoding.UTF8.GetString(DoHandle(_pcPointer.PointerOffset(Chat, Offset1Chat, Offset2Chat, Offset3Chat, Offset4Chat, Offset5Chat), (uint)shortcut.Length));


            if (_strBackup != strInput)
                _bchanged = true;

            _strBackup = strInput;


            //Proof for shortcut
            if (_strBackup == shortcut)
                bdone = true;
            

            if (bdone)
                dbpanel.Location = Cursor.Position;
        }

        /// <summary>
        /// Method to catch the Settings-call to display the settings-window.
        /// </summary>
        /// <param name="shortcut">Your shortcut for the call</param>
        private void CatchKeyboardInputForSettings(string shortcut)
        {
            var strInput = Encoding.UTF8.GetString(DoHandle(_pcPointer.PointerOffset(Chat, Offset1Chat, Offset2Chat, Offset3Chat, Offset4Chat, Offset5Chat), (uint)shortcut.Length));


            if (_strBackup != strInput)
                _bchanged = true;

            _strBackup = strInput;


            //Proof for shortcut
            if (_strBackup == shortcut)
            {
                if (_bchanged)
                {
                    var cfg = new Configure(this);
                    cfg.ShowDialog();

                    _bchanged = false;
                }
            }
        }

        //Catch exception (when no SC2 is found)
        private void CallException()
        {
            
            HackAlreadyRunning();

            var bcheck = false;

            var pc = Process.GetProcesses();
            for (var i = 0; i < pc.Length; i++)
            {
                if (pc[i].ProcessName == "SC2")
                {
                    _pvView = new ProcessView("SC2");
                    _pcPointer = new PointerScan("SC2");
                    bcheck = true;
                    break;
                }
            }

            if (bcheck == false)
            {
                tmrTick.Enabled = false;
                MessageBox.Show("SC2 not found!", "Failure!");
                Close();
            }
        }

        //Catch Keystrokes by the user
        private void CatchKeystrokes()
        {
            //Get input for Ressource-panel 
            int icatch1 = GetAsyncKeyState(_uRessource);    //Ressource-Panel
            int icatch2 = GetAsyncKeyState(_uWorker);       //Worker- Panel
            int icatch3 = GetAsyncKeyState(_uIncome);       //Income- Panel
            int icatch4 = GetAsyncKeyState(_uStates);       //States- Panel
            int icatch5 = GetAsyncKeyState(_uMaphack);      //Maphack- Panel
            int icatch6 = GetAsyncKeyState(0x0D);           //Enter/ adjustment
            int icatch7 = GetAsyncKeyState(_uProduction);   //Unit- Panel
            int icatch8 = GetAsyncKeyState(_uApm);          //APM- Panel
            int icatch9 = GetAsyncKeyState(_uArmy);         //Army Panel
            int iSettings = GetAsyncKeyState(_uSettings);   //Settings-call
            int icatch10 = GetAsyncKeyState(_uInject);      //Inject- method

            //Proof for Setting-form
            if (iSettings == -32767 || iSettings == 1)
            {
                WriteSettings();
                Hide();
                tmrTick.Enabled = false;

                var cfg = new Configure(this);
                cfg.ShowDialog();
            }

            //Inject- method
            //if (icatch10 == -32767 || icatch10 == 1)
            //    _Autoinject = !_Autoinject;

            PanelEnableOrDisable(pnlRessources, icatch1);
            PanelEnableOrDisable(pnlWorkers, icatch2);
            PanelEnableOrDisable(pnlIncome, icatch3);
            PanelEnableOrDisable(pnlStates, icatch4);
            PanelEnableOrDisable(pnlMaphack, icatch5);
            PanelEnableOrDisable(pnlProduction, icatch7);
            PanelEnableOrDisable(pnlAPM, icatch8);
            PanelEnableOrDisable(pnlArmy, icatch9);
            
       

            #region Fix Panels position

            //Fix the new position with Enter
            if (icatch6 != 1 && icatch6 != -32767) return;

            _bdone1 = false;
            _bdone2 = false;
            _bdone3 = false;
            _bdone4 = false;
            _bdone5 = false;
            _bdone6 = false;
            _bdone7 = false;
            _bdone8 = false;
            _bdone9 = false;

            #endregion


        }

        //Adjust the height of the Panels 
        private void AdjustPanelHeight()
        {
            //Cath the amount of players without observers
            var i = _pInfor.PlayerAmount;

            pnlRessources.Height = 70 + 36*i;
            pnlIncome.Height = 70 + 36*i;
            pnlAPM.Height = 70 + 36*i;
            pnlProduction.Height = 10 + 30 *i;
            pnlArmy.Height = 70 + 36*i;
        }

        //Catch the Location for the Readstuff
        /// <summary>
        /// Method to grab the actual Points intead of 'Location.ToString()'
        /// </summary>
        /// <param name="definition">The words before the Location</param>
        /// <param name="line">The line where the definition is found</param>
        /// <returns></returns>
        private static Point CatchPoint(string definition)
        {
            //Line 5, 6, 7, 8
            //{X=25,Y=25}
            //Read the textfile- informations
            //var sr = new StreamReader("Settings.ini");
            //string[] strsplit0 = sr.ReadToEnd().Split('\r');


            string strresult = definition;

            string[] strsplit = strresult.Split('=');
            string[] strsplit2 = strsplit[1].Split(',');
            string[] strsplit3 = strsplit[2].Split('}');

            string a = strsplit2[0];
            string b = strsplit3[0];

            var ptr = new Point(int.Parse(a), int.Parse(b));

            return ptr;
        }

        //Write new positions when closing!
        private void Form1FormClosing(object sender, FormClosingEventArgs e)
        {
            //Write settings
            WriteSettings();

            //Thread closing
            Environment.Exit(1);
        }

        //Method to enable/disable panels 
        private static void PanelEnableOrDisable(DoubleBufferPanel pnl, int iNumber)
        {
            if (iNumber == 1 || iNumber == -32767)
            {
                if (pnl.Visible)
                {
                    pnl.Visible = false;
                    pnl.Enabled = false;
                }

                else if (!pnl.Visible)
                {
                    pnl.Visible = true;
                    pnl.Enabled = true;
                }
            }
        }

        //Check if this tool is already running
        private void HackAlreadyRunning()
        {
            //var proc = Process.GetProcesses();
            //var bproof = false;

            //for (var i = 0; i< proc.Length; i++)
            //{
            //    if (proc[i].ProcessName == CurrentProcessName)
            //        bproof = true;
            //}

            //if (bproof)
            //{
            //    MessageBox.Show("The hack is already launched!", "Heyyy, yay!");
            //    Close();
            //}
        }

        //Enable/ disable the timer with this from the configure-form
        public bool ToggleTimer
        {
            get { return tmrTick.Enabled; }
            set { tmrTick.Enabled = value; }
        }

        //Check if the Unit is dead or alive
        private bool UnitIsAlive(int iUnit)
        {
            //In case there is a building SCV
            if (_uInfoUnit[iUnit].State == 35)
                return true;

            return ((_uInfoUnit[iUnit].TagetFilterFlag & 0x200000000) == 0);

        }

        //Other thread to catch the unit's ID's
        public void ThreadFunction()
        {
            Graphics e = pnlProduction.CreateGraphics();

            while (true)
            {
                if (_pvView.ProcessInForeground())
                {
                    try
                    {
                        #region Variables

                        //Terran Units
                        var scv = new int[_pInfor.PlayerAmount];
                        var marine = new int[_pInfor.PlayerAmount];
                        var mule = new int[_pInfor.PlayerAmount];
                        var marauder = new int[_pInfor.PlayerAmount];
                        var reaper = new int[_pInfor.PlayerAmount];
                        var ghost = new int[_pInfor.PlayerAmount];
                        var hellion = new int[_pInfor.PlayerAmount];
                        var siegetank = new int[_pInfor.PlayerAmount];
                        var thor = new int[_pInfor.PlayerAmount];
                        var viking = new int[_pInfor.PlayerAmount];
                        var banshee = new int[_pInfor.PlayerAmount];
                        var medivac = new int[_pInfor.PlayerAmount];
                        var raven = new int[_pInfor.PlayerAmount];
                        var battlecruiser = new int[_pInfor.PlayerAmount];

                        //Protoss Units
                        var probe = new int[_pInfor.PlayerAmount];
                        var zealot = new int[_pInfor.PlayerAmount];
                        var stalker = new int[_pInfor.PlayerAmount];
                        var sentry = new int[_pInfor.PlayerAmount];
                        var darktemplar = new int[_pInfor.PlayerAmount];
                        var hightemplar = new int[_pInfor.PlayerAmount];
                        var immortal = new int[_pInfor.PlayerAmount];
                        var colossus = new int[_pInfor.PlayerAmount];
                        var warpprism = new int[_pInfor.PlayerAmount];
                        var observer = new int[_pInfor.PlayerAmount];
                        var phoenix = new int[_pInfor.PlayerAmount];
                        var voidray = new int[_pInfor.PlayerAmount];
                        var carrier = new int[_pInfor.PlayerAmount];
                        var mothership = new int[_pInfor.PlayerAmount];
                        var archon = new int[_pInfor.PlayerAmount];

                        //Zerg Units
                        var drone = new int[_pInfor.PlayerAmount];
                        var larva = new int[_pInfor.PlayerAmount];
                        var zergling = new int[_pInfor.PlayerAmount];
                        var baneling = new int[_pInfor.PlayerAmount];
                        var overlord = new int[_pInfor.PlayerAmount];
                        var roach = new int[_pInfor.PlayerAmount];
                        var hydra = new int[_pInfor.PlayerAmount];
                        var mutalisk = new int[_pInfor.PlayerAmount];
                        var corrupter = new int[_pInfor.PlayerAmount];
                        var ultra = new int[_pInfor.PlayerAmount];
                        var broodlord = new int[_pInfor.PlayerAmount];
                        var overseer = new int[_pInfor.PlayerAmount];
                        var infestor = new int[_pInfor.PlayerAmount];
                        var queen = new int[_pInfor.PlayerAmount];


                        #endregion

                        for (var i = 0; i < _uInfor.TotalUnits; i++)
                        {
                            var strUnit = _uInfoUnit[i].UnitName;

                            if (!UnitIsAlive(i))
                                continue;
                            

                        
                            for (var r = 0; r < _pInfor.PlayerAmount; r++)
                            {
                                if (_uInfoUnit[i].Owner != r + 1)
                                    continue;

                                #region Catch the Units


                                //Terran Units
                                scv[r] += GetUnits(strUnit, "SCV");
                                mule[r] += GetUnits(strUnit, "MULE");
                                marine[r] += GetUnits(strUnit, "Marine");
                                marauder[r] += GetUnits(strUnit, "Marauder");
                                ghost[r] += GetUnits(strUnit, "Ghost");
                                reaper[r] += GetUnits(strUnit, "Reaper");
                                hellion[r] += GetUnits(strUnit, "Hellion");
                                siegetank[r] += GetUnits(strUnit, "SiegeTank");
                                thor[r] += GetUnits(strUnit, "Thor");
                                medivac[r] += GetUnits(strUnit, "Medivac");
                                banshee[r] += GetUnits(strUnit, "Banshee");
                                viking[r] += GetUnits(strUnit, "Viking");
                                raven[r] += GetUnits(strUnit, "Raven");
                                battlecruiser[r] += GetUnits(strUnit, "Battlecruiser");

                                //Protoss Units
                                probe[r] += GetUnits(strUnit, "Probe");
                                zealot[r] += GetUnits(strUnit, "Zealot");
                                stalker[r] += GetUnits(strUnit, "Stalker");
                                hightemplar[r] += GetUnits(strUnit, "HighTemplar");
                                darktemplar[r] += GetUnits(strUnit, "DarkTemplar");
                                archon[r] += GetUnits(strUnit, "Archon");
                                immortal[r] += GetUnits(strUnit, "Immortal");
                                warpprism[r] += GetUnits(strUnit, "WarpPrism");
                                observer[r] += GetUnits(strUnit, "Observer");
                                colossus[r] += GetUnits(strUnit, "Colossus");
                                phoenix[r] += GetUnits(strUnit, "Phoenix");
                                voidray[r] += GetUnits(strUnit, "VoidRay");
                                carrier[r] += GetUnits(strUnit, "Carrier");
                                mothership[r] += GetUnits(strUnit, "Mothership");
                                sentry[r] += GetUnits(strUnit, "Sentry");

                                //Zerg Unit
                                drone[r] += GetUnits(strUnit, "Drone");
                                overseer[r] += GetUnits(strUnit, "Overseer");
                                overlord[r] += GetUnits(strUnit, "Overlord");
                                zergling[r] += GetUnits(strUnit, "Zergling");
                                baneling[r] += GetUnits(strUnit, "Baneling");
                                roach[r] += GetUnits(strUnit, "Roach");
                                hydra[r] += GetUnits(strUnit, "Hydralisk");
                                infestor[r] += GetUnits(strUnit, "Infestor");
                                mutalisk[r] += GetUnits(strUnit, "Mutalisk");
                                corrupter[r] += GetUnits(strUnit, "Corruptor");
                                broodlord[r] += GetUnits(strUnit, "BroodLord");
                                larva[r] += GetUnits(strUnit, "Larva");
                                queen[r] += GetUnits(strUnit, "Queen");
                                ultra[r] += GetUnits(strUnit, "Ultralisk");


                                #endregion
                            }
                        }

                        #region Call drawfunction



                        for (var r = 0; r < _pInfor.PlayerAmount; r++)
                        {
                            //int iXpos = 10;
                            //int iReduce = 400;

                            //if (scv[r] > 0)
                            //{
                            //    //CallUnits(scv[r], e, _imgScv, iXpos, (10 + 30*r), "T");
                            //    iXpos = 40;
                            //    _bPos1 = false;
                            //}
                            //else
                            //    _bPos1 = true;

                            //if (mule[r] > 0)
                            //{
                            //    //CallUnits(mule[r], e, _imgMule, iXpos, (10 + 30 * r), "T");
                            //    iXpos = 70;
                            //    _bPos2 = false;
                            //}
                            //else
                            //    _bPos2 = true;

                            //if (marine[r] > 0)
                            //{
                            //    //CallUnits(marine[r], e, _imgMarine, iXpos, (10 + 30 * r), "T");
                            //    iXpos = 100;
                            //    _bPos3 = false;
                            //}
                            //else
                            //    _bPos3 = true;

                            //if (marauder[r] > 0)
                            //{
                            //    //CallUnits(marauder[r], e, _imgMarauder, iXpos, (10 + 30 * r), "T");
                            //    iXpos = 130;
                            //    _bPos4 = false;
                            //}
                            //else
                            //    _bPos4 = true;

                            //if (reaper[r] > 0)
                            //{
                            //    //CallUnits(reaper[r], e, _imgReaper, iXpos, (10 + 30 * r), "T");
                            //    iXpos = 160;
                            //    _bPos5 = false;
                            //}
                            //else
                            //    _bPos5 = true;

                            //if (ghost[r] > 0)
                            //{
                            //    //CallUnits(ghost[r], e, _imgGhost, iXpos, (10 + 30 * r), "T");
                            //    iXpos = 190; 
                            //    _bPos6 = false;
                            //}
                            //else
                            //    _bPos6 = true;

                            //if (hellion[r] > 0)
                            //{
                            //    //CallUnits(hellion[r], e, _imgHellion, iXpos, (10 + 30 * r), "T");
                            //    iXpos = 220;
                            //    _bPos7 = false;
                            //}
                            //else
                            //    _bPos7 = true;

                            //if (siegetank[r] > 0)
                            //{
                            //    //CallUnits(siegetank[r], e, _imgSiegeTank, iXpos, (10 + 30 * r), "T");
                            //    iXpos = 250;
                            //    _bPos8 = false;
                            //}
                            //else
                            //    _bPos8 = true;

                            //if (thor[r] > 0)
                            //{
                            //    //CallUnits(thor[r], e, _imgThor, iXpos, (10 + 30 * r), "T");
                            //    iXpos = 280;
                            //    _bPos9 = false;
                            //}
                            //else
                            //    _bPos9 = true;

                            //if (banshee[r] > 0)
                            //{
                            //    //CallUnits(banshee[r], e, _imgBanshee, iXpos, (10 + 30 * r), "T");
                            //    iXpos = 310;
                            //    _bPos10 = false;
                            //}
                            //else
                            //    _bPos10 = true;

                            //if (medivac[r] > 0)
                            //{
                            //    //CallUnits(medivac[r], e, _imgMedivac, iXpos, (10 + 30 * r), "T");
                            //    iXpos = 340;
                            //    _bPos11 = false;
                            //}
                            //else
                            //    _bPos11 = true;

                            //if (viking[r] > 0)
                            //{
                            //    //CallUnits(viking[r], e, _imgViking, iXpos, (10 + 30 * r), "T");
                            //    iXpos = 370;
                            //    _bPos12 = false;
                            //}
                            //else
                            //    _bPos12 = true;

                            //if (raven[r] > 0)
                            //{
                            //    //CallUnits(raven[r], e, _imgRaven, iXpos, (10 + 30 * r), "T");
                            //    iXpos = 400;
                            //    _bPos13 = false;
                            //}
                            //else
                            //    _bPos13 = true;

                            //if (battlecruiser[r] > 0)
                            //{
                            //    //CallUnits(battlecruiser[r], e, _imgBattlecruiser, iXpos, (10 + 30 * r), "T");
                            //    iXpos = 430;
                            //    _bPos14 = false;
                            //}
                            //else
                            //    _bPos14 = true;




                            //Terran Units
                            if (_pInfoPlayer[r].Race == "Terran")
                            {
                                CallUnits(scv[r], e, _imgScv, 10, (10 + 30*r), "T");
                                CallUnits(mule[r], e, _imgMule, 40, (10 + 30*r), "T");
                                CallUnits(marine[r], e, _imgMarine, 70, (10 + 30*r), "T");
                                CallUnits(marauder[r], e, _imgMarauder, 100, (10 + 30*r), "T");
                                CallUnits(reaper[r], e, _imgReaper, 130, (10 + 30*r), "T");
                                CallUnits(ghost[r], e, _imgGhost, 160, (10 + 30*r), "T");
                                CallUnits(hellion[r], e, _imgHellion, 190, (10 + 30*r), "T");
                                CallUnits(siegetank[r], e, _imgSiegeTank, 220, (10 + 30*r), "T");
                                CallUnits(thor[r], e, _imgThor, 250, (10 + 30*r), "T");
                                CallUnits(banshee[r], e, _imgBanshee, 280, (10 + 30*r), "T");
                                CallUnits(medivac[r], e, _imgMedivac, 310, (10 + 30*r), "T");
                                CallUnits(viking[r], e, _imgViking, 340, (10 + 30*r), "T");
                                CallUnits(raven[r], e, _imgRaven, 370, (10 + 30*r), "T");
                                CallUnits(battlecruiser[r], e, _imgBattlecruiser, 400, (10 + 30*r), "T");
                            }


                            ////Protoss Units
                            if (_pInfoPlayer[r].Race == "Protoss")
                            {
                                CallUnits(probe[r], e, _imgProbe, 10, (10 + 30*r), "P");
                                CallUnits(zealot[r], e, _imgZealot, 40, (10 + 30*r), "P");
                                CallUnits(stalker[r], e, _imgStalker, 70, (10 + 30*r), "P");
                                CallUnits(sentry[r], e, _imgSentry, 100, (10 + 30*r), "P");
                                CallUnits(hightemplar[r], e, _imgHighTemplar, 130, (10 + 30*r), "P");
                                CallUnits(darktemplar[r], e, _imgDarkTemplar, 160, (10 + 30*r), "P");
                                CallUnits(archon[r], e, _imgArchon, 190, (10 + 30*r), "P");
                                CallUnits(immortal[r], e, _imgImmortal, 220, (10 + 30*r), "P");
                                CallUnits(colossus[r], e, _imgColossus, 250, (10 + 30*r), "P");
                                CallUnits(warpprism[r], e, _imgWarpPrism, 280, (10 + 30*r), "P");
                                CallUnits(observer[r], e, _imgOberver, 310, (10 + 30*r), "P");
                                CallUnits(phoenix[r], e, _imgPhoenix, 340, (10 + 30*r), "P");
                                CallUnits(voidray[r], e, _imgVoidRay, 370, (10 + 30*r), "P");
                                CallUnits(carrier[r], e, _imgCarrier, 400, (10 + 30*r), "P");
                                CallUnits(mothership[r], e, _imgMothership, 430, (10 + 30*r), "P");
                            }

                            //Zerg Units
                            if (_pInfoPlayer[r].Race == "Zerg")
                            {
                                CallUnits(drone[r], e, _imgDrone, 10, (10 + 30*r), "Z");
                                CallUnits(overlord[r], e, _imgOverlord, 40, (10 + 30*r), "Z");
                                CallUnits(overseer[r], e, _imgOverseer, 70, (10 + 30*r), "Z");
                                CallUnits(zergling[r], e, _imgZergling, 100, (10 + 30*r), "Z");
                                CallUnits(baneling[r], e, _imgBaneling, 130, (10 + 30*r), "Z");
                                CallUnits(roach[r], e, _imgRoach, 160, (10 + 30*r), "Z");
                                CallUnits(hydra[r], e, _imgHydra, 190, (10 + 30*r), "Z");
                                CallUnits(mutalisk[r], e, _imgMutalisk, 220, (10 + 30*r), "Z");
                                CallUnits(infestor[r], e, _imgInfestor, 250, (10 + 30*r), "Z");
                                CallUnits(corrupter[r], e, _imgCorruptor, 280, (10 + 30*r), "Z");
                                CallUnits(ultra[r], e, _imgUltra, 310, (10 + 30*r), "Z");
                                CallUnits(broodlord[r], e, _imgBroodlord, 340, (10 + 30*r), "Z");
                                CallUnits(queen[r], e, _imgQueen, 370, (10 + 30*r), "Z");
                                CallUnits(larva[r], e, _imgLarva, 400, (10 + 30*r), "Z");
                            }
                        }

                        #endregion
                    }

                    catch
                    {
                        new Exception();
                        //  MessageBox.Show(ex.Message + ex.Source + ex.StackTrace + ex.TargetSite);
                    }

                }
            }
        }

        //Thread to catch Input and Output by the user
        public void InputThread()
        {
            while (true)
            {
                try
                {
                    CatchKeystrokes();
                    //CatchInput();
                }

                catch
                {
                    new Exception();
                }

                Thread.Sleep(10);
            }
        }

        //Method to get the units
        private static int GetUnits(string sUnitString, string sUnitCatch)
        {
            if (sUnitString.Contains(sUnitCatch))
                return 1;
            
            return 0;
        }

        //Method to call the Draw function
        private void CallUnits(int iUnit, Graphics g, Image picture, int xposi, int yposi, string race)
        {
            g.DrawImage(picture, xposi, yposi, 30, 30);
            g.DrawString(iUnit.ToString(CultureInfo.InvariantCulture), new Font("Arial", 8, FontStyle.Regular),
                         new SolidBrush(Color.White), xposi, yposi);

            if (iUnit == 0)
                g.DrawLine(new Pen(new SolidBrush(Color.Red), 2), xposi, yposi, xposi + 30, yposi + 30);

        }

        //WM Messages (Keydown ect)
        public enum WMessages
        {
            WmLbuttondown = 0x201, //Left mousebutton down
            WmLbuttonup = 0x202,  //Left mousebutton up
            WmLbuttondblclk = 0x203, //Left mousebutton doubleclick
            WmRbuttondown = 0x204, //Right mousebutton down
            WmRbuttonup = 0x205,   //Right mousebutton up
            WmRbuttondblclk = 0x206, //Right mousebutton doubleclick
            WmKeydown = 0x100,  //Key down
            WmKeyup = 0x101,   //Key up
        }

        //Actual keys with codes
        public enum VKeys
        {
            VkLbutton = 0x01,   //Left mouse button
            VkRbutton = 0x02,   //Right mouse button
            VkCancel = 0x03,   //Control-break processing
            VkMbutton = 0x04,   //Middle mouse button (three-button mouse)
            VkBack = 0x08,   //BACKSPACE key
            VkTab = 0x09,   //TAB key
            VkClear = 0x0C,   //CLEAR key
            VkReturn = 0x0D,   //ENTER key
            VkShift = 0x10,   //SHIFT key
            VkControl = 0x11,   //CTRL key
            VkMenu = 0x12,   //ALT key
            VkPause = 0x13,   //PAUSE key
            VkCapital = 0x14,   //CAPS LOCK key
            VkEscape = 0x1B,   //ESC key
            VkSpace = 0x20,   //SPACEBAR
            VkPrior = 0x21,   //PAGE UP key
            VkNext = 0x22,   //PAGE DOWN key
            VkEnd = 0x23,   //END key
            VkHome = 0x24,   //HOME key
            VkLeft = 0x25,   //LEFT ARROW key
            VkUp = 0x26,   //UP ARROW key
            VkRight = 0x27,   //RIGHT ARROW key
            VkDown = 0x28,   //DOWN ARROW key
            VkSelect = 0x29,   //SELECT key
            VkPrint = 0x2A,   //PRINT key
            VkExecute = 0x2B,   //EXECUTE key
            VkSnapshot = 0x2C,   //PRINT SCREEN key
            VkInsert = 0x2D,   //INS key
            VkDelete = 0x2E,   //DEL key
            VkHelp = 0x2F,   //HELP key
            Vk0 = 0x30,   //0 key
            Vk1 = 0x31,   //1 key
            Vk2 = 0x32,   //2 key
            Vk3 = 0x33,   //3 key
            Vk4 = 0x34,   //4 key
            Vk5 = 0x35,   //5 key
            Vk6 = 0x36,    //6 key
            Vk7 = 0x37,    //7 key
            Vk8 = 0x38,   //8 key
            Vk9 = 0x39,    //9 key
            VkA = 0x41,   //A key
            VkB = 0x42,   //B key
            VkC = 0x43,   //C key
            VkD = 0x44,   //D key
            VkE = 0x45,   //E key
            VkF = 0x46,   //F key
            VkG = 0x47,   //G key
            VkH = 0x48,   //H key
            VkI = 0x49,    //I key
            VkJ = 0x4A,   //J key
            VkK = 0x4B,   //K key
            VkL = 0x4C,   //L key
            VkM = 0x4D,   //M key
            VkN = 0x4E,    //N key
            VkO = 0x4F,   //O key
            VkP = 0x50,    //P key
            VkQ = 0x51,   //Q key
            VkR = 0x52,   //R key
            VkS = 0x53,   //S key
            VkT = 0x54,   //T key
            VkU = 0x55,   //U key
            VkV = 0x56,   //V key
            VkW = 0x57,   //W key
            VkX = 0x58,   //X key
            VkY = 0x59,   //Y key
            VkZ = 0x5A,    //Z key
            VkNumpad0 = 0x60,   //Numeric keypad 0 key
            VkNumpad1 = 0x61,   //Numeric keypad 1 key
            VkNumpad2 = 0x62,   //Numeric keypad 2 key
            VkNumpad3 = 0x63,   //Numeric keypad 3 key
            VkNumpad4 = 0x64,   //Numeric keypad 4 key
            VkNumpad5 = 0x65,   //Numeric keypad 5 key
            VkNumpad6 = 0x66,   //Numeric keypad 6 key
            VkNumpad7 = 0x67,   //Numeric keypad 7 key
            VkNumpad8 = 0x68,   //Numeric keypad 8 key
            VkNumpad9 = 0x69,   //Numeric keypad 9 key
            VkSeparator = 0x6C,   //Separator key
            VkSubtract = 0x6D,   //Subtract key
            VkDecimal = 0x6E,   //Decimal key
            VkDivide = 0x6F,   //Divide key
            VkF1 = 0x70,   //F1 key
            VkF2 = 0x71,   //F2 key
            VkF3 = 0x72,   //F3 key
            VkF4 = 0x73,   //F4 key
            VkF5 = 0x74,   //F5 key
            VkF6 = 0x75,   //F6 key
            VkF7 = 0x76,   //F7 key
            VkF8 = 0x77,   //F8 key
            VkF9 = 0x78,   //F9 key
            VkF10 = 0x79,   //F10 key
            VkF11 = 0x7A,   //F11 key
            VkF12 = 0x7B,   //F12 key
            VkScroll = 0x91,   //SCROLL LOCK key
            VkLshift = 0xA0,   //Left SHIFT key
            VkRshift = 0xA1,   //Right SHIFT key
            VkLcontrol = 0xA2,   //Left CONTROL key
            VkRcontrol = 0xA3,    //Right CONTROL key
            VkLmenu = 0xA4,      //Left MENU key
            VkRmenu = 0xA5,   //Right MENU key
            VkPlay = 0xFA,   //Play key
            VkZoom = 0xFB, //Zoom key
        }

        //Save settings in .XML File
        private void XmlWrite()
        {
            var myWrite = new XmlTextWriter("Settings.xml", Encoding.UTF8) { Formatting = Formatting.Indented };

            myWrite.WriteStartDocument(false);

            myWrite.WriteStartElement("SC2ExternalHack");

            myWrite.WriteElementString("TimerRefreshRate", tmrTick.Interval.ToString(CultureInfo.InvariantCulture));
            myWrite.WriteElementString("PlayerNickName", _strNickname);

            //Save opacity
            var str = new string[2];
            if (Opacity != 1)
            {
                str = Opacity.ToString(CultureInfo.InvariantCulture).Split('.');

                myWrite.WriteElementString("Opacity", str[0] + "," + str[1]);
            }
            else
                myWrite.WriteElementString("Opacity", Opacity.ToString(CultureInfo.InvariantCulture));

            myWrite.WriteElementString("RessourcePanelPos", pnlRessources.Location.ToString());
            myWrite.WriteElementString("IncomePanelPos", pnlIncome.Location.ToString());
            myWrite.WriteElementString("WorkerPanelPos", pnlWorkers.Location.ToString());
            myWrite.WriteElementString("InformationPanelPos", pnlStates.Location.ToString());
            myWrite.WriteElementString("MaphackPanelPos", pnlMaphack.Location.ToString());
            myWrite.WriteElementString("ProductionPanelPos", pnlProduction.Location.ToString());
            myWrite.WriteElementString("ApmPanelPos", pnlAPM.Location.ToString());
            myWrite.WriteElementString("ArmyPanelPos", pnlArmy.Location.ToString());
            myWrite.WriteElementString("NotificationPanelPos", pnlNotification.Location.ToString());

            myWrite.WriteElementString("RessourcePanelKey", "0x" + Convert.ToString(_uRessource, 16));
            myWrite.WriteElementString("IncomePanelKey", "0x" + Convert.ToString(_uIncome, 16));
            myWrite.WriteElementString("WorkerPanelKey", "0x" + Convert.ToString(_uWorker, 16));
            myWrite.WriteElementString("InformationPanelKey", "0x" + Convert.ToString(_uStates, 16));
            myWrite.WriteElementString("MaphackPanelKey", "0x" + Convert.ToString(_uMaphack, 16));
            myWrite.WriteElementString("ProductionPanelKey", "0x" + Convert.ToString(_uProduction, 16));
            myWrite.WriteElementString("ApmPanelKey", "0x" + Convert.ToString(_uApm, 16));
            myWrite.WriteElementString("ArmyPanelKey", "0x" + Convert.ToString(_uArmy, 16));
            myWrite.WriteElementString("SettingsFormKey", "0x" + Convert.ToString(_uSettings, 16));

            myWrite.WriteElementString("RessourcePanelOnline", pnlRessources.Enabled.ToString(CultureInfo.InvariantCulture));
            myWrite.WriteElementString("IncomePanelOnline", pnlIncome.Enabled.ToString(CultureInfo.InvariantCulture));
            myWrite.WriteElementString("WorkerPanelOnline", pnlWorkers.Enabled.ToString(CultureInfo.InvariantCulture));
            myWrite.WriteElementString("InformationPanelOnline", pnlStates.Enabled.ToString(CultureInfo.InvariantCulture));
            myWrite.WriteElementString("MaphackPanelOnline", pnlMaphack.Enabled.ToString(CultureInfo.InvariantCulture));
            myWrite.WriteElementString("ProductionPanelOnline", pnlProduction.Enabled.ToString(CultureInfo.InvariantCulture));
            myWrite.WriteElementString("ApmPanelOnline", pnlAPM.Enabled.ToString(CultureInfo.InvariantCulture));
            myWrite.WriteElementString("ArmyPanelOnline", pnlArmy.Enabled.ToString(CultureInfo.InvariantCulture));
            myWrite.WriteElementString("NotificationPanelOnline", pnlNotification.Enabled.ToString(CultureInfo.InvariantCulture));



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
                                        tmrTick.Interval = int.Parse(xReader.Value);
                                        break;

                                    case "PlayerNickName":
                                        _strNickname = xReader.Value;
                                        break;

                                    case "Opacity":
                                        Opacity = Convert.ToDouble(xReader.Value);
                                        break;


                                    case "RessourcePanelPos":
                                        pnlRessources.Location = CatchPoint(xReader.Value);
                                        break;

                                    case "IncomePanelPos":
                                        pnlIncome.Location = CatchPoint(xReader.Value);
                                        break;

                                    case "WorkerPanelPos":
                                        pnlWorkers.Location = CatchPoint(xReader.Value);
                                        break;

                                    case "InformationPanelPos":
                                        pnlStates.Location = CatchPoint(xReader.Value);
                                        break;

                                    case "MaphackPanelPos":
                                        pnlMaphack.Location = CatchPoint(xReader.Value);
                                        break;

                                    case "ProductionPanelPos":
                                        pnlProduction.Location = CatchPoint(xReader.Value);
                                        break;

                                    case "ApmPanelPos":
                                        pnlAPM.Location = CatchPoint(xReader.Value);
                                        break;

                                    case "ArmyPanelPos":
                                        pnlArmy.Location = CatchPoint(xReader.Value);
                                        break;

                                    case "NotificationPanelPos":
                                        pnlNotification.Location = CatchPoint(xReader.Value);
                                        break;


                                    case "RessourcePanelKey":
                                        _uRessource = Convert.ToInt32(xReader.Value, 16);
                                        break;

                                    case "IncomePanelKey":
                                        _uIncome = Convert.ToInt32(xReader.Value, 16);
                                        break;

                                    case "WorkerPanelKey":
                                        _uWorker = Convert.ToInt32(xReader.Value, 16);
                                        break;

                                    case "InformationPanelKey":
                                        _uStates = Convert.ToInt32(xReader.Value, 16);
                                        break;

                                    case "MaphackPanelKey":
                                        _uMaphack = Convert.ToInt32(xReader.Value, 16);
                                        break;

                                    case "ProductionPanelKey":
                                        _uProduction = Convert.ToInt32(xReader.Value, 16);
                                        break;

                                    case "ApmPanelKey":
                                        _uApm = Convert.ToInt32(xReader.Value, 16);
                                        break;

                                    case "ArmyPanelKey":
                                        _uArmy = Convert.ToInt32(xReader.Value, 16);
                                        break;

                                    case "SettingsFormKey":
                                        _uSettings = Convert.ToInt32(xReader.Value, 16);
                                        break;


                                    case "RessourcePanelOnline":
                                        pnlRessources.Visible = Convert.ToBoolean(xReader.Value);
                                        break;

                                    case "IcomePanelOnline":
                                        pnlIncome.Visible = Convert.ToBoolean(xReader.Value);
                                        break;

                                    case "WorkerPanelOnline":
                                        pnlWorkers.Visible = Convert.ToBoolean(xReader.Value);
                                        break;

                                    case "InformationPanelOnline":
                                        pnlStates.Visible = Convert.ToBoolean(xReader.Value);
                                        break;

                                    case "MaphackPanelOnline":
                                        pnlMaphack.Visible = Convert.ToBoolean(xReader.Value);
                                        break;

                                    case "ProductionPanelOnline":
                                        pnlProduction.Visible = Convert.ToBoolean(xReader.Value);
                                        break;

                                    case "ApmPanelOnline":
                                        pnlAPM.Visible = Convert.ToBoolean(xReader.Value);
                                        break;

                                    case "ArmyPanelOnline":
                                        pnlArmy.Visible = Convert.ToBoolean(xReader.Value);
                                        break;

                                    case "NotificationPanelOnline":
                                        pnlNotification.Visible = Convert.ToBoolean(xReader.Value);
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
