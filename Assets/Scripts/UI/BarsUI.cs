using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarsUI : MonoBehaviour {

    [SerializeField]
    private Image HealthContent;

    [SerializeField]
    private Text healthText;

    [SerializeField]
    private Image ManaContent;

    [SerializeField]
    private Text manaText;

    private float percentage;

    private Player player;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitBars(Player newPlayer)
    {
        player = newPlayer;
        UpdateManaBar();
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        HealthContent.fillAmount = player.GetHealthPercentage();
        string[] temp = healthText.text.Split(':');
        healthText.text = temp[0] + ": " + player.GetCurrentHealth();
    }

    public void UpdateManaBar()
    {
        ManaContent.fillAmount = player.GetManaPercentage();
        string[] temp = manaText.text.Split(':');
        manaText.text = temp[0] + ": " + player.GetCurrentMana();
    }
}