﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float maxHP;
    public int money;
    public float power;
    public float speed;
    public float attackCooldown;
    public EnemyType enemyType;
    public Vector2 initialDirection;

    private float currHP;
    private bool isDead = false;
    private HealthBar healthBar;
    private Rigidbody2D rb;
    private Vector2 currDirection;

    private Animator animator;
    private SpriteRenderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        currHP = maxHP;
        rb = GetComponent<Rigidbody2D>();
        currDirection = initialDirection;
        currDirection.Normalize();
        
        renderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        animator.SetBool("walking", true);
        animator.SetBool("bike", true);

        if (currDirection.x == -1)
        {
            renderer.flipX = true;
        }
        healthBar = GetComponentInChildren<HealthBar>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;
        currHP -= damage;

        if (enemyType == EnemyType.FOX && transform.childCount == 2 && currHP <= (maxHP / 2))
        {
            animator.SetBool("bike", false);
            speed /= 1.5f;
            GameObject bike = transform.GetChild(1).gameObject;
            bike.transform.parent = null;
            bike.SetActive(true);
            Destroy(bike, 1.0f);
        }
 
        healthBar.AdjustFillAmount(currHP, maxHP);
        if (currHP <= 0)
        {
            isDead = true;
            speed = 0;
            healthBar.AdjustFillAmount(0, maxHP);
            animator.SetTrigger("die");
            SoundManager.S.MakeDeathSound(enemyType);
            GameManager.S.OnEnemyDeath(money);
            UIManager.S.ShowMoneyFlashText(money, transform.position);
            Destroy(this.gameObject, 1.0f);
        }
    }

    public void Turn(Direction dir, float leftAngle, float rightAngle)
    {
        switch (dir)
        {
            case Direction.LEFT:
                RotateByDegrees(leftAngle);
                break;
            case Direction.RIGHT:
                RotateByDegrees(-rightAngle);
                break;
            case Direction.EITHER:
                float r = Random.Range(0, 1.0f);
                if (r < 0.5f) RotateByDegrees(leftAngle);
                else RotateByDegrees(-rightAngle);
                break;
            default:
                return;
        }
    }

    private void RotateByDegrees(float degree)
    {
        Vector2 newDir = currDirection;
        newDir.x = currDirection.x * Mathf.Cos(Mathf.Deg2Rad * degree) - currDirection.y * Mathf.Sin(Mathf.Deg2Rad * degree);
        newDir.y = currDirection.x * Mathf.Sin(Mathf.Deg2Rad * degree) + currDirection.y * Mathf.Cos(Mathf.Deg2Rad * degree);
        currDirection = newDir;

        if (currDirection.x == -1)
        {
            renderer.flipX = true;
        }

        if (currDirection.x == 1)
        {
            renderer.flipX = false;
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.S.gameState != GameManager.GameState.playing) return;
        rb.velocity = currDirection * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Hut"))
        {
            animator.SetBool("walking", false);
            speed = 0;
            StartCoroutine(AttackHut(collision.gameObject.GetComponent<Hut>()));
        }

        if (collision.gameObject.CompareTag("SplashDamage")) //splash damage
        {
            TakeDamage(collision.gameObject.GetComponent<BrickProjectile>().splashDamage);
        }
    }

    private IEnumerator AttackHut(Hut hut)
    {
        while (hut != null)
        {
            animator.SetTrigger("attack");
            SoundManager.S.MakeAttackSound(enemyType, animator.GetBool("bike"));
            hut.TakeDamage(power);
            yield return new WaitForSeconds(attackCooldown);
        }
    }
}
