using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private bool building = true;
    private SpriteRenderer renderer;

    public float shootInterval;
    private float shootTimer = 0f;

    public GameObject projectileObject;

    public GameObject enemyInRange;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        renderer.color = Color.red;
        shootTimer = shootInterval;
    }

    // Update is called once per frame
    void Update()
    {
        if (building) return;

        shootTimer += Time.deltaTime;

        if (shootTimer >= shootInterval)
        {
            if (enemyInRange != null)
            {
                Shoot();
                shootTimer = 0f;
            }
        }
    }

    private void Shoot()
    {
        Vector3 dir = enemyInRange.transform.position - transform.position;
        GameObject projectile = Instantiate(projectileObject, transform.position, Quaternion.identity);
        projectile.GetComponent<Rigidbody2D>().velocity = dir * 7;
        Destroy(projectile, 5f);
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlaceableArea" && building) renderer.color = Color.green;

        if (collision.gameObject.tag == "Turret" && building) renderer.color = Color.red;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Turret" && building) renderer.color = Color.green;

        if (collision.gameObject.tag == "PlaceableArea" && building) renderer.color = Color.red;
    }

    // return true on success
    public bool BuildTurret()
    {
        if (renderer.color == Color.green)
        {
            building = false;
            renderer.color = Color.white;
            return true;
        }
        return false;
    }
}
