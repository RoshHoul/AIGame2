using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//This class is used for creating the projectile, affecting the individuals which it managed to hit and
// a small particle effect system
public class CannonBall : MonoBehaviour
{
    public float power = 40.0f;
    public float radius = 5.0f;
    public float upforce = 1.0f;

    public ParticleSystem explosion;

    bool explode = false;

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
                foreach (Rigidbody rb in hit.GetComponentsInChildren<Rigidbody>()) {
                   rb.isKinematic = false;
                   rb.AddExplosionForce(power, explostionPosition, radius, upforce, ForceMode.Impulse);
                }

                foreach (Collider col in hit.GetComponentsInChildren<Collider>()) {
                    col.enabled = true;
                }
                hit.GetComponent<Unit>().Die();
                ParticleSystem expl = Instantiate(explosion, transform.position, transform.rotation);
                float totalDuration = expl.duration + expl.startLifetime;
                expl.Play();
                Destroy(expl, totalDuration);
                //Destroy(expl);
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.name == "Terrain" || other.gameObject.tag == "Unit") {
            Debug.Log("PLUSHTIM ZEMQTA");
            explode = true;
        }
    }

    
}
