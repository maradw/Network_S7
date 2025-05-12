
using Photon.Pun;

using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class PlayerController : MonoBehaviourPun
{
    [Tooltip("Arma equipada (ProjectileWeapon)")]
    public Weapon equippedWeapon;

    void Update()
    {
        if (!photonView.IsMine || equippedWeapon == null)
            return;

        if (Input.GetButtonDown("Fire1"))
        {
            bool didShoot = equippedWeapon.TryShoot();
            if (didShoot)
                photonView.RPC(nameof(RPC_HandleShoot), RpcTarget.Others);
        }
    }

    [PunRPC]
    void RPC_HandleShoot()
    {
        equippedWeapon.PerformShoot();
    }
}