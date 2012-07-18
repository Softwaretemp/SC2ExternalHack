using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using bellaPatricia_Lib;
using System.Diagnostics;

namespace SC2_External_Hack_II
{
    class MapInfo
    {
        


        private readonly ProcessMemoryReader _pReader = new ProcessMemoryReader(Process.GetProcessesByName("SC2")[0]);

        //Constructor for this class, prove the gameversion and adjust the Addresses
        public MapInfo()
        {
            var sc2Proc = Process.GetProcessesByName("SC2")[0];

            switch (sc2Proc.MainModule.FileVersionInfo.FileVersion)
            {
                case "1.4.3.21029":
                    _iMapstruct = 0x1D76428;
                    _iIngame = _iMapstruct + 0x3C;          //If > 0 => InGame
                    _iMapTop = _iMapstruct + 0xE8;
                    _iMapBottom = _iMapstruct + 0xE0;
                    _iMapRight = _iMapstruct + 0xE4;
                    _iMapLeft = _iMapstruct + 0xDC;
                    break;

                case "1.5.0.21995":
                    _iMapstruct = 0x25D1128;
                    _iIngame = _iMapstruct + 0x3C;          //If > 0 => InGame
                    _iMapTop = _iMapstruct + 0xE8;
                    _iMapBottom = _iMapstruct + 0xE0;
                    _iMapRight = _iMapstruct + 0xE4;
                    _iMapLeft = _iMapstruct + 0xDC;
                    break;

                case "1.4.4.22418":
                    _iMapstruct = 0x1D74428;
                    _iIngame = _iMapstruct + 0x3C;          //If > 0 => InGame
                    _iMapTop = _iMapstruct + 0xE8;
                    _iMapBottom = _iMapstruct + 0xE0;
                    _iMapRight = _iMapstruct + 0xE4;
                    _iMapLeft = _iMapstruct + 0xDC;
                    break;
            }
        }

        //Default addresses for patch 1.4.3.21029
        private int _iMapstruct,
                    _iIngame,        
                    _iMapTop,
                    _iMapBottom,
                    _iMapRight,
                    _iMapLeft;

        public bool InGame
        {
            get
            {
                var bInGame = _pReader.ReaduIntMemory((IntPtr)_iIngame) > 0;

                return bInGame;
            }
        }

        public int MapTop
        {
            get { return (_pReader.ReadIntMemory((IntPtr)_iMapTop) / 4096); }
        }

        public  int MapBottom
        {
            get { return (_pReader.ReadIntMemory((IntPtr) _iMapBottom) / 4096); }
        }

        public int MapRight
        {
            get { return (_pReader.ReadIntMemory((IntPtr) _iMapRight) / 4096); }
        }

        public int MapLeft
        {
            get { return (_pReader.ReadIntMemory((IntPtr) _iMapLeft) / 4096); }
        }

        public int PlayAbleWidht
        {
            get { return MapRight - MapLeft; }
        }

        public int PlayAbleHeight
        {
            get { return MapTop - MapBottom; }
        }
    }
}
