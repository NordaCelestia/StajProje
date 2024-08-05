using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyMovement : MonoBehaviour
{
    [SerializeField] Animator CharacterAnimator;

    private Rigidbody rigidBodyDummy;

    int randomVar;
    float speed;

    private void Start()
    {
        rigidBodyDummy = GetComponent<Rigidbody>();
        StartCoroutine(dummyMove());
    }

    private void Update()
    {
        CharacterAnim();
    }

    void CharacterAnim()   //Karakterin hýzýný saða ve sola göre negatif veya pozitif olarak hesapalayýp bunu animatöre vermek
    {

        float MovementVariable = rigidBodyDummy.velocity.x;
        if (MovementVariable == 0)
        {
            CharacterAnimator.SetBool("isMoving", false);
        }
        else
        {
            CharacterAnimator.SetBool("isMoving", true);
        }



        CharacterAnimator.SetFloat("MovementVariable", MovementVariable);
    }

    IEnumerator dummyMove()
    {
        while (true)
        {
            randomVar = Random.Range(0, 6);
            if (randomVar % 2 == 0)
            {
                speed = 1;
            }
            else
            {
                speed = -1;
            }

            Vector3 force = new Vector3(speed, 0f, 0f) * 2;
            float applyForceDuration = 1.4f; 
            float startTime = Time.time;

            while (Time.time < startTime + applyForceDuration)
            {
                rigidBodyDummy.AddForce(force);
                yield return null; 
            }

            Vector3 velocity = rigidBodyDummy.velocity;
            velocity.y = 0f;

            if (velocity.magnitude > 10)
            {
                rigidBodyDummy.velocity = velocity.normalized * 10;
            }

            
            yield return new WaitForSeconds(1f);
        }
    }
}
