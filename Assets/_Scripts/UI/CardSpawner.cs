using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSpawner : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab = default;

    /// <summary>
    /// Create instance of card on the scene
    /// </summary>
    /// <param name="cardHolder">Parent game object of instantinated card</param> 
    public CardController InstantiateCard(Transform cardHolder, Card card)
    {
        var currentCard = Instantiate(cardPrefab);
        currentCard.name = card.name;
        var cardController = currentCard.AddComponent<CardController>();
        cardController.Initialize(card);
        currentCard.transform.SetParent(cardHolder);
        currentCard.transform.localScale = Vector3.one;
        return cardController;
    }



    
}
