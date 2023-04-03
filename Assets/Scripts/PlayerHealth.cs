using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private static float currentPlayerHealth;

    public static float startingHealth;

    public void Start()
    {
        startingHealth = 100;
        ResetPlayerHealth();
    }

    public void ResetPlayerHealth()
    {
        currentPlayerHealth = startingHealth;
    }

    public static void DamagePlayer(float amt)
    {
        currentPlayerHealth -= amt;
    }

    public static float GetCurrentPlayerHealth()
    {
        return currentPlayerHealth;
    }
}
