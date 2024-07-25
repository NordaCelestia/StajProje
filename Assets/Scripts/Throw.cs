using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Throw : MonoBehaviour
{
    [SerializeField] GameObject throwPosition;
    [SerializeField] Animator CharacterAnimator;
    [SerializeField] GameObject Snowball;
    [SerializeField] GameObject AnimationControl;
    [SerializeField] Rigidbody SnowballRB;
    [SerializeField] Image UISnowballRenderer;
    [SerializeField] float snowballSpeed = 10;

    
    
    


    private void Start()
    {
        SnowballRB = Snowball.GetComponent<Rigidbody>();

        UIUpdate(0.4f);
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

            StartCoroutine(animationTransition());
            
            StartCoroutine(delayedThrow());
                 

        }
    }

    IEnumerator animationTransition()
    {
        yield return new WaitForSeconds(1.1f);

        UIUpdate(1f);

        CharacterAnimator.SetTrigger("isThrowing");
    }

    IEnumerator delayedThrow() //animasyonda kolun kalkmasý için bekliyor.
    {
        yield return new WaitForSeconds(1.4f);

        UIUpdate(0.4f);

        Snowball.SetActive(true);
        Snowball.transform.position = throwPosition.transform.position;

        Vector3 force = throwPosition.transform.forward * snowballSpeed;

        
        SnowballRB.AddForce(force, ForceMode.VelocityChange);
    }

    void UIUpdate(float alfa)
    {
        Color colorSnowball = UISnowballRenderer.color;
        colorSnowball.a = alfa;
        UISnowballRenderer.color = colorSnowball;
    }
}
