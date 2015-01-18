using CoreAudioApi;

namespace RadioCabinet
{

    public class SystemVolumeControl
    {
        private MMDevice device;
        public static float MIN_VAL = (float) 0.0;
        public static float MAX_VAL = (float) 1024.0;

        // Constructor
        public SystemVolumeControl()
        {
            var devEnum = new MMDeviceEnumerator();
            device = devEnum.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia);
        }

        public void SetVolume(int volume)
        {
            device.AudioEndpointVolume.MasterVolumeLevelScalar = ((float)volume / MAX_VAL);
        }
    }
}
