using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.UI;
using UnityEngine;

namespace _Project.Scripts.TriggerEndGame
{
    public class TriggerEnd : MonoBehaviour
    {
        public static Action onTouched;
        public static bool messageSent;

        public List<int> ballsData = new List<int>();
        private Dictionary<int, Coroutine> ballCoroutines = new Dictionary<int, Coroutine>();

        private void Start()
        {
            messageSent = false;
        }

        private void OnEnable()
        {
            UIController.onLoseNoMoveOpened += ToggleMessage;
            UIController.onLoseCrossOpened += ToggleMessage;
        }

        private void OnDisable()
        {
            UIController.onLoseNoMoveOpened -= ToggleMessage;
            UIController.onLoseCrossOpened -= ToggleMessage;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Ball") && !UIController.endGame && !messageSent)
            {
                int instanceID = other.gameObject.GetInstanceID();
                if (!ballsData.Contains(instanceID))
                {
                    ballsData.Add(instanceID);
                    Coroutine coroutine = StartCoroutine(ExecuteAfterTime(instanceID, 1));
                    ballCoroutines[instanceID] = coroutine;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Ball") && !UIController.endGame)
            {
                int instanceID = other.gameObject.GetInstanceID();
                if (ballsData.Contains(instanceID))
                {
                    ballsData.Remove(instanceID);
                    if (ballCoroutines.ContainsKey(instanceID))
                    {
                        StopCoroutine(ballCoroutines[instanceID]);
                        ballCoroutines.Remove(instanceID);
                    }
                }
            }
        }

        IEnumerator ExecuteAfterTime(int instanceID, float timeInSec)
        {
            yield return new WaitForSeconds(timeInSec);
            if (ballsData.Contains(instanceID))
            {
                messageSent = true;
                onTouched?.Invoke();
                print("message sent: " + messageSent);
            }
        }

        private void ToggleMessage()
        {
            messageSent = true;
        }
    }
}
