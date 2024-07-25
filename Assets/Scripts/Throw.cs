using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw : MonoBehaviour
{
    [SerializeField] GameObject throwPosition;
    [SerializeField] Animator CharacterAnimator;
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
            CharacterAnimator.SetTrigger("GrabSnowball");

            while (true)
            {
                if (AnimControlVar.AnimationControl() == true)
                {
                    CharacterAnimator.SetTrigger("isThrowing");
                    StartCoroutine(delayedThrow());
                    break;
                }
                else
                {
                    continue;
                }
            }

        }
    }

    IEnumerator delayedThrow() //animasyonda kolun kalkmas� i�in bekliyor.
    {
        yield return new WaitForSeconds(0.3f);
        Snowball.SetActive(true);
        Snowball.transform.position = throwPosition.transform.position;

        Vector3 force = throwPosition.transform.forward * snowballSpeed;

        
        SnowballRB.AddForce(force, ForceMode.VelocityChange);
    }
}
