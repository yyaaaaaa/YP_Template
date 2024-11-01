using System.Collections;
using UnityEngine;
using YP;

public class Sound : MonoBehaviour
{
    private void Awake()
    {
        instance = this;
    }

    private static Sound instance;
    [SerializeField] private AudioStreamCash audioStreamCash;
    [SerializeField] private AudioPlayer _music; public static AudioPlayer music => instance._music;
    [SerializeField] private AudioPlayer _sfx; public static AudioPlayer sfx => instance._sfx;

    private static bool musicClipExists => instance != null
                                           && instance._music != null
                                           && instance._music.audioSource != null
                                           && instance._music.audioSource.clip != null;

    private static IEnumerator waitCash(string key, bool loop)
    {
        while (!instance.audioStreamCash.initialized)
            yield return new WaitForSecondsRealtime(0.2f);

        var clip = instance.audioStreamCash.FindSound(key);
        instance._music.PlayClip(clip, loop ? AudioPlayer.PlayType.Loop : AudioPlayer.PlayType.Simple);
    }

    public static void PlayMusic(string key, bool loop)
    {
        instance.StartCoroutine(waitCash(key, loop));
    }

    public static void EnableMusic(bool en)
    {
        instance._music.audioSource.mute = !en;
    }

    public static void EnableSound(bool en)
    {
        instance._sfx.audioSource.mute = !en;
    }

    public static void PlaySFX(string key)
    {
        AudioClip clip = instance.audioStreamCash.FindSound(key);
        if (instance._sfx.audioSource.volume == 0f) return;

        instance._sfx.audioSource.volume = 0.4f;
        instance._sfx.PlayClip(clip, AudioPlayer.PlayType.OneShot);
    }
}