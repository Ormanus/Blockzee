using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip MoveSound;
    public AudioClip FallSound;
    public AudioClip DeathSound;
    public AudioClip SingleFinishSound;
    public AudioClip FullFinishSound;

    public static AudioManager Instance;
    public AudioManager()
    {
        Instance = this;
    }

    IEnumerator CreateAudioPlayer(AudioClip clip)
    {
        GameObject go = new GameObject("SoundPlayer");
        go.AddComponent<AudioSource>();
        go.GetComponent<AudioSource>().clip = clip;
        go.GetComponent<AudioSource>().Play();

        yield return new WaitForSeconds(clip.length);

        Destroy(go);
    }

    void PlayClipInInstance(AudioClip clip)
    {
        StartCoroutine(CreateAudioPlayer(clip));
    }

    public static void PlayClip(AudioClip clip)
    {
        Instance.PlayClipInInstance(clip);
    }
}
