using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAttacker : AbsAttacker
{
    [Header("Base damage of component")]
    [SerializeField] private int damage = 5;
    [Header("Reload time for attack")]
    [SerializeField] private float cooldownTime;
    [Header("Time before animation key frame")]
    [SerializeField] private float damageDelay;
    [Header("1 diagonal unit is 14, others are 10")]
    [SerializeField] private int attackDistance;

    private IEnumerator attackRoutine;
    private IUnit unit;
    private IUnit Unit
    {
        get
        {
            if (unit == null)
                unit = GetComponent<IUnit>();
            return unit;
        }
    }
    private IUnit target;
    public override IUnit Target
    {
        set
        {
            target = value;
        }
    }
    public override bool CanAttack
    {
        get
        {
            return target != null && Unit.GridPos.GridDistance(target.GridPos) <= attackDistance;
        }
    }


    public override void Attack()
    {
        if (attackRoutine == null)
        {
            attackRoutine = AttackRoutine(target);
            StartCoroutine(attackRoutine);
        }
    }

    private IEnumerator AttackRoutine(IUnit unit)
    {
        if (unit != null && !unit.Equals(null))
        {
            RiseOnAttack();
        }
        else
        {
            attackRoutine = null;
            yield break;
        }
        yield return new WaitForSeconds(damageDelay);
        if (unit != null && !unit.Equals(null))
        {
            unit.Health.Damage(damage, Unit);
        }
        else
        {
            attackRoutine = null;
            yield break;
        }
        yield return new WaitForSeconds(cooldownTime - damageDelay);
        attackRoutine = null;
    }
    private void OnDisable()
    {
        attackRoutine = null;
    }
}
