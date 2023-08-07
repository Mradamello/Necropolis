using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaSystem : MonoBehaviour
{
    public float currentStamina;
    [SerializeField] private float maxStamina;
    [SerializeField] private float staminaRegen;
    public Slider slider;
    private void Start()
    {
        // set current stamina to max stamina
        currentStamina = maxStamina;
        // set max stamina as max fill of stamina bar
        slider.maxValue = currentStamina;
        // set slider current value as current stamina amount
        slider.value = currentStamina;
    }
    void Update()
    {
        // each frame regen staminaRegen amount of stamina, and update the stamina bar fill amount
        if(currentStamina < maxStamina)
        {
            currentStamina += staminaRegen;
            if(currentStamina > maxStamina)
            {
                currentStamina = maxStamina;
            }
        }
        slider.value = currentStamina;
    }
}
