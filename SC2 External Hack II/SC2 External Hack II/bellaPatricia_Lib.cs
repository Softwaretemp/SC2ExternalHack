using System;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using SC2_External_Hack_II.Properties;

namespace bellaPatricia_Lib
{
    /// <summary>
    /// ProcessMemoryReader is a class that enables direct reading a process memory
    /// </summary>
    class ProcessMemoryReaderApi
    {
        // constants information can be found in <winnt.h>
        [Flags]
        public enum ProcessAccessType
        {
            ProcessTerminate = (0x0001),
            ProcessCreateThread = (0x0002),
            ProcessSetSessionid = (0x0004),
            ProcessVmOperation = (0x0008),
            ProcessVmRead = (0x0010),
            ProcessVmWrite = (0x0020),
            ProcessDupHandle = (0x0040),
            ProcessCreateProcess = (0x0080),
            ProcessSetQuota = (0x0100),
            ProcessSetInformation = (0x0200),
            ProcessQueryInformation = (0x0400)
        }

        #region DLLImports

        // function declarations are found in the MSDN and in <winbase.h> 

        //		HANDLE OpenProcess(
        //			DWORD dwDesiredAccess,  // access flag
        //			BOOL bInheritHandle,    // handle inheritance option
        //			DWORD dwProcessId       // process identifier
        //			);
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(UInt32 dwDesiredAccess, Int32 bInheritHandle, UInt32 dwProcessId);

        //		BOOL CloseHandle(
        //			HANDLE hObject   // handle to object
        //			);
        [DllImport("kernel32.dll")]
        public static extern Int32 CloseHandle(IntPtr hObject);

        //		BOOL ReadProcessMemory(
        //			HANDLE hProcess,              // handle to the process
        //			LPCVOID lpBaseAddress,        // base of memory area
        //			LPVOID lpBuffer,              // data buffer
        //			SIZE_T nSize,                 // number of bytes to read
        //			SIZE_T * lpNumberOfBytesRead  // number of bytes read
        //			);
        [DllImport("kernel32.dll")]
        public static extern Int32 ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [In, Out] byte[] buffer, UInt32 size, out IntPtr lpNumberOfBytesRead);

        //		BOOL WriteProcessMemory(
        //			HANDLE hProcess,                // handle to process
        //			LPVOID lpBaseAddress,           // base of memory area
        //			LPCVOID lpBuffer,               // data buffer
        //			SIZE_T nSize,                   // count of bytes to write
        //			SIZE_T * lpNumberOfBytesWritten // count of bytes written
        //			);
        [DllImport("kernel32.dll")]
        public static extern Int32 WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [In, Out] byte[] buffer, UInt32 size, out IntPtr lpNumberOfBytesWritten);

        #endregion
    }

    public class ProcessMemoryReader
    {
        /// <summary>	
        /// Process from which to read		
        /// </summary>
        /// 
        public ProcessMemoryReader(Process mProcess)
        {
            _mReadProcess = mProcess;
        }

        public Process ReadProcess
        {
            get
            {
                return _mReadProcess;
            }
            set
            {
                _mReadProcess = value;
            }
        }
        private Process _mReadProcess;
        private IntPtr _mHProcess = IntPtr.Zero;

        private byte[] ReadMem(IntPtr ioffset, uint iLenght)
        {
            var psc2 = _mReadProcess;

            ReadProcess = psc2;

            OpenProcess();


            int bytesread;

            byte[] buffer = ReadProcessMemory(ioffset, iLenght, out bytesread);
            CloseHandle();

            return buffer;
        }

        public void OpenProcess()
        {
            //			m_hProcess = ProcessMemoryReaderApi.OpenProcess(ProcessMemoryReaderApi.PROCESS_VM_READ, 1, (uint)m_ReadProcess.Id);
            const ProcessMemoryReaderApi.ProcessAccessType access = ProcessMemoryReaderApi.ProcessAccessType.ProcessVmRead
                                             | ProcessMemoryReaderApi.ProcessAccessType.ProcessVmWrite
                                             | ProcessMemoryReaderApi.ProcessAccessType.ProcessVmOperation;

            //  access = ProcessMemoryReaderApi.ProcessAccessType.PROCESS_VM_OPERATION;
            _mHProcess = ProcessMemoryReaderApi.OpenProcess((uint)access, 1, (uint)_mReadProcess.Id);
        }

        public void CloseHandle()
        {
            int iRetValue = ProcessMemoryReaderApi.CloseHandle(_mHProcess);
            if (iRetValue == 0)
                //Chill YOUR FUCKING LIFE
            {}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryAddress">The address where you value is stored</param>
        /// <param name="bytesToRead">The amount of bytes you want to read</param>
        /// <param name="bytesRead">out Integer</param>
        /// <returns></returns>
        public byte[] ReadProcessMemory(IntPtr memoryAddress, uint bytesToRead, out int bytesRead)
        {
            var buffer = new byte[bytesToRead];

            IntPtr ptrBytesRead;
            ProcessMemoryReaderApi.ReadProcessMemory(_mHProcess, memoryAddress, buffer, bytesToRead, out ptrBytesRead);

            bytesRead = ptrBytesRead.ToInt32();

            return buffer;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryAddress">The address where you want to write your value</param>
        /// <param name="bytesToWrite">The bytearray of your value</param>
        /// <param name="bytesWritten">out Byte</param>
        public void WriteProcessMemory(IntPtr memoryAddress, byte[] bytesToWrite, out int bytesWritten)
        {
            IntPtr ptrBytesWritten;
            ProcessMemoryReaderApi.WriteProcessMemory(_mHProcess, memoryAddress, bytesToWrite, (uint)bytesToWrite.Length, out ptrBytesWritten);

            bytesWritten = ptrBytesWritten.ToInt32();
        }

        //Read short
        public short ReadShortMemory(IntPtr memoryAddress)
        {
            return BitConverter.ToInt16(ReadMem(memoryAddress, 5), 0);
        }

        //Read ushort
        public ushort ReaduShortMemory(IntPtr memoryAddress)
        {
            return BitConverter.ToUInt16(ReadMem(memoryAddress, 5), 0);
        }

        //Read int
        public int ReadIntMemory(IntPtr memoryAddress)
        {
            return BitConverter.ToInt32(ReadMem(memoryAddress, 10), 0);
        }

        //Read uint
        public uint ReaduIntMemory(IntPtr memoryAddress)
        {
            return BitConverter.ToUInt32(ReadMem(memoryAddress, 10), 0);
        }

        //Read long 
        public long ReadLongMemory(IntPtr memoryAddress)
        {
            return BitConverter.ToInt64(ReadMem(memoryAddress, 19), 0);
        }

        //Read ulong 
        public ulong ReaduLongMemory(IntPtr memoryAddress)
        {
            return BitConverter.ToUInt64(ReadMem(memoryAddress, 20), 0);
        }

        //Read UTF-8
        public string ReadUtf8(IntPtr memoryAddress, uint lenght)
        {
           return Encoding.UTF8.GetString(ReadMem(memoryAddress, lenght));
        }
    }

    //Processview (selfmade)
    class ProcessView
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();


        private readonly string _myProc;
        /// <summary>
        /// ProcessView: This is the name of your process that you want to proof.
        /// </summary>
        /// <param name="processname"></param>
        public ProcessView(string processname)
        {
            var aviable = false;
            var proc = Process.GetProcesses();
            foreach (var t in proc.Where(t => t.ProcessName == processname))
            {
                aviable = true;
            }

            if (aviable)
                _myProc = processname;

            else
                MessageBox.Show(Resources.ProcessView_ProcessView_No_SC2_found_);
        }

        //Check if process myProc is aviable...
        public bool ProcessAviable()
        {
            var proc = Process.GetProcesses();

            return proc.Any(t => t.ProcessName == _myProc);
        }

        //if process if found, check if it's in foreground
        public bool ProcessInForeground()
        {
            if (ProcessAviable())
            {
                var byName = Process.GetProcessesByName(_myProc);
                var activeWindow = GetForegroundWindow();

                return byName.Any(proc => proc.MainWindowHandle == activeWindow);
            }
            return false;
        }
    }

    //Pointercatch (selfmade)
    class PointerScan
    {
        readonly ProcessMemoryReader _pReader = new ProcessMemoryReader(Process.GetProcessesByName("SC2")[0]);
        readonly Process[] _myProcesses;

        //Catch the process' name
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strprocessname">The name of your process</param>
        public PointerScan(string strprocessname)
        {
            _myProcesses = Process.GetProcessesByName(strprocessname);
            _pReader.ReadProcess = _myProcesses[0];
        }


        private int Pointer(int iaddress, int ioffset)
        {
            /*Open pr to read process*/
            _pReader.OpenProcess();

            int bytesReaded;

            var memory = _pReader.ReadProcessMemory((IntPtr)iaddress, 8, out bytesReaded);
            _pReader.CloseHandle();


            //Memory contains the value, we gotta convert it to string
            var strresult = "0x" +
                               Convert.ToString(BitConverter.ToUInt32(memory, 0), 16).ToString(
                                   CultureInfo.InvariantCulture).ToUpper();

            //Now we convert the Hexadecimal to int and we can do our procedure again!
            int isecondoffset = Convert.ToInt32(strresult, 16);
            isecondoffset += ioffset;

            return isecondoffset;
        }

        //Single pointer
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iaddress">Your static address</param>
        /// <param name="ioffset">Your first offset</param>
        /// <returns></returns>
        public int PointerOffset(int iaddress, int ioffset)
        {
            return Pointer(iaddress, ioffset);
        }

        //Second pointer
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iaddress">Your static address</param>
        /// <param name="ioffset">Your first offset</param>
        /// <param name="ioffset2">Your second offset</param>
        /// <returns></returns>
        public int PointerOffset(int iaddress, int ioffset, int ioffset2)
        {
            return PointerOffset(Pointer(iaddress, ioffset2), ioffset);
        }

        //Thirs Pointer
        public int Pointer_(int iaddress, int ioffset, int ioffset2, int ioffset3)
        {
            return PointerOffset(PointerOffset(Pointer(iaddress, ioffset3), ioffset2), ioffset);
        }

        //Forth Pointer
        public int PointerOffset(int iaddress, int ioffset, int ioffset2, int ioffset3, int ioffset4)
        {
            return PointerOffset(PointerOffset(PointerOffset(Pointer(iaddress, ioffset4), ioffset3), ioffset2), ioffset);
        }

        //Fifth Pointer
        public int PointerOffset(int iaddress, int ioffset, int ioffset2, int ioffset3, int ioffset4, int ioffset5)
        {
            return PointerOffset(PointerOffset(PointerOffset(PointerOffset(Pointer(iaddress, ioffset5), ioffset4), ioffset3), ioffset2), ioffset);
        }

        //Sixth Pointer
        public int PointerOffset(int iaddress, int ioffset, int ioffset2, int ioffset3, int ioffset4, int ioffset5, int ioffset6)
        {
            return PointerOffset(PointerOffset(PointerOffset(PointerOffset(PointerOffset(Pointer(iaddress, ioffset6), ioffset5), ioffset4), ioffset3), ioffset2), ioffset);
        }

        //Seventh Pointer
        public int PointerOffset(int iaddress, int ioffset, int ioffset2, int ioffset3, int ioffset4, int ioffset5, int ioffset6, int ioffset7)
        {
            return PointerOffset(PointerOffset(PointerOffset(PointerOffset(PointerOffset(PointerOffset(Pointer(iaddress, ioffset7), ioffset6), ioffset5), ioffset4), ioffset3), ioffset2), ioffset);
        }

        //Eight Pointer
        public int PointerOffset(int iaddress, int ioffset, int ioffset2, int ioffset3, int ioffset4, int ioffset5, int ioffset6, int ioffset7, int ioffset8)
        {
            return PointerOffset(PointerOffset(PointerOffset(PointerOffset(PointerOffset(PointerOffset(PointerOffset(Pointer(iaddress, ioffset8), ioffset7), ioffset6), ioffset5), ioffset4), ioffset3), ioffset2), ioffset);
        } 
    }

    //Signature Scan
    public class SigScan
    {
        /// <summary>
        /// ReadProcessMemory
        /// 
        ///     API import definition for ReadProcessMemory.
        /// </summary>
        /// <param name="hProcess">Handle to the process we want to read from.</param>
        /// <param name="lpBaseAddress">The base address to start reading from.</param>
        /// <param name="lpBuffer">The return buffer to write the read data to.</param>
        /// <param name="dwSize">The size of data we wish to read.</param>
        /// <param name="lpNumberOfBytesRead">The number of bytes successfully read.</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool ReadProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            [Out()] byte[] lpBuffer,
            int dwSize,
            out int lpNumberOfBytesRead
            );

        /// <summary>
        /// m_vDumpedRegion
        /// 
        ///     The memory dumped from the external process.
        /// </summary>
        private byte[] _mVDumpedRegion;

        /// <summary>
        /// m_vProcess
        /// 
        ///     The process we want to read the memory of.
        /// </summary>
        private Process _mVProcess;

        /// <summary>
        /// m_vAddress
        /// 
        ///     The starting address we want to begin reading at.
        /// </summary>
        private IntPtr _mVAddress;

        /// <summary>
        /// m_vSize
        /// 
        ///     The number of bytes we wish to read from the process.
        /// </summary>
        private Int32 _mVSize;


        #region "sigScan Class Construction"
        /// <summary>
        /// SigScan
        /// 
        ///     Main class constructor that uses no params. 
        ///     Simply initializes the class properties and 
        ///     expects the user to set them later.
        /// </summary>
        public SigScan()
        {
            _mVProcess = null;
            _mVAddress = IntPtr.Zero;
            _mVSize = 0;
            _mVDumpedRegion = null;
        }
        /// <summary>
        /// SigScan
        /// 
        ///     Overloaded class constructor that sets the class
        ///     properties during construction.
        /// </summary>
        /// <param name="proc">The process to dump the memory from.</param>
        /// <param name="addr">The started address to begin the dump.</param>
        /// <param name="size">The size of the dump.</param>
        public SigScan(Process proc, IntPtr addr, int size)
        {
            _mVProcess = proc;
            _mVAddress = addr;
            _mVSize = size;
        }
        #endregion

        #region "sigScan Class Private Methods"
        /// <summary>
        /// DumpMemory
        /// 
        ///     Internal memory dump function that uses the set class
        ///     properties to dump a memory region.
        /// </summary>
        /// <returns>Boolean based on RPM results and valid properties.</returns>
        private bool DumpMemory()
        {
            try
            {
                // Checks to ensure we have valid data.
                if (this._mVProcess == null)
                    return false;
                if (this._mVProcess.HasExited == true)
                    return false;
                if (this._mVAddress == IntPtr.Zero)
                    return false;
                if (this._mVSize == 0)
                    return false;

                // Create the region space to dump into.
                this._mVDumpedRegion = new byte[this._mVSize];

                int nBytesRead;

                // Dump the memory.
                bool bReturn = ReadProcessMemory(
                    this._mVProcess.Handle, this._mVAddress, this._mVDumpedRegion, this._mVSize, out nBytesRead
                    );

                // Validation checks.
                if (bReturn == false || nBytesRead != this._mVSize)
                    return false;
                return true;
            }

            catch 
            {
                return false;
            }
        }

        /// <summary>
        /// MaskCheck
        /// 
        ///     Compares the current pattern byte to the current memory dump
        ///     byte to check for a match. Uses wildcards to skip bytes that
        ///     are deemed unneeded in the compares.
        /// </summary>
        /// <param name="nOffset">Offset in the dump to start at.</param>
        /// <param name="btPattern">Pattern to scan for.</param>
        /// <param name="strMask">Mask to compare against.</param>
        /// <returns>Boolean depending on if the pattern was found.</returns>
        private bool MaskCheck(int nOffset, byte[] btPattern, string strMask)
        {
            // Loop the pattern and compare to the mask and dump.
            return !btPattern.Where((t, x) => strMask[x] != '?' && ((strMask[x] == 'x') && (t != this._mVDumpedRegion[nOffset + x]))).Any();

            // The loop was successful so we found the pattern.
        }

        #endregion

        #region "sigScan Class Public Methods"
        /// <summary>
        /// FindPattern
        /// 
        ///     Attempts to locate the given pattern inside the dumped memory region
        ///     compared against the given mask. If the pattern is found, the offset
        ///     is added to the located address and returned to the user.
        /// </summary>
        /// <param name="btPattern">Byte pattern to look for in the dumped region.</param>
        /// <param name="strMask">The mask string to compare against.</param>
        /// <param name="nOffset">The offset added to the result address.</param>
        /// <returns>IntPtr - zero if not found, address if found.</returns>
        public IntPtr FindPattern(byte[] btPattern, string strMask, int nOffset)
        {
            try
            {
                // Dump the memory region if we have not dumped it yet.
                if (this._mVDumpedRegion == null || this._mVDumpedRegion.Length == 0)
                {
                    if (!this.DumpMemory())
                        return IntPtr.Zero;
                }

                // Ensure the mask and pattern lengths match.
                if (strMask.Length != btPattern.Length)
                    return IntPtr.Zero;

                // Loop the region and look for the pattern.
                for (int x = 0; x < this._mVDumpedRegion.Length; x++)
                {
                    if (this.MaskCheck(x, btPattern, strMask))
                    {
                        // The pattern was found, return it.
                        return new IntPtr((int)this._mVAddress + (x + nOffset));
                    }
                }

                // Pattern was not found.
                return IntPtr.Zero;
            }

            catch 
            {
                return IntPtr.Zero;
            }
        }

        /// <summary>
        /// ResetRegion
        /// 
        ///     Resets the memory dump array to nothing to allow
        ///     the class to redump the memory.
        /// </summary>
        public void ResetRegion()
        {
            this._mVDumpedRegion = null;
        }
        #endregion

        #region "sigScan Class Properties"
        public Process Process
        {
            get { return this._mVProcess; }
            set { this._mVProcess = value; }
        }
        public IntPtr Address
        {
            get { return this._mVAddress; }
            set { this._mVAddress = value; }
        }
        public Int32 Size
        {
            get { return this._mVSize; }
            set { this._mVSize = value; }
        }
        #endregion

    }
}