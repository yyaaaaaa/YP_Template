using UnityEngine;

namespace YP
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioPlayer : MonoBehaviour
    {
        public enum PlayType
        { Simple, Loop, OneShot };

        private AudioSource _audioSource; public AudioSource audioSource
        {
            get
            {
                _audioSource ??= GetComponent<AudioSource>();
                return _audioSource;
            }
        }

        public void PlayClip(AudioClip clip, PlayType playType)
        {
            if (AudioStreamCash.available)
                audioSource.clip = AudioStreamCash.GetClip(clip.name);
            else audioSource.clip = clip;

            switch (playType)
            {
                case PlayType.Simple:
                    audioSource.loop = false;
                    audioSource.Play();
                    break;

                case PlayType.Loop:
                    audioSource.loop = true;
                    audioSource.Play();
                    break;

                case PlayType.OneShot:
                    audioSource.PlayOneShot(audioSource.clip);
                    break;
            }
        }
    }
}