using System.Collections;
using UnityEngine;

public class DestroyAfterTimeout : MonoBehaviour
{
    [SerializeField] private float Timeout = 1f;
    
    private void OnEnable()
    {
        StartCoroutine(CoroKillMe(Timeout));
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator CoroKillMe(float timeout)
    {
        yield return new WaitForSeconds(timeout);
        Destroy(gameObject);
    }
}
