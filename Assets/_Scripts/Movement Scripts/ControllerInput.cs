using System.Runtime.InteropServices;

public class ControllerInput
{
    public enum CONTROLLER_TYPE : ushort
    {
        CONTROLLER,
        GUITAR,
        DRUM,
        UNKNOWN
    }

    public enum CONTROLLER_BUTTON : ushort
    {
        ANY = 0x0000,
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



    ///<summary>
    /// gives trigger values from 0 -> 1
    ///</summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Triggers
    {
        public override string ToString() => "(" + LT + ", " + RT + ")";
        public float LT, RT;
    }

    ///<summary>
    /// gives stick values from -1 -> 1
    ///</summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Stick
    {
        public override string ToString() => "(" + x + ", " + y + ")";
        public float x, y;
    }


    ///<summary>
    ///use as replacement for stick[0]
    ///</summary>    
    ///<example><code> 
    ///Stick sticks[2];
    ///sticks[LS].x;//gets the left stick
    ///</code></example> 
    public const ushort LS = 0;

    ///<summary>
    ///use as replacement for stick[1]
    ///</summary>    
    ///<example><code> 
    ///Stick sticks[2];
    ///sticks[RS].x;//gets the left stick
    ///</code></example> 
    public const ushort RS = 1;


    static private bool updateChange = false;

    private const string DLL = "ControllerDLL.dll";

    [DllImport(DLL)] private static extern void controllerUpdate();

    ///<summary>
    ///updates all controllers
    ///</summary>
    ///<remarks>Must be called before using any other function</remarks>    
    public static void update() => controllerUpdate();


    ///<summary>
    ///checks if the controller from index 0 -> 3 (inclusive) is connected (up to 4 controllers)
    ///</summary>
    [DllImport(DLL)] public static extern bool controllerConnected(int index);

    ///<summary>
    ///gets the type of controller that is connected to the computer 
    ///</summary>
    ///<remarks>Must be compared with CONTROLLER_TYPE enums</remarks>    
    [DllImport(DLL)] public static extern int getControllerType(int index);

    ///<summary>
    ///sets a radial deadzone magnitude. any value below the threshold is zero.
    ///</summary>
    [DllImport(DLL)] public static extern void setStickDeadZone(int index, float dz);

    ///<summary>
    ///sets the vibration to left and right channels (values range from 0 -> 1)
    ///</summary>
    [DllImport(DLL)] public static extern void setVibration(int index, float L, float R);

    ///<summary>
    ///sets the vibration to the left channels (values range from 0 -> 1)
    ///</summary>
    [DllImport(DLL)] public static extern void setVibrationL(int index, float L);

    ///<summary>
    ///sets the vibration to the right channel (values range from 0 -> 1)
    ///</summary>
    [DllImport(DLL)] public static extern void setVibrationR(int index, float R);

    ///<summary>
    ///gets the vibration of left and right channels and populates L & R (values range from 0 -> 1)
    ///</summary>
    [DllImport(DLL)] public static extern void getVibration(int index, ref float L, ref float R);

    ///<summary>
    ///gets the vibration of left and right channels and returns a float array of size 2 (values range from 0 -> 1)
    ///</summary>
    public static float[] getVibration(int index)
    {
        float[] vibe = new float[2];
        if (controllerConnected(index))
            getVibration(index, ref vibe[0], ref vibe[1]);
        return vibe;
    }


    ///<summary>
    ///gets the vibration of left channel and populates L (values range from 0 -> 1)
    ///<summary>
    [DllImport(DLL)] public static extern void getVibrationL(int index, ref float L);

    ///<summary>
    ///gets the vibration of left channel and returns a float (values range from 0 -> 1)
    ///</summary>
    public static float getVibrationL(int index)
    {
        float vibe = 0;
        if (controllerConnected(index))
            getVibrationL(index, ref vibe);
        return vibe;
    }

    ///<summary>
    ///gets the vibration of right channel and populates R (values range from 0 -> 1)
    ///</summary>
    [DllImport(DLL)] public static extern void getVibrationR(int index, ref float R);

    ///<summary>
    ///gets the vibration of right channel and returns a float (values range from 0 -> 1)
    ///</summary>
    public static float getVibrationR(int index)
    {
        float vibe = 0;
        if (controllerConnected(index))
            getVibrationR(index, ref vibe);
        return vibe;
    }

    ///<summary>
    ///stops controller vibrations in left and right channels
    ///</summary>
    [DllImport(DLL)] public static extern void resetVibration(int index);

    ///<summary>
    ///checks if button is being held down. 
    ///<returns>if the button continues to be held this statementis returns true
    ///</returns>
    ///</summary>
    ///<example>
    ///<code>
    ///if(isButtomPressed(0,(int)CONTROLLER_BUTTON.A))
    ///{
    ///   //Do stuff here
    ///}
    ///</code>    
    ///</example>
    [DllImport(DLL)] public static extern bool isButtonPressed(int index, int bitmask);

    ///<summary>
    ///checks if button is not being held down. 
    ///<returns>while the button is released this statementis returns true</returns>
    ///</summary>
    ///<example>
    ///<code>
    ///if(isButtomReleased(0,(int)CONTROLLER_BUTTON.A))
    ///{
    ///   //Do stuff here
    ///}
    ///</code>    
    ///</example>
    [DllImport(DLL)] public static extern bool isButtonReleased(int index, int bitmask);

    ///<summary>
    ///gets the instance when this button is pressed
    ///</summary>
    ///<example>
    ///<code>
    ///if(isButtomDown(0,(int)CONTROLLER_BUTTON.A))
    ///{
    ///   //Do stuff here
    ///}
    ///</code>    
    ///</example>
    [DllImport(DLL)] public static extern bool isButtonDown(int index, int bitmask);

    ///<summary>
    ///gets the instance the button is released
    ///</summary>
    ///<example>
    ///<code>
    ///if(isButtomUp(0,(int)CONTROLLER_BUTTON.A))
    ///{
    ///   //Do stuff here
    ///}
    ///</code>    
    ///</example> 
    [DllImport(DLL)] public static extern bool isButtonUp(int index, int bitmask);


    ///<summary>
    ///gets the stick information for specified controller index and populates a Stick array of size 2
    ///</summary>
    ///<example>
    ///<code>
    ///Stick[] sticks = new Stick[2];
    ///getSticks(0, sticks);
    ///sticks[LS].x//gets the x component of the left stick
    ///</code>    
    ///</example> 
    [DllImport(DLL)] public static extern void getSticks(int index, Stick[] sticks);

    ///<summary>
    ///gets the stick information for specified controller index and returns an array of size 2
    ///</summary>
    ///<example>
    ///<code>
    ///Stick[] sticks = getSticks(0);
    ///sticks[LS].x//gets the x component of the left stick
    ///</code>    
    ///</example> 
    public static Stick[] getSticks(int index)
    {
        Stick[] sticks = new Stick[2];
        if (controllerConnected(index))
            getSticks(index, sticks);
        return sticks;
    }

    ///<summary>
    ///gets the trigger information for specified controller index and populates trigger
    ///</summary>
    ///<example>
    ///<code>
    ///Triggers triggers = new Triggers();
    ///getTriggers(0,triggers);
    ///triggers.LT;//gets the LT compression magnitude
    ///</code>    
    ///</example> 
    [DllImport(DLL)] public static extern void getTriggers(int index, ref Triggers trig);

    ///<summary>
    ///gets the trigger information for specified controller index and returns trigger
    ///</summary>
    ///<example>
    ///<code>
    ///Triggers triggers = getTriggers(0);
    ///triggers.LT;//gets the LT compression magnitude
    ///</code>    
    ///</example>
    public static Triggers getTriggers(int index)
    {
        Triggers triggers = new Triggers();
        if (controllerConnected(index))
            getTriggers(index, ref triggers);
        return triggers;
    }

}