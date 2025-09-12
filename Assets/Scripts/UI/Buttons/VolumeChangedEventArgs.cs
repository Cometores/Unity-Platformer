using System;

namespace UI.Buttons
{
    public sealed class VolumeChangedEventArgs : EventArgs
    {
        public float OldVolume { get; }
        public float NewVolume { get; }
        public string MixerName { get; }

        public VolumeChangedEventArgs(float oldVolume, float newVolume, string mixerName)
        {
            OldVolume = oldVolume;
            NewVolume = newVolume;
            MixerName = mixerName;
        }
    }
}