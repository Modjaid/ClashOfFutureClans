using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Create new units by touch on Screen
/// !Warning! !!!Определяет землю тупа по имени ГО "Ground" на текущий момент!!!
/// </summary>
public class UnitCreator : MonoBehaviour
{
    public event Action<GameObject> OnCreateNewUnit;

    [SerializeField] private float createDelay;
    [Header("UI Panel in BattleWarUI object")]
    [SerializeField] private UITriggerHandler triggerHandler;
    [Header("радиус от точки касания, для проверки есть ли здания поблизости")]
    [SerializeField] private float controlRadius;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private LayerMask mask;
    private BattleWarCardUI currentUnitCard;
  
    private Coroutine cooldown;
    private bool IsStun;


    private Vector3 testToucPos;
    private Color testColor;

    public void Start()
    {
        triggerHandler.OnDragEvent += (x) => IsStun = true;
    }

    public void StartCreator(BattleWarCardUI unit)
    {
        currentUnitCard = unit;
        triggerHandler.OnEndEvent += OnOneTouchHandler;
    }

    public void StopCreator()
    {
        triggerHandler.OnEndEvent -= OnOneTouchHandler;
    }

    [Obsolete("TODO: Ubrat' opredelenie zemli po imeni")]
    public void CreateUnitWithDelay(PointerEventData touchData)
    {
        if(currentUnitCard.Count > 0 && cooldown == null)
        {
            Ray ray = Camera.main.ScreenPointToRay(touchData.position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.name == "Ground")
                {
                    for (int x = 0; x < currentUnitCard.SpawnCount; x++)
                    {
                        var newGO = Instantiate(currentUnitCard.UnitPrefab);
                        newGO.transform.position = hit.point + new Vector3(x / 2, 0, x % 2);
                        newGO.name = currentUnitCard.UnitName;
                        currentUnitCard.Count--;
                        OnCreateNewUnit?.Invoke(newGO);
                        cooldown = StartCoroutine(Utils.Delay(createDelay, () => cooldown = null));
                    }
                }
            }
        }
    }


    private void OnOneTouchHandler(PointerEventData touchData)
    {
        if (currentUnitCard.Count > 0 && !IsStun)
        {
            Ray ray = Camera.main.ScreenPointToRay(touchData.position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit,10000, mask))
            {
                if (hit.transform.gameObject.name == "Ground" && !SearchBuildingInRadius(hit.point) && !levelManager.BaseCollider.Contains(hit.point))
                {
                    testToucPos = hit.point;
                    testColor = Color.green - new Color(0, 0, 0, 0.7f);

                    for (int x = 0; x < currentUnitCard.SpawnCount; x++)
                    {
                        var newGO = Instantiate(currentUnitCard.UnitPrefab);
                        newGO.transform.position = hit.point;
                        newGO.transform.position = hit.point + new Vector3(x / 2, 0, x % 2);
                        newGO.name = currentUnitCard.UnitName;
                        currentUnitCard.Count--;
                        OnCreateNewUnit?.Invoke(newGO);
                  //          currentUnitCard.SetAwaitMode();

                    }
                }
                else
                {
                    testColor = Color.red - new Color(0, 0, 0, 0.7f);
                }
            }
        }
        IsStun = false;
    }
    /// <summary>
    /// False - Здания в радиусе касания отсутствуют
    /// </summary>
    /// <returns></returns>
    private bool SearchBuildingInRadius(Vector3 touchPos)
    {
       var colliders = Physics.OverlapSphere(touchPos,controlRadius);
        foreach(var collider in colliders)
        {
            if (collider.GetComponent<BasingBuilding>())
            {
                return true;
            }
        }
        return false;
    }

    private bool IsBase(Vector3 toucPos)
    {
        //  Debug.Log($"touchPos:{toucPos}, {(toucPos.x <= levelManager.BaseScale && toucPos.z <= levelManager.BaseScale)}");
        return levelManager.BaseCollider.Contains(toucPos);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = testColor;
        Gizmos.DrawSphere(testToucPos, controlRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(levelManager.BaseCollider.center,levelManager.BaseCollider.size);
    }
}
