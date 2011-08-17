using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Diagnostics;

namespace AppModXIV
{
    public class AppModXIV
    {

    }

    #region " AUTOMATIC UPDATES "

    public class AutomaticUpdates
    {
        public string currentversion;
        public string fname;

        public void checkDLLs(string dllName, string ver)
        {
            if (dllName == "AppModXIV")
            {
                currentversion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
            else
            {
                currentversion = ver;
            }

            if (checkUpdates(dllName))
            {
                MessageBox.Show(string.Format("{0}.dll is out-dated. Please re-download from the public directory.", dllName));
            }
        }

        public bool checkUpdates(string filename)
        {
            string latestversion = "0.0.0.0";
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(string.Format("https://secure.ffxiv-app.com/appv/{0}.txt", filename));
                HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    StreamReader cu = new StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding("utf-8"));
                    latestversion = cu.ReadLine();
                    cu.Close();
                    response.Close();
                }
            }
            catch (WebException)
            {
                return false;
            }

            int lver = Int32.Parse(latestversion.Replace(".", ""));
            int cver = Int32.Parse(currentversion.Replace(".", ""));

            if (lver > cver)
            {
                return true;
            }
            return false;
        }
    }

    #endregion

    #region " BITMASKING "


    /// <summary>
    /// The BitField class exposes the following methods:
    ///	Initialization:
    ///		BitField()		// Constructor
    ///		ClearField()	// ClearField clears all contents of the Field
    /// 
    /// Operations:
    ///		SetOn()			// Setting the specified flag and leaving all other flags unchanged
    ///		SetOff()		// Unsetting the specified flag and leaving all other flags unchanged.
    ///		SetToggle()		// Toggling the specified flag and leaving all other bits unchanged.
    ///		IsOn			// IsOn checks if the specified flag is set/on in the Field.
    ///		
    ///	Conversion:
    ///		DecimalToFlag	// Convert a decimal value to a Flag FlagsAttribute value.
    ///		ToStringDec()	// Return a string representation of the Field in decimal (base 10) notation
    /// </summary>
    public class BitField
    {
        /// <summary>
        /// The FlagsAttribute indicates that an enumeration can be treated
        /// as a bit field; that is, a mask comprised of a set of flags.
        /// </summary>
        /// <remarks>
        /// - Bit fields can be combined using a bitwise OR operation, whereas enumerated constants cannot.
        ///		This means that the results from bitwise operations are also bit fields
        /// - Bit fields are generally used for lists of elements that might occur in combination,
        ///		whereas enumeration constants are generally used for lists of mutually exclusive elements.
        ///		Therefore, bit fields are designed to be combined to generate
        ///		unnamed values, whereas enumerated constants are not. Languages vary in their use of bit 
        ///		fields compared to enumeration constants.
        /// - The ulong keyword denotes a simple type that stores a 64-bit unsigned integer
        ///		so we can have up to a maximum of 64 unique flags with one enumeration of this type.
        /// </remarks>
        /// <example>
        /// The clear flag is enumerated to zero, and used appropriately can be used clear the Field.
        /// </example>
        [FlagsAttribute]
        public enum Flag : ulong
        {					// Hexidecimal		Decimal		Binary
            Clear = 0x00,	// 0x...0000		0			...00000000000000000
            f1 = 0x01,		// 0x...0001		1			...00000000000000001			
            f2 = f1 << 1,	// 0x...0002		2			...00000000000000010
            f3 = f2 << 1,	// 0x...0004		4			...00000000000000100
            f4 = f3 << 1,	// 0x...0008		8			...00000000000001000
            f5 = f4 << 1,	// 0x...0010		16			...00000000000010000
            f6 = f5 << 1,	// 0x...0020		32			...00000000000100000
            f7 = f6 << 1,	// 0x...0040		64			...00000000001000000
            f8 = f7 << 1,	// 0x...0080		128			...00000000010000000
            f9 = f8 << 1,	// 0x...0100		256			...00000000100000000
            f10 = f9 << 1,	// 0x...0200		512			...00000001000000000
            f11 = f10 << 1,	// 0x...0400		1024		...00000010000000000
            f12 = f11 << 1,	// 0x...0800		2048		...00000100000000000
            f13 = f12 << 1,	// 0x...1000		4096		...00001000000000000
            f14 = f13 << 1,	// 0x...2000		8192		...00010000000000000
            f15 = f14 << 1,	// 0x...4000		16384		...00100000000000000
            f16 = f15 << 1	// 0x...8000		32768		...01000000000000000
        };

        /// <summary>
        /// The Field that will store our 16 flags
        /// </summary>
        private ulong _Mask;

        /// <summary>
        /// Public property SET and GET to access the Field
        /// </summary>
        public ulong Mask
        {
            get
            {
                return _Mask;
            }
            set
            {
                _Mask = value;
            }
        }

        /// <summary>
        /// Contructor
        /// Add all initialization here
        /// </summary>
        public BitField(Flag flg)
        {
            _Mask = (ulong)flg;
        }

        /// <summary>
        /// ClearField clears all contents of the Field
        /// Set all bits to zero using the clear flag
        /// </summary>
        public void ClearField()
        {
            SetField(Flag.Clear);
        }

        /// <summary>
        /// Setting the specified flag(s) and turning all other flags off.
        ///  - Bits that are set to 1 in the flag will be set to one in the Field.
        ///  - Bits that are set to 0 in the flag will be set to zero in the Field. 
        /// </summary>
        /// <param name="flg">The flag to set in Field</param>
        private void SetField(Flag flg)
        {
            Mask = (ulong)flg;
        }

        /// <summary>
        /// Setting the specified flag(s) and leaving all other flags unchanged.
        ///  - Bits that are set to 1 in the flag will be set to one in the Field.
        ///  - Bits that are set to 0 in the flag will be unchanged in the Field. 
        /// </summary>
        /// <example>
        /// OR truth table
        /// 0 | 0 = 0
        /// 1 | 0 = 1
        /// 0 | 1 = 1
        /// 1 | 1 = 1
        /// </example>
        /// <param name="flg">The flag to set in Field</param>
        public void SetOn(Flag flg)
        {
            Mask |= (ulong)flg;
        }

        /// <summary>
        /// Unsetting the specified flag(s) and leaving all other flags unchanged.
        ///  - Bits that are set to 1 in the flag will be set to zero in the Field.
        ///  - Bits that are set to 0 in the flag will be unchanged in the Field. 
        /// </summary>
        /// <example>
        /// AND truth table
        /// 0 & 0 = 0
        /// 1 & 0 = 0
        /// 0 & 1 = 0
        /// 1 & 1 = 1
        /// </example>
        /// <param name="flg">The flag(s) to unset in Field</param>
        public void SetOff(Flag flg)
        {
            Mask &= ~(ulong)flg;
        }

        /// <summary>
        /// Toggling the specified flag(s) and leaving all other bits unchanged.
        ///  - Bits that are set to 1 in the flag will be toggled in the Field. 
        ///  - Bits that are set to 0 in the flag will be unchanged in the Field. 
        /// </summary>
        /// <example>
        /// XOR truth table
        /// 0 ^ 0 = 0
        /// 1 ^ 0 = 1
        /// 0 ^ 1 = 1
        /// 1 ^ 1 = 0
        /// </example>
        /// <param name="flg">The flag to toggle in Field</param>
        public void SetToggle(Flag flg)
        {
            Mask ^= (ulong)flg;
        }

        /// <summary>
        /// AllOn checks if all the specified flags are set/on in the Field.
        /// </summary>
        /// <param name="flg">flag(s) to check</param>
        /// <returns>
        /// true if all flags are set in Field
        /// false otherwise
        /// </returns>
        public bool AllOn(Flag flg)
        {
            return (Mask & (ulong)flg) == (ulong)flg;
        }

        /// <summary>
        /// Convert a decimal value to a Flag FlagsAttribute value.
        /// Method is thread safe
        /// </summary>
        /// <param name="dec">Valid input: dec between 0,64 </param>
        /// <returns></returns>
        public static Flag DecimalToFlag(decimal dec)
        {
            ulong tMsk = 0;
            byte shift;

            shift = (byte)dec;
            if (shift > 0 && shift <= 64)
            {
                tMsk = (ulong)0x01 << (shift - 1);
            }

            return (Flag)tMsk;
        }

        /// <summary>
        /// Return a string representation of the Field in
        /// decimal (base10) notation.
        /// </summary>
        public String ToStringDec()
        {
            return String.Format("{0}", Mask);
        }
    }

    #endregion

    #region " ERROR LOGGING "

    public class ErrorLogging
    {
        public static void LogError(string error, string app, string path)
        {
            string filename = path + "\\" + app + "_BugReport.txt";

            List<string> data = new List<string>();

            if (File.Exists(filename))
            {
                using (StreamReader reader = new StreamReader(filename))
                {
                    string line = null;
                    do
                    {
                        line = reader.ReadLine();
                        data.Add(line);
                    }
                    while (line != null);
                }
            }

            // truncate the file if it's too long
            int writeStart = 0;
            if (data.Count > 500)
                writeStart = data.Count - 500;

            using (StreamWriter stream = new StreamWriter(filename, false))
            {
                for (int i = writeStart; i < data.Count; i++)
                {
                    stream.WriteLine(data[i]);
                }

                stream.Write(error);
            }
        }
    }

    #endregion

    #region " CONTANTS "

    public class Constants
    {
        public static bool bJP = false;



        public static int CraftingStep = 0;
        public static Boolean Logout = false;
        public static Boolean GMTell = false;

        public static uint CHATLOG;
        public static uint CRAFTING;

        public static string strfmt2 = "{0,1} {1,1} {2,1} {3,1} {4,1} {5,1}";
        public static IntPtr pHandle;

        public static int PID;
        public static int Instance = 0;
        public static Process[] FFXIV_PID;
        public static int LogErrors = 0;

        public static Boolean FFXIVOpen = false;
        public static Boolean _isVista = false;

        public static int totErr = 0;

        public const int WM_KEYDOWN = 0x100;
        public const int WM_KEYUP = 0x101;
        public const int WM_CHAR = 0x102;
        public const int WM_PASTE = 0x302;

        public static Dictionary<string, string> LangString = new Dictionary<string, string>();
        public static Dictionary<string, string> xTranLang = new Dictionary<string, string>();
        public static Dictionary<string, string> xColor = new Dictionary<string, string>();
        public static Dictionary<string, string> xPos = new Dictionary<string, string>();
        public static Dictionary<string, string> xMemory = new Dictionary<string, string>();
        public static Dictionary<string, string> xTranslate = new Dictionary<string, string>();
        public static Dictionary<string, string> xErrors = new Dictionary<string, string>();
        public static Dictionary<string, int> xRepairS = new Dictionary<string, int>();
        public static Dictionary<string, int> xCraftTimer = new Dictionary<string, int>();
        public static Dictionary<string, string> xATCodes = new Dictionary<string, string>();
        public static Dictionary<string, string> xTab = new Dictionary<string, string>();
        public static Dictionary<string, string> xSize = new Dictionary<string, string>();

        public static string[] rLanguages = { "Albanian", "Arabic", "Bulgarian", "Catalan", "Simplified", "Traditional", "Croatian", "Czech", "Danish", "Dutch", "English", "Estonian", "Filipino", "Finnish", "French", "Galician", "German", "Greek", "Hebrew", "Hindi", "Hungarian", "Indonesian", "Italian", "Japanese", "Korean", "Latvian", "Lithuanian", "Maltese", "Norwegian", "Polish", "Portuguese", "Romanian", "Russian", "Serbian", "Slovak", "Slovenian", "Spanish", "Swedish", "Thai", "Turkish", "Ukrainian", "Vietnamese" };

        public static string[] CMGeneral = { "0020", "0042", "0046", "0062" };
        public static string[] CMCommon = { "0001", "000D", "0003", "0004", "0002", "000E", "0005", "000F", "0006", "0010", "0007", "0011", "0008", "0012", "0009", "0013", "000A", "0014", "000B", "0015", "000C" };

        public static string[] CMSay = { "0001" };
        public static string[] CMTell = { "000D", "0003" };
        public static string[] CMParty = { "0004" };
        public static string[] CMShout = { "0002" };
        public static string[] CMLS = { "000E", "0005", "000F", "0006", "0010", "0007", "0011", "0008", "0012", "0009", "0013", "000A", "0014", "000B", "0015", "000C" };
    }

    #endregion
}