using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRotationBinder : MonoBehaviour
{
    [SerializeField] private Transform toRotate;
    [SerializeField] private Vector3 axis;
    private IUnit unit;
    void Start()
    {
        unit = GetComponent<IUnit>();
    }

    // Update is called once per frame
    void Update()
    {
        if (unit.Target != null)
        {
            var delta = Quaternion.LookRotation(unit.Target.GO.transform.position - toRotate.position, Vector3.up);
            var vec = axis * (delta.eulerAngles.y + 90);
            delta = Quaternion.Euler(vec);
            toRotate.rotation = Quaternion.RotateTowards(toRotate.rotation, delta, Time.deltaTime * 300);
        }
    }
}
