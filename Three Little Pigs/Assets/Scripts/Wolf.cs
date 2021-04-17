using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : MonoBehaviour
{
    public float maxHP;
    public float power;
    public float speed;
    public float attackCooldown;
    public Vector2 initialDirection;

    private float currHP;
    private Rigidbody2D rb;
    private Vector2 currDirection;

    // Start is called before the first frame update
    void Start()
    {
        currHP = maxHP;
        rb = GetComponent<Rigidbody2D>();
        currDirection = initialDirection;
        currDirection.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void TakeDamage(float damage)
    {
        currHP -= damage;
        if (currHP <= damage)
        {
            speed = 0;
            GameManager.S.OnEnemyDeath();
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
            speed = 0;
            StartCoroutine(AttackHut(collision.gameObject.GetComponent<Hut>()));
        }
    }

    private IEnumerator AttackHut(Hut hut)
    {
        while (hut != null)
        {
            hut.TakeDamage(power);
            yield return new WaitForSeconds(attackCooldown);
        }
    }
}
