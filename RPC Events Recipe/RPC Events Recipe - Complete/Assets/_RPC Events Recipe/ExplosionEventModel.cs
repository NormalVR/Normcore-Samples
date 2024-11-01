using UnityEngine;
using Normal.Realtime;

[RealtimeModel]
public partial class ExplosionEventModel {
    [RealtimeProperty(1, true)] private int     _trigger;
    [RealtimeProperty(2, true)] private int     _senderID;
    [RealtimeProperty(3, true)] private Vector3 _position;
    [RealtimeProperty(4, true)] private float   _scale;

    // Used to fire an event on all clients
    public void FireEvent(int senderID, Vector3 position, float scale) {
        this.trigger++;
        this.senderID = senderID;
        this.position = position;
        this.scale    = scale;
    }

    // An event that consumers of this model can subscribe to in order to respond to the event
    public delegate void EventHandler(int senderID, Vector3 position, float scale);
    public event EventHandler eventDidFire;

    // A RealtimeCallback method that fires whenever we read any values from the server
    [RealtimeCallback(RealtimeModelEvent.OnDidReadModel)]
    private void DidRead() {
        if (eventDidFire != null && trigger != 0)
            eventDidFire(senderID, position, scale);
    }
}
