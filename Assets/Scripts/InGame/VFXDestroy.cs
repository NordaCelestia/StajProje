using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXDestroy : MonoBehaviour
{
    private float secondsToSuicide = 1f;

    void Start()
    {
        if (!this.gameObject.CompareTag("Parent"))
        {
            Destroy(gameObject, secondsToSuicide);
        }
            
    }

    
}
