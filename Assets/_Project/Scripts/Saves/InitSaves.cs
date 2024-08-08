using _Project.Scripts.Card;
using TMPro;
using UnityEngine;
using YG;

namespace _Project.Scripts.Saves
{
    public class InitSaves : MonoBehaviour
    {
        public GameObject cardPrefab;
        public TextMeshProUGUI _scoreText;
        public TextMeshProUGUI _highScoreText;

        private void Start()
        {
            var saves = DataSaver.LoadGameObjects();
            foreach (var data in saves.gameObjectDataList)
            {
                var cardObject = Instantiate(cardPrefab, data.position.ToVector3(), Quaternion.identity);
                var cardComponent = cardObject.GetComponent<CardComponent>();
                cardComponent.Card = new Card.Card(data.suit, data.rank);
                cardComponent.ApplySprite();
                cardComponent.SetScaleBasedOnRank();
            }

            _highScoreText.text = YandexGame.savesData.highScore.ToString();
            _scoreText.text = YandexGame.savesData.currentScore.ToString();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus)
            {
                var saves = new SavesYG();
                var cards = FindObjectsOfType<CardComponent>();
                foreach (var card in cards)
                {
                    GameObjectData data = new GameObjectData
                    {
                        // position = card.transform.position,
                        position = new SimpleVector3(card.transform.position),
                        suit = card.Card.Suit,
                        rank = card.Card.Rank
                    };
                    saves.gameObjectDataList.Add(data);
                }
                DataSaver.SaveGameObjects(saves);
                YandexGame.NewLeaderboardScores("ScoreLeaderBoard", YandexGame.savesData.highScore);
            }
        }

        // private void OnApplicationQuit()
        // {
        //     var saves = new SavesYG();
        //     var cards = FindObjectsOfType<CardComponent>();
        //     foreach (var card in cards)
        //     {
        //         GameObjectData data = new GameObjectData
        //         {
        //             // position = card.transform.position,
        //             position = new SimpleVector3(card.transform.position),
        //             suit = card.Card.Suit,
        //             rank = card.Card.Rank
        //         };
        //         saves.gameObjectDataList.Add(data);
        //     }
        //     DataSaver.SaveGameObjects(saves);
        // }
    }
}