using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyMovement : MonoBehaviour
{
    [SerializeField] Animator CharacterAnimator;

    private Rigidbody rigidBodyDummy;

    private float speed = 10f;
    private float direction = 1f;
    private float changeDirectionInterval = 1f;
    private float nextDirectionChangeTime = 0f;
    private float maxSpeed = 10f; // Maksimum hýz

    private void Start()
    {
        rigidBodyDummy = GetComponent<Rigidbody>();
        StartCoroutine(dummyMove());
    }

    private void Update()
    {
        CharacterAnim();
        CheckForDirectionChange();
    }

    void CharacterAnim()
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
            if (rigidBodyDummy.velocity.magnitude < maxSpeed)
            {
                Vector3 force = new Vector3(direction * speed, 0f, 0f);
                rigidBodyDummy.AddForce(force, ForceMode.VelocityChange);
            }

            yield return new WaitForSeconds(changeDirectionInterval);
        }
    }

    private void CheckForDirectionChange()
    {
        if (Time.time >= nextDirectionChangeTime)
        {
            direction = Random.Range(0, 2) == 0 ? -1f : 1f;

            nextDirectionChangeTime = Time.time + changeDirectionInterval;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            direction = -direction;
            rigidBodyDummy.velocity = Vector3.zero;
        }
    }
}
