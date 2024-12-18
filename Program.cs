using System.Runtime.InteropServices;

internal class Program
{
    static void Main(string[] args)
    {
        // Create a DEVMODE structure to hold the display settings
        DEVMODE devMode = new DEVMODE();
        devMode.dmSize = (short)Marshal.SizeOf(typeof(DEVMODE));

        // Get the current display settings
        EnumDisplaySettings(null, -1, ref devMode);

        if (args.Length == 0)
        {
            Console.WriteLine($"Current Resolution is: {devMode.dmPelsWidth}x{devMode.dmPelsHeight}");
            Console.WriteLine("Use parameter -FHD, -HD+, -HD, or -CUSTOM to switch.");
            return;
        }

        // Change the display resolution settings
        switch (args[0].ToUpper())
        {
            case "FHD":
                devMode.dmPelsWidth = 1920;
                devMode.dmPelsHeight = 1080;
                devMode.dmPositionX = 0;  // Reset position
                break;
            case "HD+":
                devMode.dmPelsWidth = 1600;
                devMode.dmPelsHeight = 900;
                devMode.dmPositionX = 0;  // Reset position
                break;
            case "HD":
                devMode.dmPelsWidth = 1280;
                devMode.dmPelsHeight = 720;
                devMode.dmPositionX = 0;  // Reset position
                break;
            case "CUSTOM":
                devMode.dmPelsWidth = 1280;
                devMode.dmPelsHeight = 1024;
                devMode.dmPositionX = 0;  // Start from the left side
                break;
            default:
                Console.WriteLine($"Current Resolution is: {devMode.dmPelsWidth}x{devMode.dmPelsHeight}");
                Console.WriteLine("Use parameter -FHD, -HD+, -HD, or -CUSTOM to switch.");
                return;
        }

        // Apply the new settings
        devMode.dmFields = 0x00040000 | 0x00080000 | 0x00100000; // DM_PELSWIDTH | DM_PELSHEIGHT | DM_POSITION
        int result = ChangeDisplaySettings(ref devMode, 0);

        if (result == 0)
        {
            Console.WriteLine("Display resolution changed successfully.");
        }
        else
        {
            Console.WriteLine("Failed to change display resolution.");
        }
    }

    // Import the necessary Windows API functions
    [DllImport("user32.dll")]
    public static extern bool EnumDisplaySettings(string? deviceName, int modeNum, ref DEVMODE devMode);

    [DllImport("user32.dll")]
    public static extern int ChangeDisplaySettings(ref DEVMODE devMode, int flags);

    // Define the DEVMODE structure
    [StructLayout(LayoutKind.Sequential)]
    public struct DEVMODE
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string dmDeviceName;
        public short dmSpecVersion;
        public short dmDriverVersion;
        public short dmSize;
        public short dmDriverExtra;
        public int dmFields;
        public int dmPositionX;
        public int dmPositionY;
        public int dmDisplayOrientation;
        public int dmDisplayFixedOutput;
        public short dmColor;
        public short dmDuplex;
        public short dmYResolution;
        public short dmTTOption;
        public short dmCollate;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string dmFormName;
        public short dmLogPixels;
        public int dmBitsPerPel;
        public int dmPelsWidth;
        public int dmPelsHeight;
        public int dmDisplayFlags;
        public int dmDisplayFrequency;
    }
}
