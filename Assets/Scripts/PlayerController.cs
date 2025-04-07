using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
        if (isDead) return;

#if UNITY_ANDROID || UNITY_WEBGL || UNITY_EDITOR
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame && canJump)
        {
            Jump();
        }
#endif

#if UNITY_STANDALONE || UNITY_WEBGL || UNITY_EDITOR
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame && canJump)
        {
            Jump();
        }
#endif

        // Visual feedback for up/down movement
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
        else
        {
            targetRotation = Quaternion.Euler(30, 0, 0);
        }

        audioSource.pitch = Mathf.Lerp(audioSource.pitch, targetPitch, Time.deltaTime * pitchLerpSpeed);
        float rotationSpeed = 10f;
        shipObject.transform.rotation = Quaternion.Lerp(shipObject.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    private void FixedUpdate()
    {
        rb.AddForce(Vector3.down * 40f, ForceMode.Force);
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        canJump = false;
    }

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
            audioSource.time = 0.15f;
            audioSource.Play();

            foreach (var particle in explosionParticles)
                particle.Play();
        }
    }
}
