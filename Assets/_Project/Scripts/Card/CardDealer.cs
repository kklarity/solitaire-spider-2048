using System.Collections.Generic;
using _Project.Scripts.UI;
using UnityEngine;
using YG;

namespace _Project.Scripts.Card
{
    public class CardDealer : MonoBehaviour
    {
        public static CardDealer Instance { get; private set; }

        private static readonly string[] Suits = { "Hearts", "Spades" };
        private static readonly Dictionary<int, int> RankWeights = new()
        {
            { 2, 50 },
            { 3, 30 },
            { 4, 15 },
            //Debug stats
            // { 3, 60 },
            // { 5, 40 },
            // { 8, 40 },
            
        };

        private int currentSuitIndex = 0;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public Card GetRandomCard()
        {
            var suit = Suits[currentSuitIndex];
            var rank = GetRandomRank();
            currentSuitIndex = (currentSuitIndex + 1) % Suits.Length;
            return new Card(suit, rank);
        }

        private int GetRandomRank()
        {
            var totalWeight = 0;
            foreach (var weight in RankWeights.Values)
            {
                totalWeight += weight;
            }

            var randomValue = Random.Range(0, totalWeight);
            foreach (var rank in RankWeights.Keys)
            {
                if (randomValue < RankWeights[rank])
                {
                    return rank;
                }
                randomValue -= RankWeights[rank];
            }

            return 2;
        }

        public void InstantiateNextCard(GameObject nextCardPrefab, Vector3 position, Card currentCard)
        {
            var nextCard = Instantiate(nextCardPrefab, position, Quaternion.identity);
            var nextCardComponent = nextCard.GetComponent<CardComponent>();
            nextCardComponent.Card = new Card(currentCard.Suit, currentCard.Rank + 1);

            if (nextCardComponent.Card.Rank > 14)
            {
                nextCardComponent.Card.Rank = 2;
                currentSuitIndex = (currentSuitIndex + 1) % Suits.Length;
                nextCardComponent.Card.Suit = Suits[currentSuitIndex];
            }

            // Debug.Log($"New card created with rank {nextCardComponent.Card.Rank} and suit {nextCardComponent.Card.Suit}");
            nextCardComponent.SetScaleBasedOnRank();
            nextCardComponent.ApplySprite();
        }

        public (Card, Sprite) GetFutureCardSprite()
        {
            var futureCard = GetRandomCard();
            var spriteName = $"{futureCard.Rank}{futureCard.Suit[0].ToString().ToLower()}";
            var cardSprite = Resources.Load<Sprite>($"Sprites/{spriteName}");
            if (cardSprite != null)
            {
                YandexGame.savesData.FutureCard = futureCard;
                return (futureCard, cardSprite);
            }
            else
            {
                Debug.LogError($"Future card sprite {spriteName} not found in Resources/Sprites");
                return (null, null);
            }
        }

        public GameObject SpawnCardWithSprite(GameObject cardPrefab, Vector3 position, Card card, Sprite sprite)
        {
            var cardObject = Instantiate(cardPrefab, position, Quaternion.identity);
            var cardComponent = cardObject.GetComponent<CardComponent>();
            cardComponent.Card = card;
            cardComponent.SetScaleBasedOnRank();
            cardObject.GetComponent<SpriteRenderer>().sprite = sprite;
            return cardObject;
        }
        
        public void SpawnSpecificCardsAtPositions(GameObject cardPrefab, Vector3[] positions)
        {
            Card[] cards = new Card[]
            {
                new Card("Hearts", 2),
                new Card("Spades", 2),
                new Card("Hearts", 3),
                new Card("Spades", 4),
                new Card("Hearts", 2),
                new Card("Spades", 3),
                new Card("Hearts", 4)
            };

            for (int i = 0; i < positions.Length; i++)
            {
                var card = cards[i];
                var sprite = Resources.Load<Sprite>($"Sprites/{card.Rank}{card.Suit[0].ToString().ToLower()}");
                SpawnCardWithSprite(cardPrefab, positions[i], card, sprite);
            }
        }

    }
}
