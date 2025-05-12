using UnityEngine;
using Photon.Pun;

/// <summary>
/// Arma que dispara proyectiles sincronizados en Photon.
/// </summary>
public class ProjectileWeapon : Weapon
{
    [Header("Proyectil")]
    [Tooltip("Nombre del prefab de la bala (debe estar en Resources/)")]
    public string projectilePrefabName;
    [Tooltip("Transform que marca la punta del cañón")]
    public Transform firePoint;
    [Tooltip("Velocidad inicial de la bala")]
    public float projectileSpeed = 20f;
    [Tooltip("Segundos antes de destruir la bala")]
    public float projectileLifetime = 5f;

    /// <summary>
    /// Implementación del disparo: instancia la bala en red y la inicializa.
    /// </summary>
    public override void PerformShoot()
    {
        if (firePoint == null) return;

        // Calcula la dirección horizontal
        Vector3 dir = firePoint.forward;
        dir.y = 0;
        dir.Normalize();

        // Instancia el proyectil
        GameObject proj = PhotonNetwork.Instantiate(
            projectilePrefabName,
            firePoint.position,
            Quaternion.LookRotation(dir)    // Alinea la rotación con la dirección
        );

        // Dale velocidad
        var rb = proj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Opcional: desactivar gravedad si quieres trayectoria recta
            rb.useGravity = false;

            rb.linearVelocity = dir * projectileSpeed;
        }

        // Resto (Initialize, si lo usas)…
    }

}
