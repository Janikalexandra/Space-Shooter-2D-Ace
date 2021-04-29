using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shield : MonoBehaviour
{
    public Slider _shieldSlider;


    public void SetMaxHealth(int health)
    {
        _shieldSlider.maxValue = health;
        _shieldSlider.value = health;
    }

    public void SetHealth(int health)
    {
        _shieldSlider.value = health;
    }
}
