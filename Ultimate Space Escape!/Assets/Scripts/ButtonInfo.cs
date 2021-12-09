using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonInfo : MonoBehaviour, IPointerExitHandler

{
    public int ItemID;
    public Text PriceText;
    public Text QuantityText;
    public GameObject InfoPanel;
    public GameObject GameManager;
    public GameObject soldOutImage;

    public int[,] shopItems;

    void Start()
    {
        if (soldOutImage != null)
        {
            soldOutImage.SetActive(false);
        }
    }
    void Update()
    {
        //Updates Quantity and Price texts to coincide with player purchase
        if (QuantityText != null)
        {
            QuantityText.text = GameManager.GetComponent<GameManager>().shopItems[3, ItemID].ToString();
        }
        
        if (PriceText != null)
        {
            PriceText.text = "Price: $" + GameManager.GetComponent<GameManager>().shopItems[2, ItemID].ToString();
        }

        if (GameManager.GetComponent<GameManager>().shopItems[2, ItemID] == 0)
        {
            if (soldOutImage != null)
            {
                soldOutImage.SetActive(true);
                PriceText.enabled = false;
            }
        }
    }



    public void InfoPanelShow()
    {
        InfoPanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        InfoPanel.SetActive(false);
    }
}
