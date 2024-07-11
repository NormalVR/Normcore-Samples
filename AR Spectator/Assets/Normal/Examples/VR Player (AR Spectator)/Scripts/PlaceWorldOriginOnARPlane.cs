using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace Normal.Realtime.Examples {
    public class PlaceWorldOriginOnARPlane : MonoBehaviour {
        [SerializeField]
        private GameObject _instructions = default;
        
        private ARRaycastManager _raycastManager;
        private List<ARRaycastHit> _raycastHits = new List<ARRaycastHit>();
        
        private void Awake() {
            _raycastManager = GetComponent<ARRaycastManager>();
        }
        
        private void Update() {
            if (Input.touchCount <= 0)
                return;
                
            Touch touch = Input.GetTouch(0);
            
            // Raycast out to see if we hit an AR Plane
            if (_raycastManager.Raycast(touch.position, _raycastHits, TrackableType.PlaneWithinPolygon)) {
                // Raycast hits are sorted by distance, so the first one will be the closest hit.
                Pose    hitPoint = _raycastHits[0].pose;
                
                // Transform to local space
                // NOTE: ARFoundation doesn't raycast hits to world space correctly, so we have to use the same broken math to undo their transformation.
                Vector3 hitPointLocalPosition = hitPoint.position - transform.position;
                
                // Move the session origin
                transform.position = -hitPointLocalPosition;
                
                // Disable instructions
                _instructions.SetActive(false);

                // Disable planes
                ARPlaneManager planeManager = GetComponent<ARPlaneManager>();
                foreach (var plane in planeManager.trackables) {
                    plane.gameObject.SetActive(false);
                }
                planeManager.enabled = false;
                
                // Disable script
                enabled = false;
            }
        }
    }
}
