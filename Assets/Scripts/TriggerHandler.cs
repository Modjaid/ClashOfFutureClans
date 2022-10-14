using System.Collections;
using System;
using UnityEngine;

namespace Artromskiy
{
    public sealed class TriggerHandler : MonoBehaviour
    {
        /// <summary>
        /// Вызывается, когда Collider входит в триггер
        /// </summary>
        public event Action<Collider> OnEnter;
        /// <summary>
        /// Вызывается, когда Collider перестаёт касаться триггера
        /// </summary>
        public event Action<Collider> OnExit;
        /// <summary>
        /// Вызывается, пока Collider пересекается с триггером
        /// </summary>
        public event Action<Collider> OnStay;

        private void OnTriggerEnter(Collider other)
        {
            OnEnter?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            OnExit?.Invoke(other);
        }

        private void OnTriggerStay(Collider other)
        {
            OnStay?.Invoke(other);
        }
    }
}