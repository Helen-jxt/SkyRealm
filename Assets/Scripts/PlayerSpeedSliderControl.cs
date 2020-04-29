using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpeedSliderControl : MonoBehaviour
{
    public Slider slider;
    public PlayerControl player;

    private void Start()
    {
        slider.value = PlayerPrefs.GetFloat("palyerSpeed", 5f);
    }

    public void setPlayerMovementSpeed(float s) 
    {
        if (player != null)
        {
            player.moveSpeed = s;
        }
        PlayerPrefs.SetFloat("palyerSpeed", s);
    }
}
