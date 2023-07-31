using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private Transform Wizard;
    // Update is called once per frame
    private void Update()
    {
        transform.position = new Vector3(Wizard.position.x, Wizard.position.y, transform.position.z);
    } 
}