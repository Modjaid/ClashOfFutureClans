using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MoonflowerCarnivore.ShurikenSalvo
{
	public class ParticleCollider : MonoBehaviour
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
		public float speed = 10f;
		[Tooltip("Cap the maximum speed to prevent particle from being flung too far from the missed target.")]
		public float maxSpeed = 50f;
		[Tooltip("How long in the projectile begins being guided towards the target. Higher delay and high particle start speed requires greater distance between attacker and target to avoid uncontrolled orbitting around the target.")]
		public float homingDelay = 1f;
		private ParticleSystem _ps_system;
		private ParticleSystem.TriggerModule _ps_trigger;
		private ParticleSystem.Particle[] _ps_particles;

		void LateUpdate()
		{
			if (target == null)
				return;
			//_ps_trigger.SetCollider(0, target.GetComponent<Collider>());
		}
	}
}