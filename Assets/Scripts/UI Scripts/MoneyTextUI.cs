using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyTextUI : MonoBehaviour
{
    public Text moneyText;
    void Update()
    {
        moneyText.text = "$ " + MoneyManager.GetCurrentMoney();
    }
}
