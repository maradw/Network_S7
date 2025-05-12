using Photon.Pun;
using UnityEngine;

public class BulletRPC : MonoBehaviour
{
    public float speed = 20f;
    public float lifetime = 3f;
    public int damage = 20;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision other)
    {
        // Si golpeamos un jugador
        PlayerHealth health = other.collider.GetComponentInParent<PlayerHealth>();
        if (health != null)
        {
            PhotonView pv = health.GetComponent<PhotonView>();
            if (pv != null)
            {
                // Solo el cliente que posee ese jugador puede reducir su vida
                pv.RPC("TakeDamage", pv.Owner, damage);
            }
        }

        Destroy(gameObject);
    }
}
