using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXDestroy : MonoBehaviour
{
    private float secondsToSuicide = 1.5f;

    void Start()
    {
        if (!this.gameObject.CompareTag("Parent"))
        {
            Destroy(gameObject, secondsToSuicide);
        }
            
    }

    
}
