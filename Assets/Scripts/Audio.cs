using System.Collections.Generic;
using System.IO;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Audio;

public class Audio : MonoBehaviour
{
    private static Audio Instance;
    
    [SerializeField] private string ImportPath = "Audio";
    [SerializeField] private AudioMixerGroup ImportMixerGroup;

    private void Awake()
    {
        Instance = this;
    }

    private IEnumerable<Transform> EnumMatchingChildren(string name)
    {
        foreach (Transform t in transform)
        {
            if (Utils.FuzzyMatch(t.name, name))
            {
                yield return t;
            }
        }
    }
    
    public static void PlayAt(string name, Vector3 pos)
    {
        if (Instance == null) return;
        var possibleNodes = Instance.EnumMatchingChildren(name).ToList();
        if (possibleNodes.Count == 0) return;
        var group = Utils.PickRandom(possibleNodes);
        var readySources = group.GetComponentsInChildren<AudioSource>()
            .Where(it => it.isPlaying == false)
            .ToList();
        if (readySources.Count > 0)
        {
            var source = Utils.PickRandom(readySources);
            source.transform.position = pos;
            source.Play();
        }
    }

    #if UNITY_EDITOR
    [Button("Import")]
    void Import()
    {
        var importRoot = new GameObject("__IMPORT__");
        importRoot.transform.SetParent(transform);
        Debug.Log(Application.dataPath);
        var path = Path.Combine(Application.dataPath, ImportPath);
        foreach (var it in Directory.GetFiles(path, "*.*", SearchOption.AllDirectories))
        {
            var parts = it.Split("Assets", 2);
            var assetPath = "Assets/" + parts[1];
            AudioClip clip = (AudioClip)UnityEditor.AssetDatabase.LoadAssetAtPath(assetPath, typeof(AudioClip));
            if (clip == null) continue;
            Debug.Log($"{it} {clip.name}");
            var go = new GameObject(clip.name);
            go.transform.SetParent(importRoot.transform);
            var source =go.AddComponent<AudioSource>(); 
            source.clip = clip;
            source.outputAudioMixerGroup = ImportMixerGroup;
        }
    }
    #endif
}
