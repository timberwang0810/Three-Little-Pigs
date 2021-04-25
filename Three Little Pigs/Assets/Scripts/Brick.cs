using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : Projectile
{
    public float splashDamage;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("call from child");
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);

            GetComponent<CircleCollider2D>().enabled = true;
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;

            Destroy(this.gameObject, 0.5f);
        }
    }
}
