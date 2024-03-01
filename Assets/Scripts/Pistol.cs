
    using System;
    using UnityEngine;

    public class Pistol : ItemObject
    {
        public Gun gun;
        private float nextFire;
        public Animator anim;
        [SerializeField] ParticleSystem ps;
        [SerializeField] private GameObject impact;
        [SerializeField] private AudioSource audioSource;

        private void Start()
        {
            gun = (Gun)item;
        }

        public override void Use()
        {
            if (Time.time >= nextFire)
            {
                // Shooting...
                nextFire = Time.time + gun.fireRate;
                // Muzzle flash.
                // Animation.
                if (Camera.main != null)
                {
                    // TODO: PISTOL SOUND...
                    SoundManager.Instance.Play("Shoot", audioSource);
                    ps.Play();
                    if (anim) anim.Play("Shoot");
                    var ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, gun.fireRange))
                    {
                        Debug.Log(hit.collider.gameObject.name);
                        Actor actor = hit.transform.GetComponent<Actor>();
                        if (actor) actor.TakeDamage(gun.damage);
                        if (impact)
                            Instantiate(impact, hit.point, Quaternion.LookRotation(hit.normal));
                    }
                }
            }
        }
    }