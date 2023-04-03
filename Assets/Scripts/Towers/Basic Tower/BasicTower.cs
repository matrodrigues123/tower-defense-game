using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTower : Tower
{
    public Transform pivot;
    public Transform barrel;
    public GameObject bulllet;
    protected override void Shoot()
    {
        base.Shoot();
        if (currentTarget != null)
        {
            GameObject newBullet = Instantiate(bulllet, barrel.position, pivot.rotation);
        }
    }
}
