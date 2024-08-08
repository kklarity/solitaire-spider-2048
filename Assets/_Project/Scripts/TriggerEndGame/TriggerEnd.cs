using System;
using UnityEngine;

namespace _Project.Scripts.TriggerEndGame
{
    public class TriggerEnd : MonoBehaviour
    {
        public static Action onTouched;
        public static bool messageSent = false;

        private void Start()
        {
            messageSent = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Ball"))
            {
                onTouched?.Invoke();
                messageSent = true;
            }
        }
    }
}