using System.Runtime.InteropServices;

public class AudioPlayer
{
    /*
    *[DEFINE]
    *[
    *    [NAME]
    *    FMOD_TIMEUNIT
    *
    *    [DESCRIPTION]   
    *    List of time types that can be returned by Sound::getLength and used with Channel::setPosition or Channel::getPosition.
    *]
    */
    const uint FMOD_TIMEUNIT_MS = 0x00000001; /* Milliseconds. */
    const uint FMOD_TIMEUNIT_PCM = 0x00000002; /* PCM samples, related to milliseconds * samplerate / 1000. */
    const uint FMOD_TIMEUNIT_PCMBYTES = 0x00000004;  /* Bytes, related to PCM samples * channels * datawidth (ie 16bit = 2 bytes). */
    const uint FMOD_TIMEUNIT_RAWBYTES = 0x00000008; /* Raw file bytes of (compressed) sound data (does not include headers).  Only used by Sound::getLength and Channel::getPosition. */
    const uint FMOD_TIMEUNIT_PCMFRACTION = 0x00000010;  /* Fractions of 1 PCM sample.  Unsigned int range 0 to 0xFFFFFFFF.  Used for sub-sample granularity for DSP purposes. */
    const uint FMOD_TIMEUNIT_MODORDER = 0x00000100; /* MOD/S3M/XM/IT.  Order in a sequenced module format.  Use Sound::getFormat to determine the PCM format being decoded to. */
    const uint FMOD_TIMEUNIT_MODROW = 0x00000200;/* MOD/S3M/XM/IT.  Current row in a sequenced module format.  Cannot use with Channel::setPosition.  Sound::getLength will return the number of rows in the currently playing or seeked to pattern. */
    const uint FMOD_TIMEUNIT_MODPATTERN = 0x00000400;  /* MOD/S3M/XM/IT.  Current pattern in a sequenced module format.  Cannot use with Channel::setPosition.  Sound::getLength will return the number of patterns in the song and Channel::getPosition will return the currently playing pattern. */


    private const string DLLName = "AudioPlayer.dll";

    //*MUST be called before using any other function 
    [DllImport(DLLName)] public static extern void init(int channels = 36);

    /*
	*disables the audio system, closing and clearing all open audio instances.
	*NOTE:
	*init() must be called before any other function calls can be made
	*/
    [DllImport(DLLName)] public static extern void disable();

    //*creates an audio instance that is stored in memory	
    [DllImport(DLLName)] public static extern bool createAudio([MarshalAs(UnmanagedType.LPStr)] string file, [MarshalAs(UnmanagedType.LPStr)] string tag);

    //*creates an audio instance that is read from disk (recommended for large audio files)	
    [DllImport(DLLName)] public static extern bool createAudioStream([MarshalAs(UnmanagedType.LPStr)] string file, [MarshalAs(UnmanagedType.LPStr)] string tag);

    //*plays a single audio channel created by createAudio()/AudioStream()	
    [DllImport(DLLName)]
    public static extern
    void play([MarshalAs(UnmanagedType.LPStr)] string tag, bool loop = false, bool newInstance = false,
        uint from = 0, uint to = 0, uint unit = FMOD_TIMEUNIT_MS);

    //*plays all existing audio channels created by createAudio()/AudioStream()	
    [DllImport(DLLName)] public static extern void playAll(bool loop = false, uint from = 0, uint to = 0, uint unit = FMOD_TIMEUNIT_MS);

    //*pauses an audio channel at specified index.
    [DllImport(DLLName)] public static extern void pause([MarshalAs(UnmanagedType.LPStr)] string tag);

    //*pauses all audio channels	
    [DllImport(DLLName)] public static extern void pauseAll();

    //*stops audio channel at specified index
    [DllImport(DLLName)] public static extern void stop([MarshalAs(UnmanagedType.LPStr)] string tag);

    //*stops all audio channels
    [DllImport(DLLName)] public static extern void stopAll();

    //*mutes audio channel at specified index
    [DllImport(DLLName)] public static extern void mute([MarshalAs(UnmanagedType.LPStr)] string tag);

    //*mutes all audio channels
    [DllImport(DLLName)] public static extern void muteAll();

    /*
	*checks if audio channel at specified index has stopped playing
	*NOTE:
	*audio is not considered off until audio has reached it's end
	*or stop()/stopAll() is called
	*/
    [DllImport(DLLName)] public static extern bool isStoped([MarshalAs(UnmanagedType.LPStr)] string tag);

    /*
	*checks if audio channel at specified index has been paused
	*NOTE:
	*audio is not considered off until audio has reached it's end
	*or pause()/pauseAll() is called
	*/
    [DllImport(DLLName)] public static extern bool isPaused([MarshalAs(UnmanagedType.LPStr)] string tag);

    //
    [DllImport(DLLName)] public static extern uint getPosition([MarshalAs(UnmanagedType.LPStr)] string tag, uint type = FMOD_TIMEUNIT_MS);

    //*gets the amount of audio channels created
    [DllImport(DLLName)] public static extern uint size();

    /*
	*sets audio volume at specified index with normal volume levels ranging from 0 -> 1.
	*NOTE:
	*volume will default to 1.
	*levels below 0 will invert sound.
	*increasing level above the normal level may result in distortion.
	*/
    [DllImport(DLLName)] public static extern void setVolume([MarshalAs(UnmanagedType.LPStr)] string tag, float vol);


    /*
	*sets the maximum volume levels for all audio channels ranging from 0 -> 1.
	*NOTE:
	*levels below 0 will invert sound.
	*increasing level above the normal level may result in distortion.
	*/
    [DllImport(DLLName)] public static extern void setMasterVolume(float vol);

    //*required for certain functionality (i.e. audio cleanup,3D sound...)
    [DllImport(DLLName)] public static extern void update();

    //*Gets all errors up until this function is called
    [DllImport(DLLName)] [return: MarshalAs(UnmanagedType.LPStr)] public static extern string getErrorLog();
}
