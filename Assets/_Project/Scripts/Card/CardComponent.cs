using _Project.Scripts.Score;
using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts.Card
{
    public class CardComponent : MonoBehaviour
    {
        public Card Card { get; set; }
        public GameObject nextCardPrefab;

        private void Start()
        {
            InitializeCard();
        }

        public void InitializeCard()
        {
            if (Card == null)
            {
                
                GetCurrentCard();
            }
        }

        public Card GetCurrentCard()
        {
            Card = CardDealer.Instance.GetRandomCard();
            SetScaleBasedOnRank();
            ApplySprite();
            return Card;
        }

        private void OnDestroy()
        {
            if (transform != null)
            {
                transform.DOKill();
            }

            // Очистка ссылок
            Card = null;
            nextCardPrefab = null;
        }

       

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var otherCardComponent = collision.gameObject.GetComponent<CardComponent>();

            if (otherCardComponent != null)
            {
                var otherCard = otherCardComponent.Card;

                if (Card.Suit == otherCard.Suit && Card.Rank == otherCard.Rank)
                {
                    var thisID = gameObject.GetInstanceID();
                    var otherID = collision.gameObject.GetInstanceID();
                    if (thisID > otherID)
                    {
                        Vector3 collisionPoint = collision.contacts[0].point;
                        if (CardDealer.Instance != null)
                        {
                            CardDealer.Instance.InstantiateNextCard(nextCardPrefab, collisionPoint, Card);
                        }
                        if (gameObject != null)
                        {
                            Destroy(gameObject);
                        }
                        if (collision.gameObject != null)
                        {
                            Destroy(collision.gameObject);
                        }

                        if (ScoreManager.Instance != null)
                        {
                            ScoreManager.Instance.AddCombo();
                            ScoreManager.Instance.AddPoints(10);
                        }

                        
                    }
                }
                if (Card.Rank == 14 && Card.Rank == otherCard.Rank)
                {
                    ScoreManager.Instance.Add10K(10000);
                }
            }
        }

        public void SetScaleBasedOnRank()
        {
            if (transform == null)
            {
                Debug.LogWarning("Transform is null, skipping scaling.");
                return;
            }

            transform.localScale = Vector3.zero;
            // var scale = 0.6f + (Card.Rank - 2) * 0.1f;
            var scale = 0.7f;

            if (transform != null)
            {
                transform.DOScale(new Vector3(scale, scale, 1), 0.1f).OnKill(() =>
                {
                    if (transform != null)
                    {
                        transform.localScale = new Vector3(scale, scale, 1);
                    }
                });
            }
        }

        public void ApplySprite()
        {
            var spriteName = $"{Card.Rank}{Card.Suit[0].ToString().ToLower()}";
            var cardSprite = Resources.Load<Sprite>($"Sprites/{spriteName}");
            if (cardSprite != null)
            {
                GetComponent<SpriteRenderer>().sprite = cardSprite;
                // Debug.Log($"Sprite {spriteName} successfully applied.");
            }
            else
            {
                Debug.LogError($"Sprite {spriteName} not found in Resources/Sprites");
            }
        }
    }
}
