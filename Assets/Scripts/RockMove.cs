using UnityEngine;

public class RockMove : MonoBehaviour
{
    public float speed = 3f;

    void Update()
{
    transform.Translate(Vector3.left * speed * Time.deltaTime);

    // Use local position relative to parent TrainingArea
    if (transform.localPosition.x < -15f)
    {
        Destroy(gameObject);
    }
}
}