using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Image healthBar;
    public Text healthText;

    public void Update()
    {
        healthBar.fillAmount = PlayerHealth.GetCurrentPlayerHealth()/PlayerHealth.startingHealth;
        healthText.text = Mathf.Floor(PlayerHealth.GetCurrentPlayerHealth()) + "/" + PlayerHealth.startingHealth;
    }
}
