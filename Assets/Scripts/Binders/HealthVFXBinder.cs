using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonflowerCarnivore.ShurikenSalvo;

public class HealthVFXBinder : MonoBehaviour
{
    public List<ParticleSystem> DamageVFX = new List<ParticleSystem>();
    public List<ParticleSystem> DeadVFX = new List<ParticleSystem>();
    void Start()
    {
        var unit = GetComponent<IUnit>();
        var Health = GetComponent<Health>();
        if (DamageVFX != null && DamageVFX.Count != 0)
        {
            Health.OnDamage += (p, u) =>
            {
                var rnd = Random.Range(0, DamageVFX.Count - 1);
                DamageVFX[rnd].gameObject.SetActive(true);
                DamageVFX[rnd].Play();
            };
        }
        for (int i = 0; i < DeadVFX.Count; i++)
        {
            int local = i;
            Health.OnDead += () =>
            {
                DeadVFX[local].Play();
                DeadVFX[local].gameObject.transform.parent = null;
            };
        }

        Destroy(this);
    }
}
