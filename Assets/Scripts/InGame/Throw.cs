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
    }

    void Update()
    {
        FindTarget();
        if (isPlayer && targetTransform != null)
        {
            UpdateCamera();
        }
    }

    void FindTarget()
    {
        string enemyTag = (this.gameObject.CompareTag("Team1")) ? "Team2" : "Player";
        GameObject target = GameObject.FindWithTag(enemyTag);
        if (target != null)
        {
            targetTransform = target.transform;
            targetRigidbody = target.GetComponent<Rigidbody>();
        }
        else
        {
            targetTransform = null;
            targetRigidbody = null;
            Debug.LogWarning("Target with tag '" + enemyTag + "' not found.");
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
