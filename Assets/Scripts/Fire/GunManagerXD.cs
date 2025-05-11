using Photon.Pun;
using UnityEngine;

using Photon.Realtime;
public class GunManagerXD : MonoBehaviour
{
    [Header("Bullet")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    private PhotonView photonView;

    private void OnEnable()
    {
      //  PlayerMovement.OnShoot += Shoot;
    }

    private void Awake()
    {
       // photonView = GetComponent<PhotonView>();

    }
    private void Start()
    {
        photonView = GetComponentInParent<PhotonView>();
        if (photonView == null)
        {
            Debug.LogError("GunManager: No se encontró un componente PhotonView en este GameObject.");
            return;
        }
        if (photonView.IsMine)
            PlayerMovement1.OnShoot += Shoot;
    }

    private void OnDisable()
    {
        PlayerMovement1.OnShoot -= Shoot;
    }

    void Shoot()
    {
        if (photonView.IsMine)
        {
            photonView.RPC("RPC_Shoot", RpcTarget.All, firePoint.position, firePoint.rotation);
        }
    }

    [PunRPC]
    void RPC_Shoot(Vector3 position, Quaternion rotation)
    {
        Instantiate(bulletPrefab, position, rotation);
    }
}
