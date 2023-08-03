using System.Collections;
using System.Collections.Generic;
using UnityEditor.Compilation;
using UnityEngine;
using UnityEngine.UI;

public class DashFill : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private PlayerMovement playerMovement;
    private Image sliderColor;
    private Color blue;
    private Color white;
    void Start()
    {
        slider.maxValue = playerMovement.dashCooldown;
        slider.value = slider.maxValue;
        sliderColor = GetComponent<Image>();
        white = new Color(1f, 1f, 1f, 1f);
        blue = new Color(0.27f, 0.92f, 0.93f, 1f);
    }

    void Update()
    {
        sliderColor.color = white;
        slider.value = (Time.time - playerMovement.lastDashTime);
        if (slider.value == slider.maxValue) sliderColor.color = blue;
    }
}
