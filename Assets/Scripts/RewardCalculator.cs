using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardCalculator
{
    private int onWin = 1000;
    private int onLose = 100;

    public RewardCalculator(BattleWarCondition condition, GameObject winMenu)
    {
        condition.OnEndGame += AddSoftCost;
    }

    private void AddSoftCost(bool won)
    {
        var money = won ? onWin : onLose;
        PlayerData.Instance.SoftCost += money;
        BattleWarUI.Instance.battleResultsScreen.Show(won, money);
    }

}
