using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopCardUI : MonoBehaviour
{
    [SerializeField] public UITriggerHandler touchEvent;

    [SerializeField] private TextMeshProUGUI unitNameText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI currentTeamUnitsCountText;

    [Header("Click buy cards Parameters")]
    [Tooltip("задержка покупки при удерживании карточки")]
    [SerializeField] private float maxBuyDelay;
    [Tooltip("задержка покупки при удерживании карточки")]
    [SerializeField] private float minBuyDelay;
    [Tooltip("вычитается из текущего delay при удерживании карточки, что ускоряет саму покупку")]
    [SerializeField] private float decreaseDelaySpeed;

    private Shop shop;
    private float currentBuyDelay;
    private float currentFrame;
    public Unit UnitPrefab { get; set; }

    private bool IsCooldown {
        get { return (currentFrame <= 0) ? false : true; } }
    private int unitCount;
    public void Init(Shop shop,Unit unitPrefab,string unitName,int unitCost, int currentTeamUnitCount)
    {
        this.shop = shop;
        this.unitCount = currentTeamUnitCount;
        unitNameText.text = unitName;
        UnitPrefab = unitPrefab;
        costText.text = unitCost.ToString();
        currentTeamUnitsCountText.text = currentTeamUnitCount.ToString();
        touchEvent.OnBeginEvent += shop.ControlActiveCard;
        ResetBuyDelay();
    }

    /// <summary>
    /// Когда игрок переключился на другую карту. Если игрок щелкает на одну и ту же- активность не меняется!
    /// </summary>
    public void ActiveCard()
    {
        touchEvent.OnPressEvent += PressCard;
    }
    /// <summary>
    /// Когда игрок переключился на другую карту. Если игрок щелкает на одну и ту же- активность не меняется!
    /// </summary>
    public void InactiveCard()
    {
        touchEvent.OnPressEvent -= PressCard;
    }

    /// <summary>
    /// При зажатии пальца по карточке, если палец отпустить параметры ускорителя ресетнутся
    /// </summary>
    /// <param name="eventData"></param>
    public void PressCard(PointerEventData eventData)
    {
        if (!IsCooldown)
        {
            shop.BuyNewUnit(this);
            currentFrame = currentBuyDelay;
            if (currentBuyDelay > minBuyDelay)
            {
                currentBuyDelay -= decreaseDelaySpeed;
            }
            else
            {
                currentBuyDelay = minBuyDelay;
            }
        }
        currentFrame -= Time.deltaTime;


    }
    public void ResetBuyDelay()
    {
        currentBuyDelay = maxBuyDelay;
        currentFrame = 0;
    }

    /// <summary>
    /// вызов либо просто при нажатии, либо при удержании
    /// </summary>
    public void UpdTextInfo()
    {
        unitCount++;
        currentTeamUnitsCountText.text = unitCount.ToString();
    }

    /// <summary>
    /// вызов либо просто при нажатии, либо при удержании
    /// </summary>
    public void BuyAnimation()
    {

    }

    /// <summary>
    /// Если денег на карточку не хватает, она переходит в InteractableOFF
    /// </summary>
    /// <param name="isInteractable"></param>
    public void Interactable(bool isInteractable)
    {

    }

}
