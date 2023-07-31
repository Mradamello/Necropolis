using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LowHPScreenDistortion : MonoBehaviour
{
    public Volume volume;
    private ChromaticAberration chromaticAberration;
    private HealthSystem healthSystem;
    //[Range(0f,1f)]
    private float value;
    [SerializeField] private float healthThreshhold;
    private void Start()
    {
        volume.profile.TryGet(out chromaticAberration);
        healthSystem = GetComponent<HealthSystem>();
    }
    private void Update()
    {
        if(chromaticAberration != null && healthSystem.health <= healthThreshhold)
        {
            value = (float) (1 - (healthSystem.health / healthThreshhold));
            chromaticAberration.active = true;
            chromaticAberration.intensity.Override(Mathf.Clamp(value, 0f, 1f));
        }else
        {
            chromaticAberration.active = false;
        }
    }
}
