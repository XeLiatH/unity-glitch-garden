﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    [Range(0f, 2f)] [SerializeField] float currentSpeed = 1f;

    Animator animator;
    Defender currentTarget;

    void Awake()
    {
        FindObjectOfType<LevelController>().AttackerSpawned();
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        transform.Translate(Vector2.left * currentSpeed * Time.deltaTime);
        UpdateAnimationState();
    }

    void OnDestroy()
    {
        var levelController = FindObjectOfType<LevelController>();
        if (levelController != null)
        {
            levelController.AttackerKilled();
        }
    }

    private void UpdateAnimationState()
    {
        if (!currentTarget)
        {
            animator.SetBool("IsAttacking", false);
        }
    }

    public void SetMovementSpeed(float speed)
    {
        currentSpeed = speed;
    }

    public void Attack(Defender target)
    {
        animator.SetBool("IsAttacking", true);
        currentTarget = target;
    }

    public void StrikeCurrentTarget(float damage)
    {
        if (!currentTarget)
        {
            return;
        }

        Health health = currentTarget.GetComponent<Health>();

        if (health)
        {
            // increase damage by 10 % per difficulty
            damage += Mathf.Round((float)(damage * 0.1 * PlayerPrefsController.GetDifficulty()));
            health.DealDamage(damage);
        }
    }
}
