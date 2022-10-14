using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ����������� ������� InitCard ��� ��������������� ������� ��������
/// </summary>
public class BattleWarCardUI : MonoBehaviour
{
    [HideInInspector] public GameObject UnitPrefab => unitPrefab;

    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private TextMeshProUGUI unitNameText;
    [SerializeField] private Image modeImage;
    [SerializeField] private Image avatar;
    /// <summary>
    /// ���-�� � ����� ������
    /// </summary>
    public int SpawnCount { get; set; } = 1;
    private int count;
    /// <summary>
    /// ���������� ������� �� ��������
    /// </summary>
    public int Count
    {
        get => count;
        set
        {
            count = value;
            countText.text = count.ToString();
            if(count <= 0)
            {
                warUI.RemoveCardUI(this);
            }
        }
    }
    public string UnitName
    {
        get; set;
    }

    private GameObject unitPrefab;
    private BattleWarUI warUI;
    private UnitCreator unitCreator;


    /// <summary>
    /// ������ ��������� � ����� �������� ������� ��� ���������
    /// </summary>
    public void SetAwaitMode()
    {
        button.interactable = true;
        unitCreator.StopCreator();
    }

    public void InitCard(BattleWarUI warUI,UnitCreator unitcreator, GameObject unitPrefab,int count,FractionData.UnitData unitData)
    {
        this.count = count;
        this.warUI = warUI;
        this.unitPrefab = unitPrefab;
        this.modeImage.sprite = unitData.mode;
        this.avatar.sprite = unitData.avatar;
        UnitName = unitData.name;
        this.unitNameText.text = unitData.name;
        this.countText.text = count.ToString();
        this.unitCreator = unitcreator;
	    this.SpawnCount = unitData.count;

        button.onClick.AddListener(() => OnButtonClicked());
    }

    private void OnButtonClicked()
    {
        //this.transform.SetSiblingIndex(0);
        warUI.SetAllButtonsInteractable();
        button.interactable = false;
        button.GetComponent<Animator>().SetTrigger("Pressed");
        unitCreator.StartCreator(this);
    }
    

}
