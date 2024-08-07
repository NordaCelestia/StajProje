using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Throw : MonoBehaviour
{
    [SerializeField] GameObject throwPosition, Snowball, AnimationControl, AudioManager;
    [SerializeField] Animator CharacterAnimator;
    [SerializeField] CinemachineVirtualCamera cinemachineCamera; // Kamera ile ilgili kodlarý devre dýþý býrakma
    float snowballSpeed;
    [SerializeField] Rigidbody SnowballRB;
    [SerializeField] Vector3 cameraOffset = new Vector3(0, 5, -10); // Kamera ile ilgili kodlarý devre dýþý býrakma
    float leadFactor;

    CharacterDataScriptableObject characterData;
    bool firstRun;
    byte throwSfxRandom, throwCooldownRandom;
    SFX sfxManager;
    private Transform targetTransform;
    private Rigidbody targetRigidbody;
    private bool isPlayer;
    private string enemyTag;
    private Transform lastTargetTransform;

    private void Start()
    {
        
        firstRun = true;
        sfxManager = AudioManager.GetComponent<SFX>();
        SnowballRB = Snowball.GetComponent<Rigidbody>();
        ThrowSnowball();

        isPlayer = gameObject.CompareTag("Player");
        DetermineEnemyTag();

        
        if (isPlayer)
        {
            snowballSpeed = 70;
            leadFactor = 0.5f;
        }
        else
        {
            snowballSpeed = characterData.snowballSpeed;
            leadFactor = characterData.leadFactor;
        }
    }

    void Update()
    {
        FindTarget();
    }

    void DetermineEnemyTag()
    {
        if (gameObject.CompareTag("Player"))
        {
            enemyTag = "Team2";
            Debug.Log("Player is looking for targets with tag 'Team2'.");
        }
        else if (gameObject.CompareTag("Team2"))
        {
            enemyTag = "Player";
            Debug.Log("Team2 is looking for targets with tag 'Player'.");
        }
        else
        {
            Debug.LogWarning("GameObject does not have a valid tag for targeting.");
        }
    }

    void FindTarget()
    {
        if (string.IsNullOrEmpty(enemyTag))
        {
            Debug.LogWarning("Enemy tag is not set. Cannot find target.");
            return;
        }

        GameObject[] targets = GameObject.FindGameObjectsWithTag(enemyTag);
        Debug.Log("Found " + targets.Length + " targets with tag '" + enemyTag + "'.");

        float closestDistance = Mathf.Infinity;
        Transform closestTarget = null;

        foreach (GameObject potentialTarget in targets)
        {
            if (potentialTarget != this.gameObject && (isPlayer ? !potentialTarget.CompareTag("Player") : true))
            {
                float distanceToTarget = (potentialTarget.transform.position - transform.position).sqrMagnitude;
                if (distanceToTarget < closestDistance)
                {
                    closestDistance = distanceToTarget;
                    closestTarget = potentialTarget.transform;
                    targetRigidbody = potentialTarget.GetComponent<Rigidbody>();
                }
            }
        }

        if (closestTarget != lastTargetTransform)
        {
            StartCoroutine(ChangeTargetWithDelay(closestTarget));
        }
    }

    IEnumerator ChangeTargetWithDelay(Transform newTarget)
    {
        yield return new WaitForSeconds(0.5f);

        // Kamera ile ilgili kodlarý devre dýþý býrakma
        if (isPlayer)
        {
            if (!newTarget.CompareTag("Player"))
            {
                lastTargetTransform = newTarget;
                targetTransform = lastTargetTransform;
                // cinemachineCamera.LookAt = targetTransform;
                Debug.Log("Target acquired: " + (targetTransform != null ? targetTransform.gameObject.name : "none"));
            }
        }
        else
        {
            lastTargetTransform = newTarget;
            targetTransform = lastTargetTransform;
            // cinemachineCamera.LookAt = targetTransform;
            Debug.Log("Target acquired: " + (targetTransform != null ? targetTransform.gameObject.name : "none"));
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
            CharacterAnimator.SetTrigger("isThrowing");
            StartCoroutine(delayedThrow());
        }
        else
        {
            CharacterAnimator.SetTrigger("GrabSnowball");
            yield return new WaitForSeconds(1.1f);
            CharacterAnimator.SetTrigger("isThrowing");
            StartCoroutine(delayedThrow());
        }
    }

    IEnumerator delayedThrow() // Animasyonda kolun kalkmasý için bekliyor
    {
        yield return new WaitForSeconds(0.3f);
        throwSfxRandom = (byte)Random.Range(0, 3);

        sfxManager.PlaySound(throwSfxRandom);

        GameObject snowballInstance = Instantiate(Snowball, throwPosition.transform.position, Quaternion.identity);
        Rigidbody snowballRb = snowballInstance.GetComponent<Rigidbody>();

        snowballRb.velocity = Vector3.zero;
        snowballRb.angularVelocity = Vector3.zero;

        if (targetTransform != null)
        {
            Vector3 targetCenter = targetTransform.position;
            targetCenter.y = throwPosition.transform.position.y;

            if (targetRigidbody != null)
            {
                Vector3 predictedPosition = targetCenter + targetRigidbody.velocity * leadFactor;
                targetCenter = predictedPosition;
            }

            Vector3 throwDirection = (targetCenter - throwPosition.transform.position).normalized;
            Vector3 force = throwDirection * snowballSpeed;
            snowballRb.AddForce(force, ForceMode.VelocityChange);
        }
        else
        {
            Debug.LogWarning("Target transform is not set. Snowball will not be thrown.");
        }
        throwCooldownRandom = (byte)Random.Range(1.7f, 2f);
        yield return new WaitForSeconds(throwCooldownRandom);

        StartCoroutine(animationTransition());
    }
}
