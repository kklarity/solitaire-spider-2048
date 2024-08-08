using System.Collections.Generic;
using _Project.Scripts.Ads;
using _Project.Scripts.Card;
using _Project.Scripts.Saves;
using _Project.Scripts.TriggerEndGame;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.UI
{
    public class UIController : MonoBehaviour
    {
        public RectTransform loseScreen;
        public RectTransform menuScreen;

        private void OnEnable()
        {
            TriggerEnd.onTouched += OpenLoseScreen;
            // RewardedAd.isRewardOn += CloseLoseScreen;
        }

        private void OnDisable()
        {
            TriggerEnd.onTouched -= OpenLoseScreen;
            // RewardedAd.isRewardOn -= CloseLoseScreen;
        }

        public void RestartLevel()
        {
            Debug.Log("Restart level by method");
            DataSaver.ResetData();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void OpenLoseScreen()
        {
            loseScreen.DOScale(new Vector3(1f, 1f, 1f), 0.2f);
        }

        public void CloseLoseScreen()
        {
            Debug.Log("close lose screen");
            // if (loseScreen != null)
            // {
            //     loseScreen.DOScale(new Vector3(0f, 0f, 1f), 0.01f);
            // }
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
            loseScreen.DOScale(new Vector3(0f, 0f, 1f), 0.01f);
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
    }
}
