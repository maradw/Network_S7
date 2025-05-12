
using Photon.Pun;

using UnityEngine;

/// <summary>
/// Manejo de vida de jugador con Photon.
/// </summary>
[RequireComponent(typeof(PhotonView))]
public class PlayerHealth : MonoBehaviourPun
{
    public float maxHealth = 100f;
    [HideInInspector] public float currentHealth;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Cambia la salud (puede ser positivo o negativo).
    /// </summary>
    public void ChangeHealth(float delta)
    {
        if (!photonView.IsMine) return;
        photonView.RPC(nameof(RPC_ChangeHealth), RpcTarget.All, delta);
    }

    [PunRPC]
    void RPC_ChangeHealth(float delta)
    {
        currentHealth = Mathf.Clamp(currentHealth + delta, 0f, maxHealth);
        // Actualiza UI aquí si es necesario
    }
}
