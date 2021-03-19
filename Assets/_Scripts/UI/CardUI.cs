using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CardUI : MonoBehaviour
{  
    [SerializeField] private Text titleText = default;

    [SerializeField] private Text descriptionText = default;

    [SerializeField] private Text manaText = default;

    [SerializeField] private Text attackText = default;

    [SerializeField] private Text hpText = default;

    [SerializeField] private GameObject glowParticle = default;

    /// <summary>
    /// Change health value with counter animation
    /// </summary>  
    /// <param name="oldValue">Old (current) value</param> 
    /// <param name="newValue">New value</param> 
    /// <param name="callback">Actions after counter animation</param> 
    public IEnumerator ChangeHpValue(int oldValue, int newValue, System.Action callback)
    {
        yield return StartCoroutine(ChangeValue(oldValue, newValue, hpText, callback));
    }

    /// <summary>
    /// Change mana value with counter animation
    /// </summary>  
    /// <param name="oldValue">Old (current) value</param> 
    /// <param name="newValue">New value</param> 
    /// <param name="callback">Actions after counter animation</param> 
    public IEnumerator ChangeManaValue(int oldValue, int newValue, System.Action callback)
    {
        yield return StartCoroutine(ChangeValue(oldValue, newValue, manaText, callback));
    }

    /// <summary>
    /// Change attack value with counter animation
    /// </summary>  
    /// <param name="oldValue">Old (current) value</param> 
    /// <param name="newValue">New value</param> 
    /// <param name="callback">Actions after counter animation</param> 
    public IEnumerator ChangeAttackValue(int oldValue, int newValue, System.Action callback)
    {
        yield return StartCoroutine(ChangeValue(oldValue, newValue, attackText, callback));
    }

    private CardController cardController;

    /// <summary>
    /// Initialize card UI
    /// </summary>  
    /// <param name="card">Card model for initialize</param> 
    /// <param name="cardController">Card controller of this card</param> 
    public void Initialize(Card card, CardController cardController)
    {
        LoadArtImage();
        SetTexts(card);
        this.cardController = cardController;
    }

    /// <summary>
    /// Change value of the card with counter animation at Text component
    /// </summary>  
    /// <param name="oldValue">Old value</param> 
    /// <param name="newValue">New value</param> 
    /// <param name="Text">Text component where value displayed</param> 
    private IEnumerator ChangeValue(int oldValue, int newValue, Text textUi, System.Action callback)
    {
        glowParticle.SetActive(true);
       
        yield return new WaitForSeconds(0.4f);
        while (oldValue != newValue)
        {
            oldValue = oldValue > newValue ? oldValue - 1 : oldValue + 1;
            textUi.text = oldValue.ToString();
            yield return new WaitForSeconds(0.2f);
        }
       
        glowParticle.SetActive(false);

        callback?.Invoke();
    }

    /// <summary>
    /// Load random card-art image from https://picsum.photos
    /// </summary> 
    private void LoadArtImage()
    {
        StartCoroutine(FileDownloader.DownloadImage("https://picsum.photos/183/156", (downloadedTexture) =>
        {
            transform.Find("ArtImage").GetComponent<Image>().sprite =
                Sprite.Create(downloadedTexture, new Rect(0, 0, downloadedTexture.width, downloadedTexture.height), new Vector2(downloadedTexture.width / 2, downloadedTexture.height / 2));
        }));
    }

    /// <summary>
    /// Initialize Text components based on card values
    /// </summary>  
    private void SetTexts(Card card)
    {
        titleText.text = card.Title;
        descriptionText.text = card.Description;
        manaText.text = card.ManaValue.ToString();
        attackText.text = card.AttackValue.ToString();
        hpText.text = card.HpValue.ToString();
    }

   
}
