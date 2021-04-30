using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickProjectile : Projectile
{
    public float splashDamage;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);

            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;

            GetComponent<CircleCollider2D>().enabled = true;
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<Animator>().SetTrigger("explode");

            Destroy(this.gameObject, 0.5f);
        }
    }
}
