using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CannonBall : MonoBehaviour
{
    public float power = 40.0f;
    public float radius = 5.0f;
    public float upforce = 1.0f;

    bool explode = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (explode) {
            Detonate();
        }
    }

    void Detonate() {
        Vector3 explostionPosition = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explostionPosition, radius);
        foreach (Collider hit in colliders) {
            if (hit.GetComponent<Unit>() != null) {
                Debug.Log("E TUKA GURMIM");
                hit.GetComponent<NavMeshAgent>().enabled = false;
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                rb.isKinematic = false;
                rb.AddExplosionForce(power, explostionPosition, radius, upforce, ForceMode.Impulse);
                Destroy(gameObject);
            }
        }
    }

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.name == "Terrain" || other.gameObject.tag == "Unit") {
            Debug.Log("PLUSHTIM ZEMQTA");
            explode = true;
        }
    }
}
