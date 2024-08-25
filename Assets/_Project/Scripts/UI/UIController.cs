using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Card;
using _Project.Scripts.Controller;
using _Project.Scripts.Saves;
using _Project.Scripts.TriggerEndGame;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace _Project.Scripts.UI
{
    public class UIController : MonoBehaviour
    {
        public RectTransform loseScreenNoExit;
        public RectTransform loseScreenCrossLine;
        public RectTransform menuScreen;

        public static Action onLoseNoMoveOpened;
        public static Action onLoseCrossOpened;

        public static bool endGame;

        private void Start()
        {
            endGame = false;
        }

        private void OnEnable()
        {
            TriggerEnd.onTouched += OpenLoseScreenCrossLine;
            InputController.onStepsEnded += OpenLoseScreen;
            // RewardedAd.isRewardOn += CloseLoseScreen;
        }

        private void OnDisable()
        {
            TriggerEnd.onTouched -= OpenLoseScreenCrossLine;
            InputController.onStepsEnded -= OpenLoseScreen;
            // RewardedAd.isRewardOn -= CloseLoseScreen;
        }

        public void RestartLevel()
        {
            Debug.Log("Restart level by method");
            DataSaver.ResetData();
            // CloseLoseScreen();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void OpenLoseScreen()
        {
            StartCoroutine(ExecuteAfterTime());
        }

        public void OpenLoseScreenCrossLine()
        {
            StartCoroutine(OpenCrossLineScreen());
        }

        public void CloseLoseScreen()
        {
            Debug.Log("close lose screen");
            
            if (loseScreenNoExit != null)
            {
                loseScreenNoExit.DOScale(new Vector3(0f, 0f, 1f), 0.01f);
                RestartLevel();
            }
        }

        public void ShowMenu()
        {
            menuScreen.DOScale(new Vector3(1f, 1f, 1f), 0.2f);
        }
        public void CloseMenu()
        {
            menuScreen.DOScale(new Vector3(0f, 0f, 1f), 0.2f);
        }

        public void ReturnForAReward()
        {
            loseScreenNoExit.DOScale(new Vector3(0f, 0f, 1f), 0.01f);
            TriggerEnd.messageSent = false;
            var myItems = FindObjectsOfType<CardComponent>();
            DestroyHalfObjects(myItems);
        }

        private static void DestroyHalfObjects(IReadOnlyList<CardComponent> allItems)
        {
            var halfCount = allItems.Count / 2;
            for (var i = 0; i < halfCount; i++)
            {
                Destroy(allItems[i].gameObject);
            }
        }
        IEnumerator ExecuteAfterTime()
        {
            yield return new WaitForSeconds(3);
            endGame = true;
            loseScreenNoExit.DOScale(new Vector3(1f, 1f, 1f), 0.2f);
            onLoseNoMoveOpened.Invoke();
        }

        IEnumerator OpenCrossLineScreen()
        {
            yield return new WaitForSeconds(3);
            endGame = true;
            loseScreenCrossLine.DOScale(new Vector3(1f, 1f, 1f), 0.2f);
            onLoseCrossOpened.Invoke();
        }
        
    }
}
