using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Рандомно инстацирует по всей земле объекта с именем Ground
/// </summary>
/// <param name="availableUnits"></param>
public class AutoUnitCreator : MonoBehaviour
{
    public event Action<GameObject> OnCreateNewUnit;

    [Header("Задержка между созданиями юнитов")]
    [SerializeField] private float createDelay;
    [Header("Земля где рандомно инстацируются юниты")]
    [SerializeField] private LevelManager lvlManager;
    /// <summary>
    /// Unit, UnitName, count of Units (TeamData), count of units (FractionData)
    /// </summary>
    private Dictionary<Unit,(string,int,int)> units;

    private Coroutine autoInstanceRoutine;
    private Map map;


    public void Init(TeamData availableUnits,Map map)
    {
        this.map = map;
        units = new Dictionary<Unit, (string, int,int)>();
        foreach(var teamUnit in availableUnits.GetAllAvailableUnits())
        {
            if(FractionData.TryGetUnitDataFromAllFractions(teamUnit.Key, out FractionData.UnitData target)){
                units[teamUnit.Key] = (target.name, teamUnit.Value,target.count);
            }
        }
    }

    public void StartRandomInstantiate()
    {
        if (autoInstanceRoutine != null) StopCoroutine(autoInstanceRoutine);
        autoInstanceRoutine = StartCoroutine(AutoInstance());
    }
    public void StopRandomInstantiate()
    {
        if (autoInstanceRoutine != null) StopCoroutine(autoInstanceRoutine);
        autoInstanceRoutine = null;
    }
    private IEnumerator AutoInstance()
    {
        while (units.Count > 0)
        {
            var keyValue = units.ElementAt(0);
            var value = units[keyValue.Key];
            for (int j = 0; j < value.Item2; j++)
            {
                for (int i = 0; i < value.Item3; i++)
                {
                    var randPos = GetRandomPoints();
                    var newUnit = Instantiate(keyValue.Key.gameObject);
                    newUnit.gameObject.name = value.Item1;
                    newUnit.transform.position = randPos + new Vector3(i / 2, 0, i % 2);
                    OnCreateNewUnit?.Invoke(newUnit);
                }
            }
            units.Remove(keyValue.Key);

            yield return new WaitForSeconds(createDelay);
        }

        autoInstanceRoutine = null;
    }


    /// <summary>
    /// For Editor
    /// </summary>
    public int GetAllUnitsCount()
    {
        int countUnits = 0;
        if (units.Count <= 0) return 0;
        foreach(KeyValuePair<Unit,(string,int,int)> item in units)
        {
            countUnits += item.Value.Item2;
        }
        return countUnits;
    }

    /// <summary>
    /// рандомная позиция тыкает пока не будет чистое поле без билдингов
    /// </summary>
    /// <returns></returns>
    private Vector3 GetRandomPoints()
    {
        while (true)
        {
            (int,int) randPos = Utils.GetRandomPointInCollider(lvlManager.BaseCollider).ToTuple();
           IUnit unit = map.GetUnit(randPos);

            if (unit == null)
            {
                return new Vector3(randPos.Item1,0,randPos.Item2);
            }
        }
    }

}
