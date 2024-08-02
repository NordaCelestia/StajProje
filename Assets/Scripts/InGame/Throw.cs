using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Throw : MonoBehaviour
{
    [SerializeField] GameObject throwPosition, Snowball, AnimationControl, AudioManager;
    [SerializeField] Animator CharacterAnimator;
    [SerializeField] float snowballSpeed = 45;
    [SerializeField] Rigidbody SnowballRB;
    [SerializeField] Camera mainCamera;
    [SerializeField] float cameraSmoothSpeed = 0.7f;
    [SerializeField] Vector3 cameraOffset = new Vector3(0, 2, -10);
    [SerializeField] float initialXRotation = 30f;
    [SerializeField] float leadFactor = 0.5f;

    bool firstRun;
    byte throwSfxRandom;
    SFX sfxManager;
    private Transform targetTransform;
    private Rigidbody targetRigidbody;
    private bool isPlayer;
    private string enemyTag;

    private void Start()
    {
        firstRun = true;
        sfxManager = AudioManager.GetComponent<SFX>();
        SnowballRB = Snowball.GetComponent<Rigidbody>();
        ThrowSnowball();

        // Kameranýn baþlangýç x rotasyonunu ayarla
        Vector3 eulerRotation = mainCamera.transform.rotation.eulerAngles;
        eulerRotation.x = initialXRotation;
        mainCamera.transform.rotation = Quaternion.Euler(eulerRotation);

        // Karakterin oyuncu olup olmadýðýný kontrol et
        isPlayer = gameObject.CompareTag("Player");

        // Enemy tag belirle
        DetermineEnemyTag();
    }

    void Update()
    {
        FindTarget();
        if (isPlayer && targetTransform != null)
        {
            UpdateCamera();
        }
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

        foreach (GameObject potentialTarget in targets)
        {
            if (potentialTarget != this.gameObject) // Kendini hedef olarak seçme
            {
                float distanceToTarget = (potentialTarget.transform.position - transform.position).sqrMagnitude;
                if (distanceToTarget < closestDistance)
                {
                    closestDistance = distanceToTarget;
                    targetTransform = potentialTarget.transform;
                    targetRigidbody = potentialTarget.GetComponent<Rigidbody>();
                }
            }
        }

        if (targetTransform == null)
        {
            Debug.LogWarning("Target with tag '" + enemyTag + "' not found.");
        }
        else
        {
            Debug.Log("Target acquired: " + targetTransform.gameObject.name);
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

        yield return new WaitForSeconds(2f);

        StartCoroutine(animationTransition());
    }

    void UpdateCamera()
    {
        if (targetTransform != null && mainCamera != null)
        {
            Vector3 targetPosition = targetTransform.position + cameraOffset;
            Vector3 direction = targetPosition - mainCamera.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            Vector3 eulerRotation = targetRotation.eulerAngles;
            eulerRotation.x = mainCamera.transform.rotation.eulerAngles.x;
            targetRotation = Quaternion.Euler(eulerRotation);

            mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, targetRotation, Time.deltaTime * cameraSmoothSpeed);
        }
    }
}
