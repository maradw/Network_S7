using System.Collections;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [Header("Munición y Cadencia")]
    [Tooltip("Balas máximas en el cargador")]
    public int maxAmmo = 30;
    [Tooltip("Tiempo mínimo entre disparos")]
    public float fireRate = 0.2f;

    protected int currentAmmo;
    protected float lastFireTime;

    protected virtual void Awake()
    {
        currentAmmo = maxAmmo;
    }

    /// <summary>
    /// Lógica concreta de disparo (raycast, proyectil, efectos, etc).  
    /// Debe implementarlo cada arma derivada.
    /// </summary>
    public abstract void PerformShoot();

    /// <summary>
    /// Comprueba cadencia y munición, descuenta bala e invoca PerformShoot().
    /// </summary>
    public bool TryShoot()
    {
        if (Time.time - lastFireTime < fireRate || currentAmmo <= 0)
            return false;

        lastFireTime = Time.time;
        currentAmmo--;
        PerformShoot();
        return true;
    }

    /// <summary>
    /// Recarga una cantidad de munición al cargador (por ejemplo, tras recoger cajas).
    /// </summary>
    public void Reload(int ammo)
    {
        currentAmmo = Mathf.Clamp(currentAmmo + ammo, 0, maxAmmo);
    }
}
