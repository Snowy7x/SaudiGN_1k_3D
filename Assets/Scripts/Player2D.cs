using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Player2D : Actor
{
    public static Player2D instance;
    public float knockback = 10f;
    [Header("Dash Things")]
    private bool canDash = true;
    private bool isDashing;
    [SerializeField] float dashingPower = 24f;
    [SerializeField] float dashingTime = 0.2f;
    [SerializeField] float dashingCooldown = 1f;
    [SerializeField] private TrailRenderer tr;
    [Header("Movement things")]
    public float runSpeed = 0.6f;
    public float jumpForce = 2.6f;

    public Sprite jumpSprite;

    private Rigidbody2D body;
    private SpriteRenderer sr;
    private Animator animator;

    private bool isGrounded;
    public GameObject groundCheckPoint;
    public float groundCheckRadius;
    public LayerMask groundLayer;

    private bool jumpPressed;
    private bool APressed;
    private bool DPressed;
    private static readonly int Move = Animator.StringToHash("Move");

    private Vector3 _scale;
    public GameObject dieParticles;
    public TMP_Text healthTxt;


    private void Awake()
    {
        instance = this;
        body = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        _scale = transform.localScale;
        
    }

    public void SetHealth(TMP_Text txt)
    {
        health = Player.instance.health;
        healthTxt = txt;
    }

   
    private void Update()
    {
        if (!Player.instance.is2D || isDashing || !isAlive)  return;
        if (InputManager.Instance.jump) jumpPressed = true;
        if (InputManager.Instance.GetMoveDir().x < 0) APressed = true;
        if (InputManager.Instance.GetMoveDir().x > 0) DPressed = true;
        if (InputManager.Instance.fire && canDash)
        {
            StartCoroutine(Dash());
        }
        UpdateAnimation();

        if (transform.position.y <= -50)
        {
            TakeDamage(health + 10f);
        }

        healthTxt.text = health.ToString();
    }

    private void UpdateAnimation()
    {
        animator.SetBool(Move, InputManager.Instance.GetMoveDir().x != 0);
    }

   
    private void FixedUpdate()
    {
        if (!Player.instance.is2D || isDashing || !isAlive) return;
        isGrounded =
            Physics2D.OverlapCircle(groundCheckPoint.transform.position, groundCheckRadius,
                groundLayer);

       
        if (APressed)
        {
            body.velocity = new Vector2(-runSpeed, body.velocity.y);
            transform.localScale = new Vector3(-_scale.x,  _scale.y, _scale.z);
            APressed = false;
        }
        else if (DPressed)
        {
            body.velocity = new Vector2(runSpeed, body.velocity.y);
            transform.localScale = new Vector3(_scale.x,  _scale.y, _scale.z);

            DPressed = false;
        }
        else
        {
            body.velocity = new Vector2(0, body.velocity.y);
        }

       
        if (jumpPressed && isGrounded)
        {
            body.velocity = new Vector2(0, jumpForce);
            jumpPressed = false;
            InputManager.Instance.ResetJump();
        }

       
        if (!isGrounded)
        {
            animator.enabled = false;
            sr.sprite = jumpSprite;
            InputManager.Instance.ResetJump();
            jumpPressed = false;
        }
        else
        {
            animator.enabled = true;
        }
    }
    
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = body.gravityScale;
        body.gravityScale = 0f;
        body.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        body.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!isAlive) return;
        Actor actor = col.GetComponent<Actor>();
        if (actor && col.isTrigger == false)
        {
            if (isDashing)
                actor.TakeDamage(actor.maxHealth);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!isAlive) return;
        Actor actor = other.GetComponent<Actor>();
        if (actor && other.isTrigger == false)
        {
            if (isDashing)
            {
                actor.TakeDamage(actor.maxHealth);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!isAlive) return;
        Bat2D bat = col.gameObject.GetComponent<Bat2D>();
        if (bat)
        {
            bat.StartCooldown();
            TakeDamage(10f);
        }
    }

    public override void TakeDamage(float damage)
    {
        try
        {
            StopCoroutine(Damage());
        }catch{}
        StartCoroutine(Damage());
        body.AddForce((-transform.forward + transform.up) * knockback);
        base.TakeDamage(damage);
    }

    public override void Die()
    {
        Instantiate(dieParticles, transform.position, transform.rotation);
        //gameObject.SetActive(false);
        sr.enabled = false;
        StartCoroutine(After_Death());
        base.Die();
    }

    IEnumerator After_Death()
    {
        MiniGame.instance.Pre_Done();
        //TODO: DO SOMETHING SOUND AND SO ON...
        yield return new WaitForSeconds(2f);
        MiniGame.instance.Done();
    }

    IEnumerator Damage()
    {
        sr.color = new Color(1, 130 / 255, 145 / 255, 1);
        yield return new WaitForSeconds(0.05f);
        sr.color = Color.white;
        yield return new WaitForSeconds(0.05f);
        sr.color = new Color(1, 130 / 255, 145 / 255, 1);
        yield return new WaitForSeconds(0.05f);
        sr.color = Color.white;
        yield return new WaitForSeconds(0.05f);
        sr.color = new Color(1, 130 / 255, 145 / 255, 1);
        yield return new WaitForSeconds(0.05f);
        sr.color = Color.white;
    }
}