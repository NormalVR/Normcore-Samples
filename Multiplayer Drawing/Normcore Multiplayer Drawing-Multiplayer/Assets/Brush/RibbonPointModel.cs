using UnityEngine;

[RealtimeModel]
public partial class RibbonPointModel {
    [RealtimeProperty(1, true)]
    private Vector3    _position;

    [RealtimeProperty(2, true)]
    private Quaternion _rotation = Quaternion.identity;
}
