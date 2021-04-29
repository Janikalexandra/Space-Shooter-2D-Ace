using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    public int shieldHealth;

    public void SetMaxHealth(int shieldHealth)
    {
        slider.maxValue = shieldHealth;
        slider.value = shieldHealth;
    }

    public void SetHealth(int shieldHealth)
    {
        slider.value = shieldHealth;
    }
}
