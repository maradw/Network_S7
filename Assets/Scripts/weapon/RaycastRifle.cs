using UnityEngine;
/// <summary>
/// Ejemplo de arma concreta: rifle de raycast simple.
/// </summary>
public class RaycastRifle : Weapon
{
    public Transform firePoint;
    public float range = 100f;
    public LayerMask hitLayers;
    public GameObject muzzleFlashPrefab;
    //public AudioClip shotSound;
    private int damage=10;

    public override void PerformShoot()
    {
        // Efecto muzzle
        if (muzzleFlashPrefab)
        {
            var fx = Instantiate(muzzleFlashPrefab, firePoint.position, firePoint.rotation);
            Destroy(fx, 1f);
        }
        // Sonido
        //if (shotSound)
           // AudioSource.PlayClipAtPoint(shotSound, firePoint.position);
        // Raycast
        if (Physics.Raycast(firePoint.position, firePoint.forward, out RaycastHit hit, range, hitLayers))
        {
            // 1) Intentamos coger el componente PlayerHealth
            var ph = hit.collider.GetComponentInParent<PlayerHealth>();
            if (ph != null)
            {
                // 2) Solo el cliente que dispara (owner) manda el RPC
                //    el propio ChangeHealth hará photonView.IsMine check en el otro extremo
                ph.ChangeHealth(-damage);
            }
        }
    }
}
