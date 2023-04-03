using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{

    public int GetTowerCost(GameObject towerPrefab)
    {
        Tower towerProps = towerPrefab.GetComponent<Tower>();
        return towerProps.towerCost;
    }
    public bool CanBuyTower(GameObject towerPrefab)
    {
        int cost = GetTowerCost(towerPrefab);
        return MoneyManager.GetCurrentMoney() >= cost;
    }

    public void BuyTower(GameObject towerPrefab)
    {
        MoneyManager.RemoveMoney(GetTowerCost(towerPrefab));
    }
}
