using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonflowerCarnivore.ShurikenSalvo;

public class AttackVFXBinder : MonoBehaviour
{
    public List<ParticleSystem> AutoTargetVFX = new List<ParticleSystem>();
    public List<ParticleSystem> SimpleVFX = new List<ParticleSystem>();

    void Start()
    {
        var unit = GetComponent<IUnit>();
        var attacker = GetComponent<AbsAttacker>();
        for (int i = 0; i < AutoTargetVFX.Count; i++)
        {
            var homing = AutoTargetVFX[i].gameObject.AddComponent<ParticleTargetted>();
            int local = i;
            attacker.OnAttack += () =>
            {
                AutoTargetVFX[local].Play();
                homing.Target = unit.Target.GO.transform;
            };
        }

        for (int i = 0; i < SimpleVFX.Count; i++)
        {
            var homing = SimpleVFX[i].gameObject.AddComponent<ParticleCollider>();
            int local = i;
            attacker.OnAttack += () =>
            {
                SimpleVFX[local].Play();
                homing.Target = unit.Target.GO.transform;
            };
        }

        Destroy(this);
    }
}
