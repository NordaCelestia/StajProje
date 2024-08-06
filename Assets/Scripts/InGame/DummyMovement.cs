using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyMovement : MonoBehaviour
{
    [SerializeField] Animator CharacterAnimator;

    private Rigidbody rigidBodyDummy;

    private float speed = 10f;
    private float direction = 1f;
    private float changeDirectionInterval;
    private float nextDirectionChangeTime = 0f;

    private void Start()
    {
        rigidBodyDummy = GetComponent<Rigidbody>();
        StartCoroutine(dummyMove());
        SetRandomDirectionAndInterval();
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
            Vector3 force = new Vector3(direction * speed, 0f, 0f);
            rigidBodyDummy.AddForce(force, ForceMode.VelocityChange);

            yield return new WaitForSeconds(changeDirectionInterval);
        }
    }

    private void CheckForDirectionChange()
    {
        if (Time.time >= nextDirectionChangeTime)
        {
            direction = Random.Range(0, 2) == 0 ? -1f : 1f;
            rigidBodyDummy.velocity = Vector3.zero;

            SetRandomDirectionAndInterval();

            nextDirectionChangeTime = Time.time + changeDirectionInterval;
        }
    }

    private void SetRandomDirectionAndInterval()
    {
        changeDirectionInterval = Random.Range(1f, 3f); // 1 ile 3 saniye arasýnda rastgele bir süre
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
