using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEmitSoundPlay : MonoBehaviour
{
    [SerializeField] private AudioClip clip;
    [SerializeField] private float volume;
    private ParticleSystem.Particle[] m_particles;
    private ParticleSystem _ps_system;

    private void Start()
    {
        _ps_system = GetComponent<ParticleSystem>();
    }

    void LateUpdate()
    {
        m_particles = new ParticleSystem.Particle[_ps_system.particleCount];
        _ps_system.GetParticles(m_particles);
        int c = 0;
        for (int i = 0; i < m_particles.Length; i++)
        {
            var particle = m_particles[i];
            if (particle.startLifetime - particle.remainingLifetime < Time.deltaTime)
            {
                Tars.Audio.AudioManager.PlaySoundNear(clip, particle.position, volume);
                c++;
                break;
            }
        }
        //Debug.Log(c);
    }
}
