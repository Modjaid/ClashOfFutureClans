using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    private Text fps;
    private float[] delta = new float[3];

    void Start()
    {
        fps = GetComponent<Text>();
	    StartCoroutine(FPSRoutine());
    }

    // Update is called once per frame
    IEnumerator FPSRoutine()
    {
        while (true)
        {
            fps.text = "FPS is " + (1f / Time.smoothDeltaTime).ToString("f");
            yield return new WaitForSeconds(0.5f);
        }
    }
}
