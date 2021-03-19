using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextButton : MonoBehaviour
{
    private Button nextButton;

    [SerializeField] private CardsInHand cardsInHand = default;

    private void Start()
    {
        nextButton = GetComponent<Button>();
        nextButton.onClick.AddListener(OnClick);
    }

    /// <summary>
    /// This method will be called when user click button
    /// </summary>   
    private void OnClick()
    {        
        cardsInHand.GetNextCard().CardAction();
    }

}
