using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{

    private static int currentPlayerMoney;
    public int starterMoney;

    public void Start()
    {
        currentPlayerMoney = starterMoney;
    }

    public static int GetCurrentMoney()
    {
        return currentPlayerMoney;
    }
    
    public static void AddMoney(int amount)
    {
        currentPlayerMoney += amount;
    }
    public static void RemoveMoney(int amount)
    {
        currentPlayerMoney -= amount;
    }
}
