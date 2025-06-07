using Normal.Realtime;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class ShooterUnoptimized : MonoBehaviour {
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

        // Set ownedByClient to false so other clients can take over ownership on collision
        var instantiateOptions = new Realtime.InstantiateOptions() {
            ownedByClient = false,
        };

        // Spawn a basketball prefab
        var basketball = Realtime.Instantiate("Basketball", position, transform.rotation, instantiateOptions);
        if (basketball == null) return; // Not connected yet

        // Set the velocity to a random forward/up velocity
        basketball.GetComponent<RealtimeTransform>().RequestOwnership();
        basketball.GetComponent<Rigidbody>().linearVelocity = transform.forward * 5.0f;
    }
}
