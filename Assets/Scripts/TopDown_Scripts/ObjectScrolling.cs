using UnityEngine;

public class ObjectScrolling : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;

        if (transform.position.y <= -10f)
            transform.position = new Vector3(transform.position.x, Random.Range(9f,10f), transform.position.z);
    }
}
