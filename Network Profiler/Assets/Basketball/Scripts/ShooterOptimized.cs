using Normal.Realtime;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class ShooterOptimized : MonoBehaviour {
    private void Update() {
        // If the mouse was clicked, spawn 10 basketballs.
        if (Mouse.current.leftButton.wasPressedThisFrame) {
            ShootTenBasketballs();
        }
    }

    // Shoot 10 basketballs
    private void ShootTenBasketballs() {
        for (var i = 0; i < 10; i++) {
            Shoot();
        }
    }

    // Shoot a single basketball
    private void Shoot() {
        // Aim in a random direction
        var position = transform.position + transform.right * Random.Range(-4.0f, 4.0f);

        // Spawn a basketball prefab
        var basketball = InstantiateBasketballFromPool(position, transform.rotation);
        if (basketball == null) return; // Not connected yet

        // Set the velocity to a random forward/up velocity
        basketball.GetComponent<RealtimeTransform>().RequestOwnership();
        basketball.GetComponent<Rigidbody>().linearVelocity = transform.forward * 5.0f;
    }

    // Create a queue to reuse basketballs
    private Queue<GameObject> _basketballPool = new Queue<GameObject>();

    private GameObject InstantiateBasketballFromPool(Vector3 position, Quaternion rotation) {
        GameObject basketball;

        // Once we hit 50 basketballs, reuse the oldest one.
        if (_basketballPool.Count >= 50) {
            // Grab the oldest basketball
            basketball = _basketballPool.Dequeue();

            // Reset the basketball's position and rotation
            basketball.GetComponent<Rigidbody>().position = position;
            basketball.GetComponent<Rigidbody>().rotation = rotation;
        } else {
            // Instantiate a fresh basketball
            var instantiateOptions = new Realtime.InstantiateOptions() {
                ownedByClient = false,
            };
            basketball = Realtime.Instantiate("Basketball", position, rotation, instantiateOptions);
        }

        // Put the basketball at the end of the queue and return it
        _basketballPool.Enqueue(basketball);
        return basketball;
    }
}
