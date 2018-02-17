using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarScript : MonoBehaviour {

    [SerializeField]
    private Image HealthContent;

    [SerializeField]
    private Text healthText;

    [SerializeField]
    private Image ManaContent;

    [SerializeField]
    private Text manaText;

    private float percentage;

    // Use this for initialization
    void Start()
    {
        string[] temp = manaText.text.Split(':');
        manaText.text = temp[0] + ":" + " 100";//+ playerStats.MaxHealth;

        string[] temp2 = healthText.text.Split(':');
        healthText.text = temp2[0] + ":" + " 100";//+ playerStats.MaxMana;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBar();
    }

    private void UpdateBar()
    {
        /*if (playerStats.GetManaPercentage() != ManaContent.fillAmount)
        {
            ManaContent.fillAmount = playerStats.GetManaPercentage();
            string[] temp = manaText.text.Split(':');
            manaText.text = temp[0] + ": " + GetGameobject.GetPlayerStats().GetCurrentMana();
            if (playerStats.GetCurrentMana() != playerStats.MaxMana)
            {
                manaText.text += " [" + playerStats.GetManaRegenPerSecond() + "]";
            }
        }
        if (playerStats.GetHealthPercentage() != HealthContent.fillAmount)
        {
            HealthContent.fillAmount = playerStats.GetHealthPercentage();
            string[] temp = healthText.text.Split(':');
            healthText.text = temp[0] + ": " + GetGameobject.GetPlayerStats().GetHealth();
            if (playerStats.GetHealth() != playerStats.MaxHealth)
            {
                healthText.text += " [" + playerStats.GetHealthRegenPerSecond() + "]";
            }
        }*/
    }
}
