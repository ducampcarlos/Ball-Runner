using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

//<summary>
//This script is responsible for the player's movement, supporting both Android and PC platforms.
//</summary>
public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] bool canJump = true;
    [SerializeField] GameObject shipObject;
    [SerializeField] AudioSource audioSource;
    [SerializeField] float pitchUp = 2f;
    [SerializeField] float pitchNormal = 1f;
    [SerializeField] float pitchLerpSpeed = 2f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // Detects if the player is on Android platform
#if UNITY_ANDROID || UNITY_EDITOR
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame && canJump)
        {
            Jump();
        }
#endif

        // Detects if the player is on PC platform
#if UNITY_STANDALONE || UNITY_EDITOR
        if (Mouse.current.leftButton.isPressed && canJump)
        {
            Jump();
        }
#endif

        // Rotate ship when is going up to look up, and down when is going down
        Quaternion targetRotation = Quaternion.identity;

        float targetPitch = pitchNormal;

        if (Mathf.Approximately(rb.linearVelocity.y, 0))
        {
            targetRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (rb.linearVelocity.y > 0)
        {
            targetRotation = Quaternion.Euler(-30, 0, 0);
            targetPitch = pitchUp;
        }
        else if (rb.linearVelocity.y < 0)
        {
            targetRotation = Quaternion.Euler(30, 0, 0);
        }


        audioSource.pitch = Mathf.Lerp(audioSource.pitch, targetPitch, Time.deltaTime * pitchLerpSpeed);


        float rotationSpeed = 10f; // Podés ajustar esto para más o menos suavidad
        shipObject.transform.rotation = Quaternion.Lerp(
            shipObject.transform.rotation,
            targetRotation,
            Time.deltaTime * rotationSpeed
        );
    }

    private void FixedUpdate()
    {
        // Gravity applied by code
        rb.AddForce(Vector3.down * 40f, ForceMode.Force);
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        canJump = false;
    }

    // When the player collides with the ground, the player can jump again.
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            canJump = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
