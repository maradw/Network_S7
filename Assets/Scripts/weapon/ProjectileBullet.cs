using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileBullet : MonoBehaviourPun
{
    private Rigidbody rb;
    private float lifeTimer;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Configura velocidad inicial y tiempo de vida.
    /// </summary>
    public void Initialize(Vector3 velocity, float lifetime)
    {
        rb.linearVelocity = velocity;
        lifeTimer = lifetime;
        if (photonView.IsMine)
            Invoke(nameof(DestroySelf), lifeTimer);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Aquí puedes añadir efectos de impacto
        DestroySelf();
    }

    void DestroySelf()
    {
        if (photonView.IsMine)
            PhotonNetwork.Destroy(gameObject);
    }
}
