using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretRange : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy") {

            bool living = collision.gameObject.GetComponent<CapsuleCollider2D>().enabled;

            if (transform.parent.GetComponent<Turret>().enemyInRange == null && living) {
                transform.parent.GetComponent<Turret>().enemyInRange = collision.gameObject;
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy") transform.parent.GetComponent<Turret>().enemyInRange = null;
    }
}
