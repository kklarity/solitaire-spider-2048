using UnityEngine;

namespace _Project.Scripts.Card
{
    public class Card
    {
        public string Suit { get; set; }
        public int Rank { get; set; }

        public Card(string suit, int rank)
        {
            Suit = suit;
            Rank = rank;
        }
    }
}