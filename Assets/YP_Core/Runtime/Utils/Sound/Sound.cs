using System.Collections;
using UnityEngine;
using VG;
using YP;

public class Sound : MonoBehaviour
{
    private void Awake()
    {
        instance = this;
    }

    private static Sound instance;
    [SerializeField] private AudioStreamCash audioStreamCash;
    [SerializeField] private SoundsDictionary sounds;
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

        var clip = instance.sounds.FindSound(key);
    }

    public static void PlayMusic(string key, bool loop)
    {
        if (instance._music.audioSource.volume == 0f) return;

        AudioClip clip = instance.sounds.FindSound(key);
        instance._music.PlayClip(clip, loop ? AudioPlayer.PlayType.Loop : AudioPlayer.PlayType.Simple);
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
        if (instance._sfx.audioSource.volume == 0f) return;

        AudioClip clip = instance.sounds.FindSound(key);
        
        //Sound volume controls
        
        instance._sfx.PlayClip(clip, AudioPlayer.PlayType.OneShot);
    }
}