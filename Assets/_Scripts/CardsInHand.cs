using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using DigitalRuby.Tween;
using UnityEngine.UI;

public class CardsInHand : MonoBehaviour
{

    [SerializeField] private float buttonDistance = 10;
    [SerializeField] private float yOffset = 0.5f;

    [SerializeField] private Deck _deck = default;
    [SerializeField] private Button _nextButton = default;

    [SerializeField] private float tweenTime = default;

    private const int minSize = 4;
    private const int maxSize = 6;

    private int currectCardIndex;
    private int cardWidth = 135;

    private CardSpawner cardSpawner;    
    private List<CardController> cardsInHand = new List<CardController>();
 
    void Start()
    {
        cardSpawner = GetComponent<CardSpawner>();
        GenerateCards();
        RepositionCards();
        currectCardIndex = 0;        
    }   

    /// <summary>
    /// Generate random cards in hand from deck
    /// </summary>  
    private void GenerateCards()
    {
        foreach (var card in _deck.GetCards(minSize, maxSize))
        {
            var currentCard = cardSpawner.InstantiateCard(transform, card);
            currentCard.OnCardDefeat += RemoveCardFromHand;
            currentCard.OnBeforeValuesChange += c => _nextButton.interactable = false;
            currentCard.OnAfterValuesChange += c => _nextButton.interactable = true;
            cardsInHand.Add(currentCard);
        }
    }

    /// <summary>
    /// Calculate positions for cards in hand and move it with tween effect
    /// </summary>  
    public void RepositionCards()
    {
        int cardsCount = cardsInHand.Count(a => !a.IsDrag);

        int index = -cardsCount / 2;
        float startOffset = cardsCount % 2 == 0 ? cardWidth / 2 : 0;

        yOffset = 2f / cardsCount;
        int i = 0;

        foreach (var card in cardsInHand)
        {
            if (card.IsDrag)
                continue;
            var startPos = card.transform.localPosition;

            float angle;
            Quaternion endRotation;
            if (cardsCount > 2)
            {
                angle = -180 / (cardsCount - 1) * Mathf.Deg2Rad;
                endRotation = Quaternion.Euler(0, 0, Mathf.Cos(angle * i) * buttonDistance);
            }
            else
            {
                endRotation = Quaternion.identity;
                angle = 0;
            }

            float x = Mathf.Cos(angle * i) * buttonDistance;
            float y = Mathf.Sin(angle * i) * buttonDistance;

            var endPos = new Vector3(startOffset + cardWidth * index++, 100 - y / yOffset, 0);
            card.gameObject.Tween($"MoveCard_{card.name}", startPos, endPos, tweenTime, TweenScaleFunctions.CubicEaseOut, (t) =>
            {
                card.transform.localPosition = t.CurrentValue;
            });

            var startRotation = card.transform.localRotation;

            card.gameObject.Tween($"RotateCard_{card.name}", startRotation, endRotation, tweenTime, TweenScaleFunctions.CubicEaseOut, (t) =>
            {
                card.transform.localRotation = t.CurrentValue;
            });
            i++;

        }        
        UnlockButton();
    }

    /// <summary>
    /// Drop card at the table
    /// </summary>  
    public void DropCard(CardController card)
    {
        cardsInHand.Remove(card);
        Destroy(card);
        if (cardsInHand.Count == 0)
        {
            _nextButton.interactable = false;
            return;
        }
        RepositionCards();
    }

    /// <summary>
    /// Get next active card
    /// </summary>  
    public CardController GetNextCard()
    {
        if (currectCardIndex == cardsInHand.Count)
            currectCardIndex = 0;
        var nextCard = cardsInHand.Skip(currectCardIndex).First();
        currectCardIndex++;
       
        return nextCard;
    }


    /// <summary>
    /// Block button interacable
    /// </summary>  
    public void BlockButton()
    {
        _nextButton.interactable = false;
    }

    /// <summary>
    /// Unlock button interacable with delay in 1 second
    /// </summary>  
    public void UnlockButton()
    {
        //yield return new WaitForSeconds(1);
        _nextButton.interactable = true;
    }

    /// <summary>
    /// Remove card from hand
    /// </summary>  
    /// <param name="card">Card for remove</param> 
    private void RemoveCardFromHand(CardController card)
    {

        cardsInHand.Remove(card);
        Destroy(card.gameObject);
        if (cardsInHand.Count == 0)
        {
            _nextButton.interactable = false;
            return;
        }

        RepositionCards();
        currectCardIndex--;
    }
}
