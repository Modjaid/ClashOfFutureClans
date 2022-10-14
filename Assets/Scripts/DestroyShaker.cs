using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyShaker : MonoBehaviour
{
    [SerializeField] private float shakeForce;
    [SerializeField] private float shakeTime;

    public void OnDestroy()
    {
        var camera = Camera.main;
        if(camera) camera.GetComponent<CameraShake>().Shake(shakeForce, shakeTime);
    }
}
