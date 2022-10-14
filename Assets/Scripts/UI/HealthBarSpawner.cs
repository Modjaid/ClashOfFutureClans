using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// По дефолту выключен
/// </summary>
public class HealthBarSpawner : MonoBehaviour
{

    public void Spawn(HealthBar bar)
    {
        bar = Instantiate(bar, HealthCanvas.Instance.transform);
        var unit = GetComponent<IUnit>();
        bar.Init(unit);
        (unit.Health as Health).OnDead += () =>
        {
            HealthCanvas.Instance.RemoveHealthBar(bar);
            Destroy(bar.gameObject);
            (unit.Health as Health).OnDamage -= bar.UpdateInfo;
        };
        (unit.Health as Health).OnDamage += bar.UpdateInfo;
        HealthCanvas.Instance.AddHealthBar(bar);

        Destroy(this);
    }
}
