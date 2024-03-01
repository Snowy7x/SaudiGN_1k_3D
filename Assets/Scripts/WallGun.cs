using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public class ShootPoint
{
    public Transform shootPoint;
    public ParticleSystem ps;
}

public class WallGun : Actor
{
    public float shootRange = 100f;
    private bool _canShoot = false;
    public Player player;
    public ShootPoint[] shootPoints;
    public Transform msPoint;
    public GameObject dieParticle;
    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        player = Player.instance;
    }

    void Update()
    {
        if (_canShoot)
        {
            transform.LookAt(player.transform);
            foreach (ShootPoint shootPoint in shootPoints)
            {
                Shoot(shootPoint);
            }
        }
    }

    private void Shoot(ShootPoint shootPoint)
    {
        RaycastHit hit;
        if (!shootPoint.ps.isPlaying) shootPoint.ps.Play();
        // TODO: GUN SOUND...
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _canShoot = true;
            SoundManager.Instance.Play("MiniGun", audioSource, true);
            /*RaycastHit hit;
            Ray ray = new Ray(msPoint.position, msPoint.forward);
            if (Physics.Raycast(ray, out hit, shootRange + shootRange * 0.5f))
                if (!hit.transform.CompareTag("Player")) _canShoot = false;*/
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _canShoot = false;
            SoundManager.Instance.StopLoop("MiniGun", "MiniGunEnd");
            foreach (ShootPoint shootPoint in shootPoints)
            {
                shootPoint.ps.Stop();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _canShoot = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shootRange);
    }

    public override void Die()
    {
        if (dieParticle) Instantiate(dieParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
        base.Die();
    }
}