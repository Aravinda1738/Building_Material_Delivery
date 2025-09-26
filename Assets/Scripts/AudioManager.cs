
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private SO_AudioChannel audioChannel;

    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioSource audioSourceBGM;
    [SerializeField]
    private AudioClip uIAudio;
    [SerializeField]
    private AudioClip Bgm;
    [SerializeField]
    private AudioClip pickDrop;


    private void OnEnable()
    {

        if (audioChannel != null)
        {
            audioChannel.onUiClick += PlayUIAudio;
            audioChannel.onPickDrop += PlayPickDropAudio;


        }
        


    }


    private void OnDisable()
    {

        if (audioChannel != null)
        {

            audioChannel.onUiClick -= PlayUIAudio;
            audioChannel.onPickDrop -= PlayPickDropAudio;


        }


    }


    public void PlayUIAudio()
    {
        audioSource.resource = uIAudio;
        audioSource.Play();
    }
    public void PlayPickDropAudio()
    {
        audioSource.resource = pickDrop;
        audioSource.Play();
    }
    
}
