using System;
using System.Linq;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Main UI Script of the current buttle Mode!
/// ��� ������ Init(...) �� ����� ��������
/// </summary>
public class BattleWarUI : MonoBehaviour
{
    private static BattleWarUI _instance;
    public static BattleWarUI Instance => _instance;

    [Header("LevelManager references")]

    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform cardParent;

    [SerializeField] private Image leftPlayerFillImage;
    [SerializeField] private Image rightPlayerFillImage;
    [SerializeField] private Image timerFillImage;

    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI lvlLeftPlayerText;
    [SerializeField] private TextMeshProUGUI lvlRightPlayerText;
    [SerializeField] private TextMeshProUGUI NoxText;
    [SerializeField] private TextMeshProUGUI SoftCostText;

    public BattleResultsScreen battleResultsScreen;
    // Да простит меня господь. И вы простите
    // Если сдадим MVP, уберите
    public int cardHierarchyOffset;

    private SlideBar leftPlayerSlideBar;
    private SlideBar rightPlayerSlideBar;
    private SlideBar timerSlideBar;

    private UnitCreator unitCreator;
    private GameTimer timer;

    private TeamManager leftTeam;
    private TeamManager rightTeam;

    private BattleWarCardUI[] cards;

    private void Awake()
    {
        _instance = this;
    }

    private void Update()
    {
        if(Input.GetKeyDown("w")) battleResultsScreen.Show(true, 0);
        if(Input.GetKeyDown("l")) battleResultsScreen.Show(false, 0);
    }

    /// <summary>
    /// ����������� ������� � ������ ���� � ����� ���������
    /// </summary>
    public void Init(TeamData leftData, TeamData rightData, TeamManager leftTeam, 
        TeamManager rightTeam, GameTimer gameTimer, UnitCreator unitCreator, BattleWarCondition matchCondition)
    {
        this.timer = gameTimer;
        this.leftTeam = leftTeam;
        this.rightTeam = rightTeam;
        this.unitCreator = unitCreator;
        

        InitSlideBars(matchCondition.LeftCommonPoints,matchCondition.RightCommonPoints);
        InitAllCards(leftData.GetAllAvailableUnits());

        matchCondition.OnDecreaseLeftPoints += (currentPoints) => UpdSlideBar(leftPlayerSlideBar, currentPoints);
        matchCondition.OnDecreaseRightPoints += (currentPoints) => UpdSlideBar(rightPlayerSlideBar, currentPoints);


        gameTimer.OnEverySecond += (currentTime) => UpdSlideBar(timerSlideBar, (int)currentTime.TotalSeconds);
        gameTimer.OnEverySecond += (currentTime) => timeText.text = $"{currentTime.Minutes}:{currentTime.Seconds}";
    }

    public void UpdSlideBar(SlideBar slider,int currentPoints)
    {
        if (slider.sliding != null) StopCoroutine(slider.sliding);
        slider.sliding = StartCoroutine(slider.Sliding(currentPoints));
    }

    public void SetAllButtonsInteractable()
    {
        foreach (BattleWarCardUI card in cards)
        {
            card.SetAwaitMode();
        }
    }

    public void RemoveCardUI(BattleWarCardUI card)
    {
       var array = cards.ToList();
       array.Remove(card);
       cards = array.ToArray();
       Destroy(card.gameObject, 0.5f);
    }

    private void InitAllCards(Dictionary<Unit, int> allUnits)
    {
        List<BattleWarCardUI> newList = new List<BattleWarCardUI>();
        foreach (KeyValuePair<Unit, int> unit in allUnits)
        {
            var newCard = Instantiate(cardPrefab, cardParent);
            var cardTransform = newCard.transform;
            var cardUI = newCard.GetComponent<BattleWarCardUI>();

            cardTransform.SetSiblingIndex(cardTransform.GetSiblingIndex() + cardHierarchyOffset);

            FractionData.UnitData unitData;

            if (FractionData.TryGetUnitDataFromAllFractions(unit.Key, out unitData))
            {
                cardUI.InitCard(this, unitCreator, unit.Key.gameObject, unit.Value, unitData);
            }
            else
            {
                Debug.LogError("!!!������� ������ �� ������� �� � ���� �������!!!");
            }

            newList.Add(cardUI);
        }
        cards = newList.ToArray();

    }

    private void InitSlideBars(int leftPoints,int rightPoints)
    {

        timerSlideBar = new SlideBar((float)timer.CurrentTime.TotalSeconds,timerFillImage);
        leftPlayerSlideBar = new SlideBar(leftPoints, leftPlayerFillImage,500);
        rightPlayerSlideBar = new SlideBar(rightPoints,rightPlayerFillImage,500);

    }


    private void SubscribeOnDeaths(TeamManager team, SlideBar slideBar)
    {
        team.OnAddUnit += (unit) => (unit.Health as Health).OnDead += () => SlidingBar(slideBar, slideBar.targetPoints - unit.Health.Points);
    }

    private void SlidingBar(SlideBar slideBar,float currentHealth)
    {
        if (slideBar.sliding != null) StopCoroutine(slideBar.sliding);
        slideBar.sliding = StartCoroutine(slideBar.Sliding(currentHealth));
    }



    private void OnDisable()
    {
        PlayerData.Instance.OnChangeNox -= SubscribeNox;  
        PlayerData.Instance.OnChangeSoftCost -= SubscribeSoftCost;

    } 
    private void OnEnable()
    {
        PlayerData.Instance.OnChangeNox += SubscribeNox;
        PlayerData.Instance.OnChangeSoftCost += SubscribeSoftCost;
        NoxText.text = PlayerData.Instance.Nox.ToString();
        SoftCostText.text = PlayerData.Instance.SoftCost.ToString();
    }

    private void SubscribeNox(int count)
    {
        NoxText.text = count.ToString();
    }
    private void SubscribeSoftCost(int count)
    {
        SoftCostText.text = count.ToString();
    }
}
