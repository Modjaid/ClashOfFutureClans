using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils 
{
    public static IEnumerator Delay(float delay, Action callback)
    {
        yield return new WaitForSeconds(delay);
        callback();
    }

    /// <summary>
    /// ����������� ��������������� levelPoints � ����������� ������� 
    /// </summary>
    public static int LevelCalc(float levelPoints)
    {
        //TODO: ����������� ������� �������� ������ �� levelPoints � �������� � ����
        return 0;
    }
    public static Vector3 GetRandomPointInCollider(Collider ground)
    {
        var point = new Vector3(
            UnityEngine.Random.Range(ground.bounds.min.x, ground.bounds.max.x),
            UnityEngine.Random.Range(ground.bounds.min.y, ground.bounds.max.y),
            UnityEngine.Random.Range(ground.bounds.min.z, ground.bounds.max.z)
        );
        return point;
    }
    public static Vector3 GetRandomPointInCollider(Bounds ground)
    {
        var point = new Vector3(
            UnityEngine.Random.Range(ground.min.x, ground.max.x),
            UnityEngine.Random.Range(ground.min.y, ground.max.y),
            UnityEngine.Random.Range(ground.min.z, ground.max.z)
        );
        return point;
    }
}
