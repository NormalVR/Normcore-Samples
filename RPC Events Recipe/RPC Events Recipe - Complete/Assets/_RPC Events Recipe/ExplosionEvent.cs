using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class ExplosionEvent : RealtimeComponent<ExplosionEventModel> {
    [SerializeField]
    private ExplosionParticleSystem _explosionParticleSystem = default;

    // When we connect to a room server, we'll be given an instance of our model to work with.
    protected override void OnRealtimeModelReplaced(ExplosionEventModel previousModel, ExplosionEventModel currentModel) {
        if (previousModel != null) {
            // Unsubscribe from events on the old model.
            previousModel.eventDidFire -= EventDidFire;
        }
        if (currentModel != null) {
            // Subscribe to events on the new model
            currentModel.eventDidFire += EventDidFire;
        }
    }

    // A public method we can use to fire the event
    public void Emit(Vector3 position, float scale) {
        model.FireEvent(realtime.clientID, transform.position, scale);
    }

    // Called whenever our event fires
    private void EventDidFire(int senderID, Vector3 position, float scale) {
        // Tell the particle system to trigger an explosion in response to the event
        _explosionParticleSystem.Emit(position, scale);
    }
}
