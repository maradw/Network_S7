using Photon.Pun;
using UnityEngine;

public class PlayerHealth : MonoBehaviourPunCallbacks
{
    public int maxHealth = 100;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    [PunRPC]
    public void TakeDamage(int damage)
    {
        if (!photonView.IsMine) return;

        currentHealth -= damage;
        Debug.Log("Me dieron! Vida restante: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Jugador muerto");
        // Aquí podrías hacer respawn, desactivar al jugador, etc.
    }
}
