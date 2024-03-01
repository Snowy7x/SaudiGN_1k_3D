using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public enum Mode
{
    Easy,
    Medium,
    Hard
}

public class Player : Actor
{
    [SerializeField] Movement movement;
    [SerializeField] Transform cam;
    [SerializeField] Transform camHolder;
    [SerializeField] GameObject hands;
    public static Player instance;
    public bool is2D = false;
    private Vector3 _orgPos;
    private Vector3 _orgRot;

    public TMP_Text healthText;

    private void Awake()
    {
        instance = this;
        is2D = false;
    }

    private void Update()
    {
        UpdateUI();
        if (transform.position.y <= -50)
        {
            TakeDamage(health + 10f);
        }
    }

    public void MiniGame_Mode(Transform target)
    {
        _orgPos = cam.position;
        _orgRot = cam.rotation.eulerAngles;
        cam.DOMove(target.position, 1f);
        cam.DORotate(target.rotation.eulerAngles, 1f);
        movement.canMove = false;
        hands.SetActive(false);
        is2D = true;
    }

    public void FPS_Mode()
    {
        StartCoroutine(GoBack());
        is2D = false;
    }

    IEnumerator GoBack()
    {
        cam.DOLocalMove(new Vector3(0, 0, 0), 1.2f);
        cam.DOLocalRotate(new Vector3(0,0, 0), 1f);
        hands.SetActive(true);
        yield return new WaitForSeconds(1.2f);
        movement.canMove = true;
    }

    public override void TakeDamage(float damage)
    {
        HUD.instance.GotHit();
        base.TakeDamage(damage);
    }

    public override void Die()
    {
        HUD.instance.DeathPanel();
        base.Die();
    }

    void UpdateUI()
    {
        healthText.text = health.ToString();
    }
}