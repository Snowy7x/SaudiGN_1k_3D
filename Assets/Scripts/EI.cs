using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EI : Actor
{
    [Header("Options")]
    [SerializeField] float petrolSpeed;
    [SerializeField] float chaseSpeed;
    [SerializeField] float attackRange;
    [SerializeField] float stopRange;
    [SerializeField] float chaseRange;
    [SerializeField] float loseRange;

    [Header("Attack")] 
    [SerializeField] Transform shootPoint;
    [SerializeField] private ParticleSystem shootPs;
    [SerializeField] private GameObject hitImpact;
    [SerializeField] private float errorRange = 10f;
    [SerializeField] private float lookSpeed = 10f;
    [SerializeField] float damage;
    [SerializeField] [Range(0.01f, 1f)]private float fireRate = 0.2f;
    private float _lastFire = 0f;
    private bool _isClose = false;
    
    [Header("Petrol")]
    [SerializeField] private Transform[] petrolPoints;
    [SerializeField] float petrolMinDistance = 3f;
    private Transform _currentPetrol;
    private bool _petrolReached;
    
    [Header("General")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator anim;
    [SerializeField] private AudioSource audioSourceShoot;
    [SerializeField] private GameObject dieEffect;

    private Transform target;

    private void Awake()
    {
        if (!anim) anim = GetComponent<Animator>();
        switch (PlayerPrefs.GetInt("Mode", 0))
        {
            case 0:
                damage = 5f;
                break;
            case 1:
                damage = 15f;
                break;
            case 2:
                damage = 50f;
                break;
        }
    }

    private void Start()
    {
        target = Player.instance.transform;
        agent.speed = petrolSpeed;
        
    }

    public enum State
    {
        Petrol,
        Chase,
        Attack,
        Idle
    }

    public State myState;
    private static readonly int Petrol1 = Animator.StringToHash("Petrol");
    private static readonly int Chase1 = Animator.StringToHash("Chase");

    private void Update()
    {
        switch (myState)
        {
            case State.Attack:
                Attack();
                break;
            case State.Chase:
                Chase();
                break;
            case State.Idle:
                Idle();
                break;
            case State.Petrol:
                Petrol();
                break;
        }
        CheckForTarget();
        UpdateAnimations();
    }
    
    void UpdateAnimations()
    {
        // TODO: Update the animations    
        switch (myState)
        {
            case State.Petrol:
                anim.SetBool(Petrol1, true);
                anim.SetBool(Chase1, false);
                break;
            
            case State.Attack:
                anim.SetBool(Petrol1, false);
                anim.SetBool(Chase1, !_isClose);

                break;
            case State.Idle:
                anim.SetBool(Petrol1, false);
                anim.SetBool(Chase1, false);
                break;
            case State.Chase:
                anim.SetBool(Petrol1, false);
                anim.SetBool(Chase1, true);
                break;
        }
    }
    
    void Attack()
    {
        // TODO: Check if can see the player
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance <= stopRange)
        {
            agent.isStopped = true;
            _isClose = true;
            agent.speed = 0;
            agent.destination = transform.position;
        }
        else Chase();

        LookAtTarget();
        if (Time.time < _lastFire + fireRate) return;
        _lastFire = Time.time + fireRate;
        // Do the actual attacking
        Vector3 shootDir = target.position - shootPoint.position + Random.insideUnitSphere * errorRange;
        RaycastHit hit;
        if(shootPs) shootPs.Play();
        SoundManager.Instance.Play("Shoot", audioSourceShoot);
        if (Physics.Raycast(shootPoint.position, shootDir, out hit, 100f))
        {
            Actor actor = hit.transform.GetComponent<Actor>();
            if (actor) actor.TakeDamage(damage);
            if (hitImpact) Instantiate(hitImpact, hit.point, Quaternion.LookRotation(hit.normal));
        }
    }

    void Petrol()
    {
        if (petrolPoints.Length <= 1)
        {
            myState = State.Idle;
            return;
        }

        agent.speed = petrolSpeed;

        // Choose random point if there is no
        if (!_currentPetrol)
            _currentPetrol = petrolPoints[Random.Range(0, petrolPoints.Length)];
        var position = _currentPetrol.position;
        agent.destination = position;
        // Check the distance for the point
        float distance = Vector3.Distance(transform.position, position);
        // if close enough to the point choose another one;
        if (distance <= petrolMinDistance)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            Transform lastPoint = _currentPetrol;
            while (_currentPetrol == lastPoint)
            {
                _currentPetrol = petrolPoints[Random.Range(0, petrolPoints.Length)];
                agent.destination = _currentPetrol.position;
            }
        }
        else
        {
            if (agent.isStopped) agent.isStopped = false;
            // Go to the the point...
            // Should do auto...
        }
    }

    void Idle()
    {
        agent.isStopped = true;
        agent.destination = transform.position;
    }

    void Chase()
    {
        _isClose = false;
        if (agent.isStopped) agent.isStopped = false;
        if (agent.speed < chaseSpeed) agent.speed = chaseSpeed;
        agent.destination = target.position;
    }

    void CheckForTarget()
    {
        // TODO: Check for the player
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance >= loseRange)
        {
            myState = State.Petrol;
        }else if (distance <= chaseRange && distance > attackRange)
        {
            myState = State.Chase;
        }else if (distance <= attackRange)
        {
            myState = State.Attack;
        }
        else
        {
            Debug.LogError("Should not reach this...");
            myState = State.Idle;
        }
    }

    void CanSee()
    {
        // TODO: CHECK IF CAN SEE THE PLAYER
    }

    void LookAtTarget()
    {
        Vector3 relativePos = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos);

        Quaternion current = transform.localRotation;

        transform.localRotation = Quaternion.Slerp(current, rotation, Time.deltaTime                              
                                                                      * lookSpeed);
    }

    public override void Die()
    {
        Instantiate(dieEffect, transform.position, transform.rotation);
        Destroy(gameObject);
        base.Die();
    }
}
