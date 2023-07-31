using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyScript : MonoBehaviour
{
     private void OnAnimationFinish()
    {
        Destroy(gameObject);
    }
}
