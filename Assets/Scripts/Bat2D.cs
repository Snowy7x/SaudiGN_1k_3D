using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class Bat2D : Actor
{
    public Transform target;

    public float speed;
    public float nextWayPointDistance = 3f;

    private Path _path;
    private int _currentWaypoint = 0;
    private bool _reached_end_of_bath = false;

    private Seeker _seeker;
    private Rigidbody2D _rb;
    private bool isFollowing;

    public GameObject dieParticles;
    [SerializeField] float cooldown = 0.2f;

    private void Start()
    {
        _seeker = GetComponent<Seeker>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && !isFollowing)
        {
            isFollowing = true;
            target = col.transform;
            InvokeRepeating("UpdatePath", 0f, .5f);
        }
    }

    void UpdatePath()
    {
        try
        {
            if (_seeker.IsDone()) _seeker.StartPath(_rb.position, target.position, OnPathComplete);
        }catch {}
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            _path = p;
            _currentWaypoint = 0;
        }
    }

    private void FixedUpdate()
    {
        if (!isFollowing || !Player.instance.is2D || _path == null) return;
        if (_currentWaypoint >= _path.vectorPath.Count)
        {
            _reached_end_of_bath = true;
            return;
        }
        _reached_end_of_bath = false;
        Vector2 dir = ((Vector2)_path.vectorPath[_currentWaypoint] - _rb.position).normalized;
        Vector2 force = dir * speed * Time.deltaTime;
        
        _rb.AddForce(force);

        float dis = Vector2.Distance(_rb.position, _path.vectorPath[_currentWaypoint]);
        if (dis < nextWayPointDistance)
        {
            _currentWaypoint++;
        }
    }

    public override void Die()
    {
        if (dieParticles)
            Instantiate(dieParticles, transform.position, transform.rotation);
        MiniGame.instance.AddKill();
        Destroy(this.gameObject);
        base.Die();
    }

    public void StartCooldown()
    {
        StartCoroutine(Cooldown_en());
    }

    IEnumerator Cooldown_en()
    {
        CancelInvoke("UpdatePath");
        _seeker.StartPath(_rb.position, new Vector3(_rb.position.x, _rb.position.y + 10f), OnPathComplete);
        yield return new WaitForSeconds(cooldown);
        InvokeRepeating("UpdatePath", 0f, .5f);
    }
}
