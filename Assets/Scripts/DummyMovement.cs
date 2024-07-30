using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyMovement : MonoBehaviour
{
    private Rigidbody rigidBodyDummy;

    int randomVar;
    float speed;

    private void Start()
    {
        rigidBodyDummy = GetComponent<Rigidbody>();
        StartCoroutine(dummyMove());
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

            Vector3 force = new Vector3(speed, 0f, 0f) * 5;
            float applyForceDuration = 2.0f; // Kuvveti uygulama süresi (saniye)
            float startTime = Time.time;

            while (Time.time < startTime + applyForceDuration)
            {
                rigidBodyDummy.AddForce(force);
                yield return null; // Bir sonraki frame'i bekleyin
            }

            Vector3 velocity = rigidBodyDummy.velocity;
            velocity.y = 0f;

            if (velocity.magnitude > 15)
            {
                rigidBodyDummy.velocity = velocity.normalized * 15;
            }

            // Hareket süresini uzatmak için bekleme süresini artýrýn
            yield return new WaitForSeconds(3.0f);
        }
    }
}
