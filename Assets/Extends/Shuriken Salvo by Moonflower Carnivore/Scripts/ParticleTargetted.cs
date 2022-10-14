using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MoonflowerCarnivore.ShurikenSalvo
{
	public class ParticleTargetted : MonoBehaviour
	{
		[Tooltip("Target object. If this parameter is undefined it will assume the attached object itself which creates self chasing particle effect.")]
		public Transform target;
		public Transform Target
        {
			set
            {
				if (target == value || value == null)
					return;
				target = value;
				_ps_system = GetComponent<ParticleSystem>();
				_ps_trigger = _ps_system.trigger;
				_ps_trigger.SetCollider(0, target.GetComponent<Collider>());
				_ps_particles = new ParticleSystem.Particle[_ps_system.main.maxParticles];// Since Unity 5.5
			}
        }
		[Tooltip("How fast the particle is guided to the closest target.")]
		public float speed = 30;
		[Tooltip("Cap the maximum speed to prevent particle from being flung too far from the missed target.")]
		public float maxSpeed = 50;
		[Tooltip("How long in the projectile begins being guided towards the target. Higher delay and high particle start speed requires greater distance between attacker and target to avoid uncontrolled orbitting around the target.")]
		public float homingDelay = 0f;
		private ParticleSystem _ps_system;
		private ParticleSystem.TriggerModule _ps_trigger;
		private ParticleSystem.Particle[] _ps_particles;

		void LateUpdate()
		{
			if (target == null)
				return;
			_ps_trigger.SetCollider(0, target.GetComponent<Collider>());
			int numParticlesAlive = _ps_system.GetParticles(_ps_particles);
			float ted = (target.position + Vector3.up - this.transform.position).sqrMagnitude + 0.001f;
			for (int i = 0; i < numParticlesAlive; i++)
			{
				Vector3 diff = target.position + Vector3.up - _ps_particles[i].position;
				float diffsqrm = diff.sqrMagnitude;
				float face = Vector3.Dot(_ps_particles[i].velocity.normalized, diff.normalized);
				float f = Mathf.Abs((ted - diffsqrm) / ted) * ted * (face + 1.001f);

				float t = 0;
				t += Time.deltaTime / (homingDelay + 0.0001f) * 100f;
				_ps_particles[i].velocity = Vector3.ClampMagnitude(Vector3.Slerp(_ps_particles[i].velocity, _ps_particles[i].velocity + diff * speed * 0.01f * f, t), maxSpeed);
			}
			_ps_system.SetParticles(_ps_particles, numParticlesAlive);
		}
	}
}