using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "DeckData", menuName = "ScriptableObjects/CardsInDeck", order = 1)]
public class Deck : ScriptableObject
{
    [SerializeField] private Card[] deck = default;
   
    /// <summary>
    /// Get random cards from deck
    /// </summary>
    /// <param name="minAmount">Minimum amount of getted cards (included)</param>
    /// <param name="maxAmount">Maximum amount of getted cards (included)</param>
    public List<Card> GetCards(int minAmount, int maxAmount)
    {
        return deck.OrderBy(x => Random.Range(0, int.MaxValue)).Take(Random.Range(minAmount, maxAmount + 1)).ToList();

        
    }

    
}
