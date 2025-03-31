using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //<summary>
    //This script is responsible for the player's movement for the Android platform.
    //</summary>

    Rigidbody rb;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] bool canJump =true;

    private void Awake()
    {
        rb=GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && canJump)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            canJump = false;
        }

        //Gravity applied by code
        rb.AddForce(Vector3.down * 20f, ForceMode.Force);
    }

    //When the player collides with the ground, the player can jump again.
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
