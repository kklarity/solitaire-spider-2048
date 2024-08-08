using System;
using _Project.Scripts.UI;
using UnityEngine;
using YG;

namespace _Project.Scripts.Ads
{
    public class RewardedAd : MonoBehaviour
    {
        private UIController _uiController;
        // public static Action isRewardOn;

        private void Start()
        {
            _uiController = GetComponent<UIController>();
        }

        private void OnEnable() => YandexGame.RewardVideoEvent += Rewarded;
        
        private void OnDisable() => YandexGame.RewardVideoEvent -= Rewarded;

        private void Rewarded(int id)
        {
            if (id == 1)
                ContinueGame();
        }
        
        public void GetLifeByRewardAd(int id)
        {
            
            YandexGame.RewVideoShow(id);
            
        }

        private void ContinueGame()
        {
            // isRewardOn.Invoke();
            _uiController.ReturnForAReward();
            YandexGame.SaveProgress();
        }
    }
}