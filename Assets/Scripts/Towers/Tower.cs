using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private float range;
    [SerializeField] public float damage;
    [SerializeField] private float timeBetweenShots;
    [SerializeField] public bool isVisibleToEnemy;
    [SerializeField] public int towerCost;
    private float nextTimeToShoot;

    public GameObject currentTarget;

    private void Awake()
    {
        Towers.towers.Add(gameObject);
    }

    protected virtual void UpdateNearestEnemy()
    {
        GameObject currentNearestEnemy = null;
        float distance = Mathf.Infinity; 
        foreach (GameObject enemy in Enemies.enemies)
        {
            float currDistance = (transform.position - enemy.transform.position).magnitude;
            if (currDistance < distance)
            {
                distance = currDistance;
                currentNearestEnemy = enemy;
            }
        }

        if (distance <= range)
        {
            currentTarget = currentNearestEnemy;
        }
        else
        {
            currentTarget = null;
        }
    }

    protected virtual void Shoot()
    {
        if (currentTarget != null)
        {
            Enemy enemyScript = currentTarget.GetComponent<Enemy>();
            enemyScript.TakeDamage(damage);
        }
    }

    private void Update()
    {
        UpdateNearestEnemy();
        if (Time.time >= nextTimeToShoot)
        {
            Shoot();
            nextTimeToShoot = timeBetweenShots + Time.time;
        }
    }
}
