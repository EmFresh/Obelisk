using System.Runtime.InteropServices;



public class ControllerInput
{
    public enum CONTROLLER_TYPE : uint
    {
        CONTROLLER,
        GUITAR,
        DRUM,
        UNKNOWN
    }

    public enum CONTROLLER_BUTTON : uint
    {
        DPAD_UP = 0x0001,
        DPAD_DOWN = 0x0002,
        DPAD_LEFT = 0x0004,
        DPAD_RIGHT = 0x0008,
        A = 0x1000,
        B = 0x2000,
        X = 0x4000,
        Y = 0x8000,
        LB = 0x0100,
        RB = 0x0200,
        THUMB_LEFT = 0x0040,
        THUMB_RIGHT = 0x0080,
        SELECT = 0x0020,
        START = 0x0010
    }

    public const ushort LS = 0;
    public const ushort RS = 1;

    [StructLayout(LayoutKind.Sequential)]
    public struct Triggers
    {
        public override string ToString() => "(" + LT + ", " + RT + ")";
        public float LT, RT;
    }

    public struct Stick
    {
        public override string ToString() => "(" + x + ", " + y + ")";
        public float x, y;
    }

    // Put your functions here
    private const string DLL = "ControllerDLL.dll";

    [DllImport(DLL)] public static extern void controllerUpdate();

    public static void update() => controllerUpdate();

    [DllImport(DLL)] public static extern bool controllerConnected(int m_index);
    [DllImport(DLL)] public static extern int getControllerType(int m_index);

    [DllImport(DLL)] public static extern void setStickDeadZone(int index, float dz);
    [DllImport(DLL)] public static extern void setVibration(int index, float L, float R);
    [DllImport(DLL)] public static extern void setVibrationL(int index, float L);
    [DllImport(DLL)] public static extern void setVibrationR(int index, float R);

    [DllImport(DLL)] public static extern void getVibration(int index, ref float L, ref float R);
    public float[] getVibration(int index)
    {
        float[] vibe = new float[2];
        getVibration(index, ref vibe[0], ref vibe[1]);
        return vibe;
    }

    [DllImport(DLL)] public static extern void getVibrationL(int index, ref float L);

    public float getVibrationL(int index)
    {
        float vibe = 0;
        getVibrationL(index, ref vibe);
        return vibe;
    }

    [DllImport(DLL)] public static extern void getVibrationR(int index, ref float R);

    public float getVibrationR(int index)
    {
        float vibe = 0;
        getVibrationR(index, ref vibe);
        return vibe;
    }

    [DllImport(DLL)] public static extern void resetVibration(int index);
    [DllImport(DLL)] public static extern bool isButtonPressed(int index, int bitmask);
    [DllImport(DLL)] public static extern bool isButtonReleased(int index, int bitmask);
    [DllImport(DLL)] public static extern bool isButtonStroked(int index, int bitmask);

    [DllImport(DLL)] public static extern void getSticks(int index, Stick[] sticks);
    public static Stick[] getSticks(int index)
    {
        Stick[] sticks = new Stick[2];
        getSticks(index, sticks);
        return sticks;
    }

    [DllImport(DLL)] public static extern void getTriggers(int index, ref Triggers trig);
    public static Triggers getTriggers(int index)
    {
        Triggers triggers = new Triggers();
        getTriggers(index, ref triggers);
        return triggers;

    }

}
