using UnityEngine;

public class RockMove : MonoBehaviour
{
    public float speed = 3f;

    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        // Destroy when far off-screen left
        if (transform.position.x < -15f)
        {
            Destroy(gameObject);
        }
    }
}