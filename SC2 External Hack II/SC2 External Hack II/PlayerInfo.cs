using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Xml;
using bellaPatricia_Lib;
using System.IO;
using System.Runtime.InteropServices;

namespace SC2_External_Hack_II
{
    internal class PlayerInfo
    {
        [DllImport("ReadMemory.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe int ReadMemory(void* address);

        private readonly List<string> _sPlayerInfo = new List<string>();
        private Color _cl = Color.White;


        #region Addresses

        //Declaring Addresses
        private int _iPlayerBase = 0,
                    _iPlayerName = 0x026A8FC0,
                    _iPlayerNameLenght = 0x026A8FB8,
                    _iPlayerCurrentMinerals = 0x026A9448,
                    _iPlayerCurrentGas = 0x026A9450,
                    _iPlayerRace = 0x03257738,
                    _iPlayerWorkerCount = 0x026A9368,
                    _iPlayerLocal = 0x015b3f2c,
                    _iPlayerCurrentSupply = 0x026A9420,
                    _iPlayerMaxSupply = 0x026A9418,
                    _iPlayerArmySizeMinerals = 0x026A9730,
                    _iPlayerArmySizeGas = 0x026A9750,
                    _iPlayerIncomeMinerals = 0x026A94C8,
                    _iPlayerIncomeGas = 0x026A94D0,
                    _iPlayerUnitsLost = 0x026A9258,
                    _iPlayerTeam = 0x02648550,
                    _iPlayerCurrentApm = 0x26A91E8,
                    _iPlayerAverageApm = (0x26A8F78 + 0x248),
                    _iPlayerTimerData = 0x15D482C,
                    _iPlayerRealCurrentApm = 0x26A9220,
                    _iPlayerCameraX = 0x26A8F80,
                    _iPlayerCameraY = 0x26A8F84,
                    _iPlayerColor = 0x026A9018,
                    _iPlayerBattleNetId = 0x016CA8B8,
                    _iPlayerAmount = 0x2D541F0,
                    _iPlayerOffsetBattleNetId = 0x4B0,
                    _iPlayerOffsetStandart = 0x910,
                    _iPlayerOffsetTeam = 0x4,
                    _iPlayerOffsetRace = 0x10;

        #endregion

        /// <summary>
        /// Call to make yout life easier!
        /// </summary>
        /// <param name="iPlayer">The player you want to call</param>
        private int ipl = 0;

        public PlayerInfo(int iPlayer)
        {
            ipl = iPlayer;

            #region Change addresses for each Patch

            var sc2Proc = Process.GetProcessesByName("SC2")[0];
            switch (sc2Proc.MainModule.FileVersionInfo.FileVersion)
            {
                case "1.4.3.21029":
                    break;

                case "1.5.0.21995":
                    _iPlayerName = 0x2F4A12C;
                    _iPlayerNameLenght = 0x02374A88;
                    _iPlayerCurrentMinerals = 0x02F4A708;
                    _iPlayerCurrentGas = 0x02F4A710;
                    _iPlayerRace = 0x03FAC3A8;
                    _iPlayerWorkerCount = 0x02F4A608;
                    _iPlayerCurrentSupply = 0x02F4A6D0;
                    _iPlayerMaxSupply = 0x02F4A6B8;
                    _iPlayerArmySizeMinerals = 0x02F4A9F0;
                    _iPlayerArmySizeGas = 0x02F4AA10;
                    _iPlayerIncomeMinerals = 0x02F4A788;
                    _iPlayerIncomeGas = 0x02F4A790;
                    _iPlayerUnitsLost = 0x02F4A4F8;
                    _iPlayerTeam = 0x2EE7550;
                    _iPlayerCurrentApm = 0x2F4A488;
                    _iPlayerAverageApm = (0x26A8F78 + 0x248);
                    _iPlayerTimerData = 0x15D482C;
                    _iPlayerRealCurrentApm = 0x2F4A4C0;
                    _iPlayerCameraX = 0x2F4A0E8;
                    _iPlayerCameraY = 0x2F4A0EC;
                    _iPlayerColor = 0x2F4A184;
                    _iPlayerBattleNetId = 0x23476A8;
                    _iPlayerAmount = 0;
                    _iPlayerOffsetBattleNetId = 0x4B0;
                    _iPlayerOffsetStandart = 0xA68;
                    _iPlayerOffsetTeam = 0x4;
                    _iPlayerOffsetRace = 0x10;
                    break;

                case "1.4.4.22418":
                    _iPlayerBase = 0x26A6F78; //Valid
                    _iPlayerName = _iPlayerBase + 0x48; //Valid
                    _iPlayerNameLenght = _iPlayerBase + 0x40; //Valid
                    _iPlayerCurrentMinerals = _iPlayerBase + 0x4D0; //Valid
                    _iPlayerCurrentGas = _iPlayerBase + 0x4D8; //Valid
                    _iPlayerRace = 0x03255738; //Valid
                    _iPlayerWorkerCount = _iPlayerBase + 0x3F0; //Valid
                    _iPlayerLocal = 0x36DA410; //Not Validated
                    _iPlayerCurrentSupply = _iPlayerBase + 0x4A8; //Valid
                    _iPlayerMaxSupply = _iPlayerBase + 0x4A0; //Valid
                    _iPlayerArmySizeMinerals = _iPlayerBase + 0x7B8; //Valid
                    _iPlayerArmySizeGas = _iPlayerBase + 0x7D8; //Valid
                    _iPlayerIncomeMinerals = _iPlayerBase + 0x550; //Valid
                    _iPlayerIncomeGas = _iPlayerBase + 0x558; //Valid
                    _iPlayerUnitsLost = _iPlayerBase + 680; //Valid
                    _iPlayerTeam = 0; //Outdated
                    _iPlayerCurrentApm = _iPlayerBase + 0x2A8; //Valid
                    _iPlayerAverageApm = (0x26A8F78 + 0x248); //Outdated
                    _iPlayerTimerData = 0x15D482C; //Outdated
                    _iPlayerRealCurrentApm = _iPlayerBase + 0x270; //Valid
                    _iPlayerCameraX = _iPlayerBase + 8; //Valid
                    _iPlayerCameraY = _iPlayerBase + 12; //Valid
                    _iPlayerColor = 0x26A7018; //Valid
                    _iPlayerBattleNetId = 0x016CA8B8; //Outdated - Not used
                    _iPlayerAmount = 0x16E5DD0; //Valid
                    _iPlayerOffsetBattleNetId = 0x4B0; //Outdated - Not used
                    _iPlayerOffsetStandart = 0x910; //Valid
                    _iPlayerOffsetTeam = 0x4; //Outdated
                    _iPlayerOffsetRace = 0x10; //Valid
                    break;
            }

            #endregion

            GetPlayerInformation(iPlayer);
        }

        //Will catch the colors of the players.
        /// <summary>
        /// Catch the color for player x
        /// </summary>
        /// <param name="iPlayer">This means which player has the color</param>
        /// <returns></returns>
        private unsafe Color CatchColors(int iPlayer)
        {
            /*0             ^white              255,255,255,255
             * 1			Red					255,180,20,30
2			Blue				255,0,66,255
3			Teal				255,28,166,233
4			Purple				255,84,0,129
5			Yellow				255,234,224,41
6			Orange				255,253,138,14
7			Green				255,22,128,0
8			Pink				255,228,91,175
9			Violet				255,31,1,200
10			Light Grey			255,82,84,148
11          Dark Green          255,15,97,69
12          Brown               255,78,41,3
13          Light Green         255,149,255,144
14          Drak Gray           255,35,35,35
15          Pink                255,228,91,175
             * 
             * 
             * White,
                Red,
                Blue,
                Teal,
                Purple,
                Yellow,
                Orange,
                Green,
                LightPink,
                Violet,
                LightGray,
                DarkGreen,
                Brown,
                LightGreen,
                DarkGray,
                Pink

             * */


            var strresult = "";

            int iresult2 =
                int.Parse(
                    ReadMemory((void*) (_iPlayerColor + _iPlayerOffsetStandart*iPlayer)).ToString(
                        CultureInfo.InvariantCulture));

            //iresult2 = int.Parse(DoHandle(_iPlayerColor + (_iPlayerOffsetStandart * iPlayer), 2)[0].ToString(CultureInfo.InvariantCulture));


            #region If- loops (Catch ARGB)

            switch (iresult2)
            {
                case 0:
                    strresult = "255,255,255,255";
                    break;
                case 1:
                    strresult = "255,180,20,30";
                    break;
                case 2:
                    strresult = "255,0,66,255";
                    break;
                case 3:
                    strresult = "255,28,166,233";
                    break;
                case 4:
                    strresult = "255,84,0,129";
                    break;
                case 5:
                    strresult = "255,234,224,41";
                    break;
                case 6:
                    strresult = "255,253,138,14";
                    break;
                case 7:
                    strresult = "255,22,128,0";
                    break;
                case 8:
                    strresult = "255,228,91,175";
                    break;
                case 9:
                    strresult = "255,31,1,200";
                    break;
                case 10:
                    strresult = "255,82,84,148";
                    break;
                case 11:
                    strresult = "255,15,97,69";
                    break;
                case 12:
                    strresult = "255,78,41,3";
                    break;
                case 13:
                    strresult = "255,149,255,144";
                    break;
                case 14:
                    strresult = "255,35,35,35";
                    break;
                case 15:
                    strresult = "255,228,91,175";
                    break;

            }

            #endregion




            string[] strendresult = strresult.Split(',');


            var iresult = new int[4];

            iresult[0] = int.Parse(strendresult[0]);
            iresult[1] = int.Parse(strendresult[1]);
            iresult[2] = int.Parse(strendresult[2]);
            iresult[3] = int.Parse(strendresult[3]);


            return Color.FromArgb(iresult[0], iresult[1], iresult[2], iresult[3]);

        }

        private unsafe int CountPlayers()
        {
            //We have to calculate each playername
            //And return the final value
            var ivalue = 0;

            for (int i = 0; i < 32; i++)
            {
                ivalue = _iPlayerBase + 0x48;

                //Build Playername
                //Name of Player
                string strges = "";

                Top_Player:

                //Catch the Unitname and parse it to it's actual name
                uint ibuffer = (uint) ReadMemory((void*) (ivalue + 0x910*i));

                byte[] bt = BitConverter.GetBytes(ibuffer);

                var strUnit = Encoding.UTF8.GetString(bt);
                strges += strUnit;

                if (!strUnit.Contains("\0"))
                {
                    ivalue += 4;
                    goto Top_Player;
                }

                if (strges.StartsWith("\0"))
                    return i;

            }

            return 0;
        }

        //Catch informations from the player
        private unsafe void GetPlayerInformation(int number)
        {
            /* 0    =   Name
             * 1    =   Lenght of Name
             * 2    =   Current Minerals
             * 3    =   Current Gas
             * 4    =   Current Food
             * 5    =   Current Max. Food
             * 6    =   Race
             * 7    =   Worker Amount
             * 8    =   Current Armysize Minerals
             * 9    =   Current Armysize Gas
             * 10   =   Current Income Minerals
             * 11   =   Current Income Gas
             * 12   =   Units Lost
             * 13   =   Color
             * 14   =   Team
             * 15   =   Current APM
             * 16   =   Current AVG APM
             * 17   =   */

            #region Variables

            //Declaring the variables
            string sName = "",
                   sNameLenght = "",
                   sCurrentMinerals = "",
                   sCurrentGas = "",
                   sCurrentFood = "",
                   sCurrentMaxFood = "",
                   sRace = "",
                   sWorkerAmount = "",
                   sCurrentArmysizeMinerals = "",
                   sCurrentArmysizeGas = "",
                   sCurrentIncomeMinerals = "",
                   sCurrentIncomeGas = "",
                   sUnitsLost = "",
                   sColor = "",
                   sTeam = "",
                   sCurrentApm = "",
                   sAverageApm = "",
                   sAmountofPlayers = "",
                   sTimerdata = "",
                   sRealCurrentApm = "",
                   sCameraX = "",
                   sCameraY = "",
                   sBattlenetId = "";

            #endregion

            #region Catch the Information

            //Name of Player
            string strges = "";

            Top1:

            //Catch the Unitname and parse it to it's actual name
            uint ibuffer = (uint) ReadMemory((void*) (_iPlayerName + 0x910*number));

            byte[] bt = BitConverter.GetBytes(ibuffer);

            var strUnit = Encoding.UTF8.GetString(bt);
            strges += strUnit;

            if (!strUnit.Contains("\0"))
            {
                _iPlayerName += 4;
                goto Top1;
            }

            //Final Name
            sName = strges;

            //Lenght of the Name of the Player
            sNameLenght = ReadMemory((void*) (_iPlayerNameLenght + 0x910*number)).ToString(CultureInfo.InvariantCulture);

            //Current Mineral Amount
            sCurrentMinerals =
                ReadMemory((void*) (_iPlayerCurrentMinerals + 0x910*number)).ToString(CultureInfo.InvariantCulture);

            //Current Gas Amount
            sCurrentGas = ReadMemory((void*) (_iPlayerCurrentGas + 0x910*number)).ToString(CultureInfo.InvariantCulture);

            //Current Food of Player
            sCurrentFood =
                (ReadMemory((void*) (_iPlayerCurrentSupply + 0x910*number))/4096).ToString(CultureInfo.InvariantCulture);

            //Current max. Food of Player
            sCurrentMaxFood =
                (ReadMemory((void*) (_iPlayerMaxSupply + 0x910*number))/4096).ToString(CultureInfo.InvariantCulture);

            //Race of Player
            sRace = "";

            var ibuffer2 = ReadMemory((void*) (_iPlayerRace + _iPlayerOffsetRace*number));

            byte[] bt2 = BitConverter.GetBytes(ibuffer2);

            sRace = Encoding.UTF8.GetString(bt2);

            string sresult = sRace;

            if (sresult.Contains("Terr"))
                sRace = "Terran";

            else if (sresult.Contains("Prot"))
                sRace = "Protoss";

            else if (sresult.Contains("Zerg"))
                sRace = "Zerg";

            //Worker Amount of Player
            sWorkerAmount =
                ReadMemory((void*) (_iPlayerWorkerCount + 0x910*number)).ToString(CultureInfo.InvariantCulture);

            //Current Armysize (Minerals) of Player
            sCurrentArmysizeMinerals =
                ReadMemory((void*) (_iPlayerArmySizeMinerals + 0x910*number)).ToString(CultureInfo.InvariantCulture);

            //Current Armysize (Gas) of Player
            sCurrentArmysizeGas =
                ReadMemory((void*) (_iPlayerArmySizeGas + 0x910*number)).ToString(CultureInfo.InvariantCulture);

            //Current Income (Minerals) for Player
            sCurrentIncomeMinerals =
                ReadMemory((void*) (_iPlayerIncomeMinerals + 0x910*number)).ToString(CultureInfo.InvariantCulture);

            //Current Income (Gas) for Player
            sCurrentIncomeGas =
                ReadMemory((void*) (_iPlayerIncomeGas + 0x910*number)).ToString(CultureInfo.InvariantCulture);

            //Amount of Lost Units of Player
            sUnitsLost = ReadMemory((void*) (_iPlayerUnitsLost + 0x910*number)).ToString(CultureInfo.InvariantCulture);

            // The Color of Player
            string str = CatchColors(number).ToString();
            string[] nfd = str.Split(']');
            sColor = nfd[0].Substring(7);
            _cl = CatchColors(number);

            //The Team of the Player
            sTeam = ReadMemory((void*) (_iPlayerTeam + _iPlayerOffsetTeam*number)).ToString(CultureInfo.InvariantCulture);

            //Get the current APM
            sCurrentApm = ReadMemory((void*) (_iPlayerCurrentApm + 0x910*number)).ToString(CultureInfo.InvariantCulture);

            //Get all actions in the game
            sAverageApm = ReadMemory((void*) (_iPlayerAverageApm + 0x910*number)).ToString(CultureInfo.InvariantCulture);

            //Get the amount of players
            if (int.Parse(sCurrentFood) != 0 && int.Parse(sCurrentMaxFood) != 0)
                sAmountofPlayers = ReadMemory((void*) _iPlayerAmount).ToString(CultureInfo.InvariantCulture);

            //Get the value of the timer
            sTimerdata = "0";

            //Get the 'Real' APM (filtered)
            sRealCurrentApm =
                ReadMemory((void*) (_iPlayerRealCurrentApm + 0x910*number)).ToString(CultureInfo.InvariantCulture);

            //Get the xlocation for the camera
            sCameraX = ReadMemory((void*) (_iPlayerCameraX + 0x910*number)).ToString(CultureInfo.InvariantCulture);

            //Getthe ylocation for the camera
            sCameraY = ReadMemory((void*) (_iPlayerCameraY + 0x910*number)).ToString(CultureInfo.InvariantCulture);

            //Getting the BattleNetID
            //sBattlenetId = Encoding.UTF8.GetString(DoHandle(_iPlayerBattleNetId + _iPlayerOffsetBattleNetId*number, 16));

            #endregion

            #region Make the results 'public'

            _sPlayerInfo.Add(sName);
            _sPlayerInfo.Add(sNameLenght);
            _sPlayerInfo.Add(sCurrentMinerals);
            _sPlayerInfo.Add(sCurrentGas);
            _sPlayerInfo.Add(sCurrentFood);
            _sPlayerInfo.Add(sCurrentMaxFood);
            _sPlayerInfo.Add(sRace);
            _sPlayerInfo.Add(sWorkerAmount);
            _sPlayerInfo.Add(sCurrentArmysizeMinerals);
            _sPlayerInfo.Add(sCurrentArmysizeGas);
            _sPlayerInfo.Add(sCurrentIncomeMinerals);
            _sPlayerInfo.Add(sCurrentIncomeGas);
            _sPlayerInfo.Add(sUnitsLost);
            _sPlayerInfo.Add(sColor);
            _sPlayerInfo.Add(sTeam);
            _sPlayerInfo.Add(sCurrentApm);
            _sPlayerInfo.Add(sAverageApm);
            _sPlayerInfo.Add(sAmountofPlayers);
            _sPlayerInfo.Add(sTimerdata);
            _sPlayerInfo.Add(sRealCurrentApm);
            _sPlayerInfo.Add(sCameraX);
            _sPlayerInfo.Add(sCameraY);
            _sPlayerInfo.Add(sBattlenetId);



            #endregion
        }


        public string PlayerName
        {
            get { return _sPlayerInfo[0]; }
        }

        public string PlayerNameLenght
        {
            get { return _sPlayerInfo[1]; }
        }

        public string MineralsCurrent
        {
            get { return _sPlayerInfo[2]; }
        }

        public string GasCurrent
        {
            get { return _sPlayerInfo[3]; }
        }

        public string Food
        {
            get { return _sPlayerInfo[4]; }
        }

        public string FoodMax
        {
            get { return _sPlayerInfo[5]; }
        }

        public string Race
        {
            get { return _sPlayerInfo[6]; }
        }

        public string WorkerAmount
        {
            get { return _sPlayerInfo[7]; }
        }

        public string ArmySizeMinerals
        {
            get { return _sPlayerInfo[8]; }
        }

        public string ArmySizeGas
        {
            get { return _sPlayerInfo[9]; }
        }

        public string IncomeMinerals
        {
            get { return _sPlayerInfo[10]; }
        }

        public string IncomeGas
        {
            get { return _sPlayerInfo[11]; }
        }

        public string UnitsLost
        {
            get { return _sPlayerInfo[12]; }
        }

        public Color PlayerColor
        {
            get { return _cl; }
        }

        public string PlayerTeam
        {
            get { return _sPlayerInfo[14]; }
        }

        public string ApmCurrent //APM with spam
        {
            get { return _sPlayerInfo[15]; }
        }

        public string Actions //unused
        {
            get { return _sPlayerInfo[16]; }
        }

        public unsafe int PlayerAmount
        {
            get { return (byte) ReadMemory((void*) _iPlayerAmount); }
        }

        public int GameTimer
        {
            get { return (int.Parse(_sPlayerInfo[18])/16); }
        }

        public string RealApmCurrent //Filtered apm
        {
            get { return _sPlayerInfo[19]; }
        }

        public int CameraX
        {
            get { return (int.Parse(_sPlayerInfo[20])/4096); }
        }

        public int CameraY
        {
            get { return (int.Parse(_sPlayerInfo[21])/4096); }
        }

        public string BattleNetId
        {
            get { return _sPlayerInfo[22]; }
        }

        public string Sc2Version
        {
            get
            {
                Process proc = Process.GetProcessesByName("SC2")[0];
                return proc.MainModule.FileVersionInfo.FileVersion;
            }
        }

        //public int LocalPlayer
        //{
        //    get
        //    {
        //        Form1 d = new Form1();
        //        if (PlayerName.Contains(d._strNickname))
        //            return (ipl + 1);

        //        return 33;
        //    }
        //}

        public unsafe int LocalPlayer
        {
            get
            {
                XmlRead();



                for (int i = 0; i < PlayerAmount; i++)
                {
                    //Name of Player
                    string strges = "";
                    int iPrePlayerName = 0x26A6FC0; //Playername

                Top2:

                    //Catch the Unitname and parse it to it's actual name
                    uint ibuffer = (uint)ReadMemory((void*)(iPrePlayerName + 0x910 * i));

                    byte[] bt = BitConverter.GetBytes(ibuffer);

                    var strUnit = Encoding.UTF8.GetString(bt);
                    strges += strUnit;

                    if (!strUnit.Contains("\0"))
                    {
                        iPrePlayerName += 4;    //Because max. lenght = 16
                        goto Top2;
                    }

                    if (strges.Contains(NickName))
                        return (i + 1);
                }

                return 33;
            }
        }

        //Stuff for the Localplayer only!!!
        private string NickName = "";
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
                                    case "PlayerNickName":
                                        NickName = xReader.Value;
                                        break;
                                }
                            }
                        }
                    }
                }
            }

            catch 
            {
                new Exception();
            }

            finally
            {
                if (xReader != null)
                    xReader.Close();
            }
        }
    }
}
