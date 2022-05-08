using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LogoCamHack : MonoBehaviour
{
    IEnumerator Start()
    {
        var camData = GetComponent<UniversalAdditionalCameraData>();
        yield return null;
        camData.renderType = CameraRenderType.Overlay;
        yield return null;
        camData.renderType = CameraRenderType.Base;
    }
}
