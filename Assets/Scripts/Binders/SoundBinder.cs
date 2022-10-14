using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBinder : MonoBehaviour
{
    [SerializeField] private List<AudioClip> attackSound = new List<AudioClip>();
    [SerializeField] private List<AudioClip> deadSound = new List<AudioClip>();
    [SerializeField] private float volume = 0.5f;
    private void Start()
    {
        IUnit unit = GetComponent<IUnit>();
        var attacker = GetComponent<AbsAttacker>();
        var health = GetComponent<Health>();
        for (int i = 0; i < attackSound.Count; i++)
        {
            int local = i;
            attacker.OnAttack += () =>
            {
                Tars.Audio.AudioManager.PlaySound(attackSound[local], unit.Target.GO, volume);
            };
        }
        for (int i = 0; i < deadSound.Count; i++)
        {
            int local = i;
            var go = gameObject;
            health.OnDead += () =>
            {
                Tars.Audio.AudioManager.PlaySound(deadSound[local], go, volume);
            };
        }

        Destroy(this);
    }
}
