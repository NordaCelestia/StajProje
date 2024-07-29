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
    [SerializeField] float snowballSpeed = 30;

    bool firstRun;
    private Transform targetTransform;

    private void Start()
    {
        firstRun = true;
        SnowballRB = Snowball.GetComponent<Rigidbody>();
        UIUpdate(1f);
        ThrowSnowball();
    }

    void Update()
    {
        FindTarget();
        
    }

    void FindTarget()
    {
        GameObject target = GameObject.FindWithTag("Team2");
        if (target != null)
        {
            targetTransform = target.transform;
        }
        else
        {
            targetTransform = null;
            Debug.LogWarning("Target with tag 'Team2' not found.");
        }
    }

    void ThrowSnowball()
    {
            StartCoroutine(animationTransition());
            
    }

    IEnumerator animationTransition() // Animasyonlar arasý geçiþ
    {

        if (firstRun)
        {
            firstRun = false;
            yield return new WaitForSeconds(1f);
            CharacterAnimator.SetTrigger("GrabSnowball");
            yield return new WaitForSeconds(1.1f);
            UIUpdate(1f);
            CharacterAnimator.SetTrigger("isThrowing");
            StartCoroutine(delayedThrow());
        }
        else
        {
            CharacterAnimator.SetTrigger("GrabSnowball");
            yield return new WaitForSeconds(1.1f);
            UIUpdate(1f);
            CharacterAnimator.SetTrigger("isThrowing");
            StartCoroutine(delayedThrow());
        }

    }

    IEnumerator delayedThrow() // Animasyonda kolun kalkmasý için bekliyor
    {
        yield return new WaitForSeconds(0.3f);

        UIUpdate(0.4f);

        Snowball.SetActive(true);
        Snowball.transform.position = throwPosition.transform.position;

        SnowballRB.velocity = Vector3.zero;
        SnowballRB.angularVelocity = Vector3.zero;

        if (targetTransform != null)
        {
            Vector3 targetCenter = targetTransform.position;
            // Hedefin y-ekseni yüksekliðini ortalayarak atýþý düzeltin
            targetCenter.y = throwPosition.transform.position.y;
            Vector3 throwDirection = (targetCenter - throwPosition.transform.position).normalized;
            Vector3 force = throwDirection * snowballSpeed;
            SnowballRB.AddForce(force, ForceMode.VelocityChange);
        }
        else
        {
            Debug.LogWarning("Target transform is not set. Snowball will not be thrown.");
        }

        yield return new WaitForSeconds(2f);
        UIUpdate(1f);

        StartCoroutine(animationTransition());
    }

    void UIUpdate(float alpha)
    {
        Color colorSnowball = UISnowballRenderer.color;
        colorSnowball.a = alpha;
        UISnowballRenderer.color = colorSnowball;
    }
}
