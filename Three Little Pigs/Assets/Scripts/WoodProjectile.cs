using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodProjectile : Projectile
{
    public int maxPierce;
    private int numPierce = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);

            numPierce++;

            if (numPierce >= maxPierce) Destroy(this.gameObject);
        }
    }
}
