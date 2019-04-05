using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script to trigger the cannons in level 2. They are either triggered by player input, or by NPC's location.
public class Cannon : MonoBehaviour
{
    [SerializeField]
    private float projSpeed = 20;
    [SerializeField]
    private GameObject projectile;
    private GameObject barrel;

    private bool canShoot, shotFired = false;
    // Start is called before the first frame update

    void Start() {
        foreach (Transform child in gameObject.transform) {
            if (child.name == "Cube") {
                barrel = child.gameObject;
            }
        }
    }

    void Update() {
        if (canShoot) {
            if (Input.GetMouseButtonDown(0) && !shotFired) {
                Shoot();
                shotFired = true;
            }

            if (Input.GetMouseButtonUp(0)) {
                shotFired = false;
            }
        }
    }

    void Shoot() {
        GameObject projClone = Instantiate(projectile, barrel.transform.position, barrel.transform.rotation);
        projClone.GetComponent<Rigidbody>().velocity = transform.TransformDirection(new Vector3(0,0, projSpeed));
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player")
            canShoot = true;
        else if (other.gameObject.tag == "Defender") {
            Shoot();
            Debug.Log("Defenderwe");
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player")
            canShoot = false;
    }
}
