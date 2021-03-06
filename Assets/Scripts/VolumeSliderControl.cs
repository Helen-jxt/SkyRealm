﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderControl : MonoBehaviour
{
    public Slider slider;

    private void Start()
    {
        slider.value = PlayerPrefs.GetFloat("volume", 5f);
    }

    public void setVolume(float s)
    {
        PlayerPrefs.SetFloat("volume", s);
    }
}
