using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Shop : MonoBehaviour
{
    [SerializeField] private Transform shopPanel;
    [SerializeField] private Transform cardContent;
    [SerializeField] private GameObject cardPrefab;

    [Header("FOR TEST")]
    [SerializeField] private TeamData playerTeam;
    [SerializeField] private string fractionName;

    /// <summary>
    /// Карточки UI и их ценники
    /// </summary>
    private Dictionary<ShopCardUI,int> cards;
    private ShopCardUI selectedCard;


    public void Start()
    {
        Init(fractionName); //Для теста
    }

    public void Init(string selectedFraction)
    {
        InitCards(selectedFraction);
    }

    /// <summary>
    /// Инстацирует все варианты юнитов текущей фракции (НЕ ТИМЫ), затем для показателя
    /// уже имеющегося кол-ва идет инициализация юнитов тимы
    /// </summary>
    /// <param name="selectedFraction"></param>
    private void InitCards(string selectedFraction)
    {
        cards = new Dictionary<ShopCardUI, int>();

        var fractionData = FractionData.GetUnitsDataFromFraction(selectedFraction);
        foreach (var unitData in fractionData)
        {
          var card = Instantiate(cardPrefab, cardContent).GetComponent<ShopCardUI>();
            cards[card] = unitData.cost;

            int countUnitsInTeam = 0;
            if (playerTeam.GetAllAvailableUnits().ContainsKey(unitData.prefab))
            {
                countUnitsInTeam = playerTeam.GetAllAvailableUnits()[unitData.prefab];
            }

            card.Init(this,unitData.prefab,unitData.name, unitData.cost, countUnitsInTeam);
        }
      //  selectedCard = cards[0].gameObject.GetComponent<ShopCardUI>();
      //  selectedCard.ActiveCard();
    }

    /// <summary>
    /// Вызов идет как только палец касается любой из карточки
    /// </summary>
    /// <param name="eventData"></param>
    public void ControlActiveCard(PointerEventData eventData)
    {
        var newCard = eventData.pointerCurrentRaycast.gameObject.GetComponent<ShopCardUI>();
        if (selectedCard != newCard)
        {
            if (selectedCard) selectedCard.InactiveCard();
            selectedCard = newCard;
            selectedCard.ActiveCard();
            selectedCard.ResetBuyDelay();
        }
        else
        {
            selectedCard.ResetBuyDelay();
        }
    }

    /// <summary>
    /// Вызов идет через касание карточки, также при удержании, но в этом случае проходит через контроль ускорителя
    /// </summary>
    public void BuyNewUnit(ShopCardUI card)
    {
        if(cards[card] <= PlayerData.Instance.SoftCost)
        {
            PlayerData.Instance.SoftCost -= cards[card];
            playerTeam.AddUnit(card.UnitPrefab);
            card.UpdTextInfo();
            card.BuyAnimation();
            ControlAllIntearctables();
        }
    }


    private void ControlAllIntearctables()
    {
        foreach(var keyValue in cards)
        {
            bool isNoMoney = keyValue.Value > PlayerData.Instance.SoftCost;
            keyValue.Key.Interactable(isNoMoney);
        }
    }
}
