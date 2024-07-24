using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw : MonoBehaviour
{
    [SerializeField] GameObject throwPosition;
    [SerializeField] GameObject Snowball;
    [SerializeField] GameObject AnimationControl;
    [SerializeField] Rigidbody SnowballRB;
    [SerializeField] float snowballSpeed = 10;

    
    AnimControl AnimControlVar;


    private void Start()
    {
        SnowballRB = Snowball.GetComponent<Rigidbody>();
        AnimControlVar = AnimationControl.GetComponent<AnimControl>();
    }

    void Update()
    {
        ThrowSnowball();


    }

    void ThrowSnowball()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

            if (AnimControlVar.AnimationControl())
            {
                StartCoroutine(delayedThrow());
            }
            else
            {

            }

        }
    }

    IEnumerator delayedThrow()
    {
        yield return new WaitForSeconds(0.3f);
        Snowball.SetActive(true);
        Snowball.transform.position = throwPosition.transform.position;

        Vector3 force = throwPosition.transform.forward * snowballSpeed;

        
        SnowballRB.AddForce(force, ForceMode.VelocityChange);
    }
}
