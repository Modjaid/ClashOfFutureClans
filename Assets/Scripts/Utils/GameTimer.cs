using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// !!IS NOT REAL TIME!!
/// StartTimer();
/// </summary>
public class GameTimer : MonoBehaviour
{
    private int currentSec = 0;
    public TimeSpan CurrentTime { get { return TimeSpan.FromSeconds((double)currentSec); } }

    public event Action<TimeSpan> OnEverySecond;


    /// <param name="startSec">if timeSeconds == 0 - timer Stop</param>
    public void StartTimer(int timeSeconds)
    {
        this.currentSec = timeSeconds;
        StopAllCoroutines();
        StartCoroutine(UpdateSec());
    }


    public void Stop()
    {
        StopAllCoroutines();
    }

    private IEnumerator UpdateSec()
    {
        while (currentSec > 0)
        {
            yield return new WaitForSecondsRealtime(1);
            currentSec--;
            OnEverySecond?.Invoke(CurrentTime);
        }
    }

}
