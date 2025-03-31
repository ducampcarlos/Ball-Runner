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

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Detects if the player is on Android platform
#if UNITY_ANDROID
        if (Touchscreen.current.primaryTouch.press.wasPressedThisFrame && canJump)
        {
            Jump();
        }
#endif

        // Detects if the player is on PC platform
#if UNITY_STANDALONE
        if (Mouse.current.leftButton.isPressed && canJump)
        {
            Jump();
        }
#endif

        // Gravity applied by code
        rb.AddForce(Vector3.down * 20f, ForceMode.Force);
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
