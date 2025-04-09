using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

public class SoundsDictionary : MonoBehaviour
{
    [SerializeField] List<SoundElement> sounds = new List<SoundElement>();
    private const string pathToAudioFolder = "_GameAssets/Sounds";


    [Button]
    public void LoadSounds()
    {
#if UNITY_EDITOR
        sounds.Clear();
        string folderFullPath = System.IO.Path.Combine("Assets", pathToAudioFolder);

        string[] guids = AssetDatabase.FindAssets("t:AudioClip", new[] {folderFullPath});
        
        foreach (var guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>(assetPath);
            if (clip != null)
            {
                sounds.Add(new SoundElement
                {
                    key = clip.name,
                    clip = clip
                });
            }
        }
        Debug.Log($"Найдено и добавлено {sounds.Count} аудиофайлов из папки: {folderFullPath}");
#endif
    }
    
    public AudioClip FindSound(string key)
    {
        key = key.Split(".")[0];

        return (from sound in sounds where sound.key == key select sound.clip).FirstOrDefault();
    }

    [Serializable]
    public struct SoundElement
    {
        public string key;
        public AudioClip clip;
    }
}