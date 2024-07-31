using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float extraForceMultiplier = 3f;
    [SerializeField] float maxSpeed = 10f;
    [SerializeField] Transform cameraTransform;
    [SerializeField] Animator CharacterAnimator;
    [SerializeField] GameObject AnimationControl;
    

    private float movementSpeed;
    private Rigidbody rb;
    private Vector3 moveDirection;
    private Quaternion initialRotation;
    AnimControl AnimControlVar;

    void Start()
    {
        AnimControlVar = AnimationControl.GetComponent<AnimControl>();
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked; 
        initialRotation = transform.rotation;
        
    }

    void Update()
    {
        Move();
        CharacterAnim();
        
    }

    void Move()    //haraket
    {
        float moveHorizontal = Input.GetAxis("Horizontal"); // Sa� ve sola hareket giri�i

        Vector3 force = new Vector3(moveHorizontal, 0f, 0f) * moveSpeed;
        Vector3 velocity = rb.velocity;

        // Yava�lamay� ve kaymay� �nlemek i�in ters y�nde ekstra kuvvet ekle
        if ((moveHorizontal > 0 && velocity.x < 0) || (moveHorizontal < 0 && velocity.x > 0) || (velocity.x == 0))
        {
            force *= extraForceMultiplier;
        }

        rb.AddForce(force);

        // Maksimum h�z� s�n�rla
        velocity = rb.velocity;
        velocity.y = 0f; // Y eksenindeki h�zlar� s�f�rla

        if (velocity.magnitude > maxSpeed)
        {
            rb.velocity = velocity.normalized * maxSpeed; // H�z� s�n�rla
        }
    }

    void CharacterAnim()   //Karakterin h�z�n� sa�a ve sola g�re negatif veya pozitif olarak hesapalay�p bunu animat�re vermek
    {

        float MovementVariable = rb.velocity.x;
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

    


}
