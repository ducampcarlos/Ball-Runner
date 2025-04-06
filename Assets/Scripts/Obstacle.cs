using UnityEngine;

public class Obstacle : MonoBehaviour
{
    float speed;

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    public void Activate(float newSpeed)
    {
        speed = newSpeed;
        gameObject.SetActive(true);
    }

    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }
}
