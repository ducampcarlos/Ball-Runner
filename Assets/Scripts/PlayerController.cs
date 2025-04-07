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
    AudioSource audioSource;
    [SerializeField] float pitchUp = 2f;
    [SerializeField] float pitchNormal = 1f;
    [SerializeField] float pitchLerpSpeed = 2f;
    [SerializeField] ParticleSystem[] explosionParticles;
    bool isDead = false;
    [SerializeField] AudioClip explosionSFX;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(isDead)
        {
            return;
        }

        // Detects if the player is on Android platform
#if UNITY_ANDROID || UNITY_EDITOR
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame && canJump && !isDead)
        {
            Jump();
        }
#endif

        // Detects if the player is on PC platform
#if UNITY_STANDALONE || UNITY_EDITOR
        if (Mouse.current.leftButton.wasPressedThisFrame && canJump && !isDead)
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
            GameManager.instance.Invoke("Restart", 1.5f);
            shipObject.SetActive(false);
            isDead = true;
            rb.isKinematic = true;
            GetComponent<Collider>().enabled = false;
            audioSource.Stop();
            audioSource.loop = false;
            audioSource.pitch = 1f;
            audioSource.clip = explosionSFX;
            audioSource.time = 0.15f; // Empezar desde 0.15s
            audioSource.Play();
            for (int i = 0; i < explosionParticles.Length; i++)
            {
                explosionParticles[i].Play();
            }
        }
    }
}
