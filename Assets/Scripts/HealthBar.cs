using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public HealthSystem healthSystem;
    void Start()
    {
        // set the max value of slider equal to max health of character
        slider.maxValue = healthSystem.healthMax;
        // set the health bar fill equal to characters health (could be lower than max hp)
        slider.value = healthSystem.health;
    }

    // Update is called once per frame
    void Update()
    {
        // each frame update the health bar fill of character
        slider.value = healthSystem.health;
    }
}
