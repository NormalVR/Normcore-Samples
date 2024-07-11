using UnityEngine;
using Normal.Realtime;
using Normal.Realtime.Serialization;

public class BrushStroke : RealtimeComponent<BrushStrokeModel> {
    [SerializeField]
    private BrushStrokeMesh _mesh = null;

    // Smoothing
    private Vector3    _ribbonEndPosition;
    private Quaternion _ribbonEndRotation = Quaternion.identity;

    // Mesh
    private Vector3    _previousRibbonPointPosition;
    private Quaternion _previousRibbonPointRotation = Quaternion.identity;

    // Unity Events
    private void Update() {
        // Animate the end of the ribbon towards the brush tip
        AnimateLastRibbonPointTowardsBrushTipPosition();

        // Add a ribbon segment if the end of the ribbon has moved far enough
        AddRibbonPointIfNeeded();
    }

    // Interface
    public void BeginBrushStrokeWithBrushTipPoint(Vector3 position, Quaternion rotation) {
        // Update the model
        model.brushTipPosition = position;
        model.brushTipRotation = rotation;

        // Update last ribbon point to match brush tip position & rotation
        _ribbonEndPosition = position;
        _ribbonEndRotation = rotation;
        _mesh.UpdateLastRibbonPoint(_ribbonEndPosition, _ribbonEndRotation);
    }

    public void MoveBrushTipToPoint(Vector3 position, Quaternion rotation) {
        model.brushTipPosition = position;
        model.brushTipRotation = rotation;
    }

    public void EndBrushStrokeWithBrushTipPoint(Vector3 position, Quaternion rotation) {
        // Add a final ribbon point and mark the stroke as finalized
        AddRibbonPoint(position, rotation);
        model.brushStrokeFinalized = true;
    }

    // Ribbon drawing
    private void AddRibbonPointIfNeeded() {
        // Only add ribbon points if this brush stroke is being drawn by the local client.
        if (!realtimeView.isOwnedLocallySelf)
            return;

        // If the brush stroke is finalized, stop trying to add points to it.
        if (model.brushStrokeFinalized)
            return;

        if (Vector3.Distance(_ribbonEndPosition, _previousRibbonPointPosition) >= 0.01f ||
            Quaternion.Angle(_ribbonEndRotation, _previousRibbonPointRotation) >= 10.0f) {

            // Add ribbon point model to ribbon points array. This will fire the RibbonPointAdded event to update the mesh.
            AddRibbonPoint(_ribbonEndPosition, _ribbonEndRotation);

            // Store the ribbon point position & rotation for the next time we do this calculation
            _previousRibbonPointPosition = _ribbonEndPosition;
            _previousRibbonPointRotation = _ribbonEndRotation;
        }
    }

    private void AddRibbonPoint(Vector3 position, Quaternion rotation) {
        // Create the ribbon point
        RibbonPointModel ribbonPoint = new RibbonPointModel();
        ribbonPoint.position = position;
        ribbonPoint.rotation = rotation;
        model.ribbonPoints.Add(ribbonPoint);
    }

    private void RibbonPointAdded(RealtimeArray<RibbonPointModel> ribbonPoints, RibbonPointModel ribbonPoint, bool remote) {
        // Add ribbon point to the mesh
        _mesh.InsertRibbonPoint(ribbonPoint.position, ribbonPoint.rotation);
    }

    // Brush tip + smoothing
    private void AnimateLastRibbonPointTowardsBrushTipPosition() {
        // If the brush stroke is finalized, skip the brush tip mesh, and stop animating the brush tip.
        if (model.brushStrokeFinalized) {
            _mesh.skipLastRibbonPoint = true;
            return;
        }

        Vector3    brushTipPosition = model.brushTipPosition;
        Quaternion brushTipRotation = model.brushTipRotation;

        // If the end of the ribbon has reached the brush tip position, we can bail early.
        if (Vector3.Distance(_ribbonEndPosition, brushTipPosition) <= 0.0001f &&
            Quaternion.Angle(_ribbonEndRotation, brushTipRotation) <= 0.01f) {
            return;
        }

        // Move the end of the ribbon towards the brush tip position
        _ribbonEndPosition =     Vector3.Lerp(_ribbonEndPosition, brushTipPosition, 25.0f * Time.deltaTime);
        _ribbonEndRotation = Quaternion.Slerp(_ribbonEndRotation, brushTipRotation, 25.0f * Time.deltaTime);

        // Update the end of the ribbon mesh
        _mesh.UpdateLastRibbonPoint(_ribbonEndPosition, _ribbonEndRotation);
    }

    protected override void OnRealtimeModelReplaced(BrushStrokeModel previousModel, BrushStrokeModel currentModel) {
        // Clear Mesh
        _mesh.ClearRibbon();

        if (previousModel != null) {
            // Unregister from events
            previousModel.ribbonPoints.modelAdded -= RibbonPointAdded;
        }

        if (currentModel != null) {
            // Replace ribbon mesh
            foreach (RibbonPointModel ribbonPoint in currentModel.ribbonPoints)
                _mesh.InsertRibbonPoint(ribbonPoint.position, ribbonPoint.rotation);

            // Update last ribbon point to match brush tip position & rotation
            _ribbonEndPosition = model.brushTipPosition;
            _ribbonEndRotation = model.brushTipRotation;
            _mesh.UpdateLastRibbonPoint(model.brushTipPosition, model.brushTipRotation);

            // Turn off the last ribbon point if this brush stroke is finalized
            _mesh.skipLastRibbonPoint = model.brushStrokeFinalized;

            // Let us know when a new ribbon point is added to the mesh
            currentModel.ribbonPoints.modelAdded += RibbonPointAdded;
        }
    }
}
