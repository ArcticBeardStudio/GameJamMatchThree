using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUI : MonoBehaviour
{

    public Text RedGemText;
    public Text GreenGemText;
    public Text BlueGemText;
    public Text ShieldText;
    public Button SpecialAttackButton;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void Init(int redValue, int greenValue, int blueValue, int shieldValue)
    {
        UpdateGemText(redValue, TileTypes.Red);
        UpdateGemText(greenValue, TileTypes.Green);
        UpdateGemText(blueValue, TileTypes.Blue);
        UpdateShieldText(shieldValue);
    }

    public void UpdateGemText(int newValue, TileTypes tileType)
    {
        if(newValue<0)
        {
            newValue = 0;
        }
        switch(tileType)
        {
            case TileTypes.Red:
                RedGemText.text = newValue.ToString();
                break;
            case TileTypes.Green:
                GreenGemText.text = newValue.ToString();
                break;
            case TileTypes.Blue:
                BlueGemText.text = newValue.ToString();
                break;
        }
    }

    public void UpdateShieldText(int newValue)
    {
        if (newValue < 0)
        {
            newValue = 0;
        }
        ShieldText.text = newValue.ToString();
    }
}
