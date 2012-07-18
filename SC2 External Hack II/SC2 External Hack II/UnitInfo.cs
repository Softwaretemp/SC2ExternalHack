using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using bellaPatricia_Lib;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SC2_External_Hack_II
{
    internal class UnitInfo
    {
        [DllImport("ReadMemory.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe int ReadMemory(void* address);

        [DllImport("ReadMemory.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe double ReadMemoryDouble(void* address);

        public UnitInfo(int unit)
        {
            _iUnit = unit;

            var sc2Proc = Process.GetProcessesByName("SC2")[0];

            switch (sc2Proc.MainModule.FileVersionInfo.FileVersion)
            {
                case "1.4.3.21029":
                    _UnitStruct = 0x1DE7EC0;
                    _UnitPositionX = _UnitStruct + 0x40;
                    _UnitPositionY = _UnitStruct + 0x44;
                    _UnitTargetFilter = _UnitStruct + 0x14;
                    _UnitTotal = _UnitStruct - 0x3C;
                    _UnitDeathType = _UnitStruct + 0x69;
                    _UnitDestinationX = _UnitStruct + 0x74;
                    _UnitDestinationY = _UnitStruct + 0x78;
                    _UnitEnergy = _UnitStruct + 0x10C;
                    _UnitDamageTaken = _UnitStruct + 0x104;
                    _UnitKills = _UnitStruct + 0x32;
                    _UnitMoveSpeed = _UnitStruct + 0xB8;
                    _UnitMoveState = _UnitStruct + 0x22;
                    _UnitOwner = _UnitStruct + 0x35;
                    _UnitState = _UnitStruct + 0x23;
                    _UnitBeeingPuked = _UnitStruct + 0xD4;
                    _UnitModel = _UnitStruct + 8;
                    break;

                case "1.5.0.21995":
                    _UnitStruct = 0x25D5284;
                    _UnitPositionX = _UnitStruct + 0x40;
                    _UnitPositionY = _UnitStruct + 0x44;
                    _UnitTargetFilter = _UnitStruct + 0x14;
                    _UnitTotal = _UnitStruct - 0x3C;
                    _UnitDeathType = _UnitStruct + 0x69;
                    _UnitDestinationX = _UnitStruct + 0x74;
                    _UnitDestinationY = _UnitStruct + 0x78;
                    _UnitEnergy = _UnitStruct + 0x10C;
                    _UnitDamageTaken = _UnitStruct + 0x104;
                    _UnitKills = _UnitStruct + 0x32;
                    _UnitMoveSpeed = _UnitStruct + 0xB8;
                    _UnitMoveState = _UnitStruct + 0x22;
                    _UnitOwner = _UnitStruct + 0x27;
                    _UnitState = _UnitStruct + 0x23;
                    _UnitBeeingPuked = _UnitStruct + 0xD4;
                    _UnitModel = _UnitStruct + 8;
                    break;

                case "1.4.4.22418":
                    _UnitStruct = 0x1DE5EC0;
                    _UnitPositionX = _UnitStruct + 0x40;
                    _UnitPositionY = _UnitStruct + 0x44;
                    _UnitTargetFilter = _UnitStruct + 0x14;
                    _UnitTotal = _UnitStruct - 0x3C;
                    _UnitDeathType = _UnitStruct + 0x69;
                    _UnitDestinationX = _UnitStruct + 0x74;
                    _UnitDestinationY = _UnitStruct + 0x78;
                    _UnitEnergy = _UnitStruct + 0x10C;
                    _UnitDamageTaken = _UnitStruct + 0x104;
                    _UnitKills = _UnitStruct + 0x32;
                    _UnitMoveSpeed = _UnitStruct + 0xB8;
                    _UnitMoveState = _UnitStruct + 0x22;
                    _UnitOwner = _UnitStruct + 0x27;
                    _UnitState = _UnitStruct + 0x23;
                    _UnitBeeingPuked = _UnitStruct + 0xD4;
                    _UnitModel = _UnitStruct + 8;
                    break;
            }
        }

        private readonly int _iUnit;

        private int _UnitStruct,
                    _UnitPositionX,
                    _UnitPositionY,
                    _UnitTargetFilter,
                    _UnitTotal,
                    _UnitDeathType,
                    _UnitDestinationX,
                    _UnitDestinationY,
                    _UnitEnergy,
                    _UnitDamageTaken,
                    _UnitKills,
                    _UnitMoveSpeed,
                    _UnitMoveState,
                    _UnitOwner,
                    _UnitState,
                    _UnitBeeingPuked,
                    _UnitModel;


        public unsafe int PositionX
        {
            get { return ReadMemory((void*) (_UnitPositionX + 0x1C0*_iUnit))/4096; }
        }

        public unsafe int PositionY
        {
            get { return ReadMemory((void*) (_UnitPositionY + 0x1C0*_iUnit))/4096; }
        }

        public unsafe int DestinationX
        {
            get { return ReadMemory((void*) (_UnitDestinationX + 0x1C0*_iUnit))/4096; }
        }

        public unsafe int DestinationY
        {
            get { return ReadMemory((void*) (_UnitDestinationY + 0x1C0*_iUnit))/4096; }
        }

        public unsafe ulong TagetFilterFlag
        {
            get { return (ulong) ReadMemory((void*) (_UnitTargetFilter + 0x1C0*_iUnit)); }
        }

        public unsafe int TotalUnits
        {
            get { return ReadMemory((void*) _UnitTotal); }
        }

        public unsafe int Owner
        {
            get { return ReadMemory((void*) (_UnitOwner + 0x1C0*_iUnit)); }
        }

        public unsafe int State
        {
            get { return ReadMemory((void*) (_UnitState + 0x1C0*_iUnit)); }
        }

        public unsafe string UnitName
        {
            get
            {
                var iUnitModel = ReadMemory((void*) (_UnitModel + 0x1C0*_iUnit));
                var strUnitModel = Convert.ToString(iUnitModel, 16);

                var iUnitModelPreShift = Convert.ToInt32(strUnitModel, 16);

                //Bitshift the Address
                var iUnitBitShifted = iUnitModelPreShift << 5;
                var iNameLenghtAddress = iUnitBitShifted + 0x7B8;


                var iNameLenghtActual = ReadMemory((void*) iNameLenghtAddress);
                var iNameDataActual = iNameLenghtActual + 36;

                //Lenght of the Name 
                var iNameLenghtJo = ReadMemory((void*) iNameLenghtActual);

                string strges = "";

                Top1:

                //Catch the Unitname and parse it to it's actual name
                var ibuffer = ReadMemory((void*) iNameDataActual);


                byte[] bt = BitConverter.GetBytes(ibuffer);

                var strUnit = Encoding.UTF8.GetString(bt);
                strges += strUnit;

                if (!strUnit.Contains("\0"))
                {
                    iNameDataActual += 4;
                    goto Top1;
                }


                return strges.Substring(10);
            }
        }

        public unsafe float Size
        {
            get
            {
                var iUnitModel = ReadMemory((void*) (_UnitModel + 0x1C0*_iUnit));
                var strUnitModel = Convert.ToString(iUnitModel, 16);

                var iUnitModelPreShift = Convert.ToInt32(strUnitModel, 16);

                //Bitshift the Address
                var iUnitBitShifted = iUnitModelPreShift << 5;
                var iSize = iUnitBitShifted + 0x3B4;

                return (ReadMemory((void*) iSize)/4096) + 1.5f;
            }
        }

        //Not working
        public unsafe Color Stance
        {
            //Detect Attackmove, Move and Stop
            get
            {
                var iUnitModel = ReadMemory((void*) (_UnitModel + 0x1C0*_iUnit));
                var strUnitModel = Convert.ToString(iUnitModel, 16);

                var iUnitModelPreShift = Convert.ToInt32(strUnitModel, 16);

                //Bitshift the Address
                var iUnitBitShifted = iUnitModelPreShift << 5;
                var iStance = iUnitBitShifted + 0x31F10;

                var result = ReadMemory((void*) iStance);

                var clStance = Color.Green;
                //0 = Move
                //1 = Attackmove
                //2 = Stop

                if (result <= 701757136)
                    clStance = Color.Green;

                else if (result <= 701753456)
                    clStance = Color.Red;



                //switch (result)
                //{
                //        //Move
                //    case 701757136:
                //        clStance = Color.Green;
                //        break;

                //        //AttackMove
                //    case 701753456:
                //        clStance = Color.Red;
                //        break;

                //        //Stop
                //    case 789963496:
                //        clStance = Color.Transparent;
                //        break;

                //}
                return clStance;
            }
        }

        public unsafe int UnitEnergy
        {
            get { return (ReadMemory((void*) (_UnitEnergy + 0x1C0*_iUnit))/4096); }
        }

        public unsafe short UnitPuked
        {
            get { return (short) (ReadMemory((void*) (_UnitBeeingPuked + 0x1C0*_iUnit))/4096); }
        }
    }
}
