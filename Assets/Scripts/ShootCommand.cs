using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootCommand : ICommand
{
    private GameObject bulletPrefab;
    private float bulletSpeed = 10f;
    private Transform soldierTransform;
    private int maxAmmo = 30;
    private int currentAmmo;
    private int totalAmmo;

    private AudioSource audioSource;
    private AudioClip shotSound;
    private AudioClip emptySound;
    private AudioClip reloadSound;

    public ShootCommand(GameObject bulletPrefab, Transform soldierTransform, int startingAmmo, AudioSource audioSource, AudioClip shotSound, AudioClip emptySound, AudioClip reloadSound)
    {
        this.bulletPrefab = bulletPrefab;
        this.soldierTransform = soldierTransform;
        this.currentAmmo = maxAmmo;
        this.totalAmmo = startingAmmo;
        this.audioSource = audioSource;
        this.shotSound = shotSound;
        this.emptySound = emptySound;
        this.reloadSound = reloadSound;
    }

    public void Execute()
    {
        if (currentAmmo > 0)
        {
            Vector3 shootPosition = soldierTransform.position + soldierTransform.forward + new Vector3(0, 2f, 0);
            GameObject bullet = Object.Instantiate(bulletPrefab, shootPosition, soldierTransform.rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.velocity = soldierTransform.forward * bulletSpeed;

            currentAmmo--;
            Debug.Log($"AYUDA: Balas restantes en el cargador: {currentAmmo}. Balas totales en el inventario: {totalAmmo}.");

            audioSource.PlayOneShot(shotSound);
        }
        else
        {
            Debug.Log("SOLDADO: -�Mierda! �Cargador vac�o, necesito recargar balas!");
            Debug.Log("AYUDA: Presiona la tecla R para recargar municiones.");

            audioSource.PlayOneShot(emptySound);
        }
    }

    public void Reload()
    {
        if (totalAmmo > 0)
        {
            int neededAmmo = maxAmmo - currentAmmo;
            int ammoToReload = Mathf.Min(neededAmmo, totalAmmo);

            currentAmmo += ammoToReload;
            totalAmmo -= ammoToReload;

            Debug.Log($"AYUDA: Cargador recargado. Balas en el cargador: {currentAmmo}. Balas restantes en el inventario: {totalAmmo}.");

            audioSource.PlayOneShot(reloadSound);
        }
        else
        {
            Debug.Log("SOLDADO: -�No puede ser! �Me he quedado sin municiones!");
            Debug.Log("AYUDA: Dir�gete a la caja de municiones m�s cercana para conseguir m�s balas.");
        }
    }

    public void AddAmmo(int amount)
    {
        totalAmmo += amount;
        Debug.Log($"AYUDA: Has recogido {amount} balas. Ahora tienes {totalAmmo} en el inventario.");
    }

    public int GetCurrentAmmo()
    {
        return currentAmmo;
    }

    public int GetTotalAmmo()
    {
        return totalAmmo;
    }
}
