using UnityEngine;
using Normal.Realtime.Serialization;

[RealtimeModel]
public partial class BrushStrokeModel {
    [RealtimeProperty(1, true)]
    private RealtimeArray<RibbonPointModel> _ribbonPoints;

    [RealtimeProperty(2, false)]
    private Vector3 _brushTipPosition;

    [RealtimeProperty(3, false)]
    private Quaternion _brushTipRotation;

    [RealtimeProperty(4, true)]
    private bool _brushStrokeFinalized;
}
