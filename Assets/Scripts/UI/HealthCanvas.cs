using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCanvas : MonoBehaviour
{
    [Header("трансформы баров при зуме камеры")]
    [Range(-30,-10)]
    [SerializeField] private float transformMoveSensitive = -20;
    [Range(40, 90)]
    [SerializeField] private float transformScaleSensitive = 70;

    [SerializeField] private HealthBar blueBarPrefab;
    [SerializeField] private HealthBar redBarPrefab;

    private static HealthCanvas instance;
    public static HealthCanvas Instance => instance;

    private readonly HashSet<HealthBar> healthBars = new HashSet<HealthBar>();
    private Camera cam;


    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        cam = Camera.main;
    }


    private void Update()
    {
        foreach (HealthBar item in healthBars)
        {
            var canvasPos = cam.WorldToScreenPoint(item.unit.position + (Vector3.up * 4.5f) + (Vector3.up / cam.transform.position.y * transformMoveSensitive));
            item.transform.position = canvasPos;
            item.transform.localScale = Vector3.one / cam.transform.position.y * transformScaleSensitive;
        }

    }
    public void Init(TeamManager leftTeam,TeamManager rightTeam)
    {
        leftTeam.OnAddUnit += (unit) =>
        {
            if (ControlUnitType(unit))
            {
                unit.GO.GetComponent<HealthBarSpawner>().Spawn(blueBarPrefab);
            }
        };

        rightTeam.OnAddUnit += (unit) =>
        {
            if (ControlUnitType(unit))
            {
                unit.GO.GetComponent<HealthBarSpawner>().Spawn(redBarPrefab);
            }
        };
    }

    public void AddHealthBar(HealthBar bar)
    {
        healthBars.Add(bar);
    }

    public void RemoveHealthBar(HealthBar bar)
    {
        healthBars.Remove(bar);
    }

    private bool ControlUnitType(IUnit unit)
    {
        switch (unit.Entity)
        {
            case Entity.Wall: return false;
        }
        return true;

    }

}
