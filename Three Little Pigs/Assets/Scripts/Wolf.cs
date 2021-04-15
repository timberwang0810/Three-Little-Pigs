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
            Destroy(this.gameObject, 1.0f);
        }
    }

    private void FixedUpdate()
    {
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
        Debug.Log("Wolf Won");
    }
}
