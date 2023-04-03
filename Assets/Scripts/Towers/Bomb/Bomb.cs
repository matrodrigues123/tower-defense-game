using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Tower
{
    protected override void UpdateNearestEnemy()
    {
        foreach (GameObject enemy in Enemies.enemies)
        {
            if (transform.position == enemy.transform.position)
            {
                currentTarget = enemy;
            }
        }

    }

    protected override void Shoot()
    {
        if (currentTarget != null)
        {
            Enemy currEnemy = currentTarget.GetComponent<Enemy>();
            if (!currEnemy.flyer)
            {
                currEnemy.TakeDamage(damage);
                Towers.towers.Remove(gameObject);
                Destroy(gameObject);
            }
        }

    }
}
