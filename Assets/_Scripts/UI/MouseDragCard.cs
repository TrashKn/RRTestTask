using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

[RequireComponent(typeof(CanvasGroup))]
public class MouseDragCard : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public bool IsCardGetForDrag { get; private set; }

    [SerializeField] private GameObject glowParticle = default;

    [SerializeField] private GameObject shineParticle = default;

    private CardsInHand cardsInHand;
    private CanvasGroup canvasGroup;    
    
    private RectTransform rect;
    private CardController cardController;

    private Vector3 lastMousePosition;

    void Start()
    {
        cardsInHand = transform.parent.GetComponent<CardsInHand>();
        cardController = GetComponent<CardController>();
        canvasGroup = GetComponent<CanvasGroup>();        
        rect = GetComponent<RectTransform>();
        IsCardGetForDrag = false;
    }  

    /// <summary>
    /// This method will be called on the start of the mouse drag
    /// </summary>
    /// <param name="eventData">mouse pointer event data</param>
    public void OnBeginDrag(PointerEventData eventData)
    {
      
        lastMousePosition = eventData.position;
        canvasGroup.blocksRaycasts = false;
        shineParticle.SetActive(true);
        IsCardGetForDrag = true;
        
        cardsInHand.RepositionCards();
    }

    /// <summary>
    /// This method will be called during the mouse drag
    /// </summary>
    /// <param name="eventData">mouse pointer event data</param>
    public void OnDrag(PointerEventData eventData)
    {
        transform.rotation = Quaternion.identity;
        cardsInHand.BlockButton();
        Vector3 oldPos = rect.position;
        rect.position = eventData.position;

        if (eventData.hovered.Exists(a => a.name == "DropCardPanel"))
            glowParticle.SetActive(true);
        else
            glowParticle.SetActive(false);

        if (!IsRectTransformInsideSreen(rect))
        {
            rect.position = oldPos;
        }
        lastMousePosition = eventData.position;
    }

    /// <summary>
    /// This method will be called at the end of mouse drag
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        IsCardGetForDrag = false;
        shineParticle.SetActive(false);
        glowParticle.SetActive(false);
        if (eventData.hovered.Exists(a => a.name == "DropCardPanel"))
        {
            cardsInHand.DropCard(cardController);           
            Destroy(this);            
        }
        else
        {
            cardsInHand.RepositionCards();
            canvasGroup.blocksRaycasts = true;
        }
       
    }

    /// <summary>
    /// This methods will check is the rect transform is inside the screen or not
    /// </summary>
    /// <param name="rectTransform">Rect Trasform</param>
    /// <returns></returns>
    private bool IsRectTransformInsideSreen(RectTransform rectTransform)
    {
        bool isInside = false;
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        int visibleCorners = 0;
        Rect rect = new Rect(0, 0, Screen.width, Screen.height);
        foreach (Vector3 corner in corners)
        {
            if (rect.Contains(corner))
            {
                visibleCorners++;
            }
        }
        if (visibleCorners == 4)
        {
            isInside = true;
        }
        return isInside;
    }   
}
