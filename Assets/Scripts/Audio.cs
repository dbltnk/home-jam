using System.Linq;
using UnityEngine;

public class Audio : MonoBehaviour
{
    public static Audio Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayAt(string name, Vector3 pos)
    {
        var group = transform.Find(name);
        if (group == null) return;
        var readySources = group.GetComponentsInChildren<AudioSource>()
            .Where(it => it.isPlaying == false)
            .ToList();
        if (readySources.Count > 0)
        {
            int idx = Random.Range(0, readySources.Count);
            var source = readySources[idx];
            source.transform.position = pos;
            source.Play();
        }
    }
}
