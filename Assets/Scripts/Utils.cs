using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static float MapIntoRange (float value, float from1, float to1, float from2, float to2) {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2; 
    }

    public static bool FuzzyMatch(string haystack, string needle)
    {
        return haystack.ToLower().Contains(needle.ToLower());
    }

    public static T PickRandom<T>(IList<T> l)
    {
        int idx = Random.Range(0, l.Count);
        return l[idx];
    }
}
