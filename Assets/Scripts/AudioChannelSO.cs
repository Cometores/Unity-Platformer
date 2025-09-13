using UI.Buttons;
using UnityEngine;

[CreateAssetMenu(fileName = "Audio", menuName = "ScriptableObjects/AudioChannel")]
public class AudioChannelSO : ScriptableObject
{
    public string channelName;
    public bool IsMuted => volume <= Constants.MUTE_THRESHOLD;
    
    [HideInInspector] public float volume;
    [HideInInspector] public float volumeBeforeMute;

    public float GetSavedVolume()
    {
        volume = PlayerPrefs.GetFloat(channelName, Constants.DEFAULT_VOLUME);
        return volume;
    }
}