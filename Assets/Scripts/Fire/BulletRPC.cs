using UnityEngine;

public class BulletRPC : MonoBehaviour
{
    public float speed = 20f;
    public float lifetime = 3f;

    private void Start()
    {
        Destroy(gameObject, lifetime); // se destruye después de un tiempo
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject);
    }
}
