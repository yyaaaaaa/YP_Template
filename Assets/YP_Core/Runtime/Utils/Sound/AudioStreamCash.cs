using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace YP
{
    public class AudioStreamCash : Initializable
    {
        public static bool available => !Environment.editor;

        private static Dictionary<string, AudioClip> cashedClips = new Dictionary<string, AudioClip>();

        [SerializeField] private List<string> _cashedClipNames;

        private int _loadedClips = 0;

        public override void Initialize()
        {
            if (available) LoadAllClips();
            else InitCompleted();
        }

        protected override void OnInitialized()
        { }

        public static AudioClip GetClip(string name) => cashedClips[name + ".mp3"];

        public void LoadAllClips()
        {
            cashedClips.Clear();
            _loadedClips = 0;

            foreach (var clipName in _cashedClipNames)
                StartCoroutine(LoadClip(clipName));

            InitCompleted();
        }

        private IEnumerator LoadClip(string name)
        {
            string url = Application.streamingAssetsPath + "/" + name;

            UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG);
            request.SendWebRequest();
            yield return new WaitUntil(() => request.isDone);

            AudioClip audioClip = DownloadHandlerAudioClip.GetContent(request);

            cashedClips.Add(name, audioClip);
            _loadedClips++;

            if (_loadedClips == _cashedClipNames.Count) yield break;
        }

        [Button("Load cash names")]
        private void LoadCashNames()
        {
            _cashedClipNames = new List<string>();

            DirectoryInfo directory = new DirectoryInfo(Application.streamingAssetsPath);
            SearchFilesInsideDirectory(directory);
        }

        private void SearchFilesInsideDirectory(DirectoryInfo directory)
        {
            FileInfo[] info = directory.GetFiles("*.mp3");
            foreach (var item in info) _cashedClipNames.Add(item.Name);

            foreach (var insideDirectory in directory.GetDirectories())
                SearchFilesInsideDirectory(insideDirectory);
        }

        public AudioClip FindSound(string key)
        {
            key = key.Split(".")[0];

            foreach (var sound in cashedClips)
            {
                if (sound.Key == key) return sound.Value;
            }

            Debug.LogError("No such sound or key");
            return null;
        }
    }
}