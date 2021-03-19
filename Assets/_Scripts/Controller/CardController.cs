using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    public bool IsDrag { get { return mouseDragCard.IsCardGetForDrag; } }

    private Card card;
    private CardUI cardUI;
    private MouseDragCard mouseDragCard;  

    public event System.Action<CardController> OnCardDefeat;
    public event System.Action<CardController> OnBeforeValuesChange;
    public event System.Action<CardController> OnAfterValuesChange;


    public int Mana
    {
        get { return card.ManaValue; }
        set
        {
            if (value != card.ManaValue)
            {
                OnBeforeValuesChange?.Invoke(this);
                StartCoroutine(cardUI.ChangeManaValue(card.ManaValue, value,() => OnAfterValuesChange?.Invoke(this)));
                card.ManaValue = value;
            }
        }
    }

    public int Attack
    {
        get { return card.AttackValue; }
        set
        {
            if (value != card.AttackValue)
            {
                OnBeforeValuesChange?.Invoke(this);
                StartCoroutine(cardUI.ChangeAttackValue(card.AttackValue, value, () => OnAfterValuesChange?.Invoke(this)));
                card.AttackValue = value;
            }
        }
    }

    public int HP
    {
        get { return card.HpValue; }
        set
        {
            if (value != card.HpValue)
            {
                OnBeforeValuesChange?.Invoke(this);
                StartCoroutine(cardUI.ChangeHpValue(card.HpValue, value, () =>
                {
                    OnAfterValuesChange?.Invoke(this);
                    if (value < 1)
                        OnCardDefeat?.Invoke(this);
                    
                }));
                card.HpValue = value;

            }
        }
    }

    /// <summary>
    /// Initialize controller of current card
    /// </summary>  
    /// <param name="card">Card model for initialize</param> 
    public void Initialize(Card card)
    {
        this.card = card;
        cardUI = GetComponent<CardUI>();
        mouseDragCard = GetComponent<MouseDragCard>();
        cardUI.Initialize(card, this);        
    }

    /// <summary>
    /// Change random value at this card from -2 to 9 
    /// </summary>    
    public void CardAction()
    {
        int newRandomValue;

        switch (Random.Range(0, 3))
        {
            case 0:
                do
                {
                    newRandomValue = Random.Range(-2, 10);
                } while (Mana == newRandomValue);
                Mana = newRandomValue;
                break;
            case 1:
                do
                {
                    newRandomValue = Random.Range(-2, 10);
                } while (Attack == newRandomValue);
                Attack = newRandomValue;
                break;
            case 2:
                do
                {
                    newRandomValue = Random.Range(-2, 10);
                } while (HP == newRandomValue);
                HP = newRandomValue;
                break;
        }
    }

    /// <summary>
    /// Clear events to avoid memory leak when object destroyed
    /// </summary>  
    private void OnDestroy()
    {
        foreach (System.Delegate d in OnCardDefeat.GetInvocationList())
            OnCardDefeat -= (System.Action<CardController>)d;
        foreach (System.Delegate d in OnBeforeValuesChange.GetInvocationList())
            OnBeforeValuesChange -= (System.Action<CardController>)d;
        foreach (System.Delegate d in OnAfterValuesChange.GetInvocationList())
            OnAfterValuesChange -= (System.Action<CardController>)d;
    }
}
