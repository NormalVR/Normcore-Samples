using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace Normal.Realtime.Utility {
    /// <summary>
    /// Sets the <see cref="TrackingOriginModeFlags"/> on the device.
    /// </summary>
    public class XRTrackingOriginMode : MonoBehaviour {
        [Tooltip("The tracking mode to set on the device at startup.")]
        [SerializeField]
        private TrackingOriginModeFlags _mode = TrackingOriginModeFlags.Floor;

        private static readonly List<XRInputSubsystem> _cachedSubsystems = new List<XRInputSubsystem>();

        private XRInputSubsystem _inputSubsystem;

        private void Awake() {
            SetTrackingOriginMode(_mode);
        }
 
        private void SetTrackingOriginMode(TrackingOriginModeFlags mode) {
            if (_inputSubsystem == null) {
                SubsystemManager.GetInstances(_cachedSubsystems);
                if (_cachedSubsystems.Count > 0) {
                    // Assume there's only 1 XR system at a time
                    _inputSubsystem = _cachedSubsystems[0];
                } else {
                    // Don't warn in the editor since the device is often disconnected during normal workflow
                    #if !UNITY_EDITOR
                    Debug.LogWarning($"Failed to resolve an instance of {nameof(XRInputSubsystem)}. This often means no XR device is currently connected.");
                    #endif
                    return;
                }
            }

            if (_inputSubsystem.TrySetTrackingOriginMode(mode)) {
                Debug.Log($"Successfully set tracking origin mode to [{_inputSubsystem.GetTrackingOriginMode()}]");
            } else {
                Debug.LogError($"Failed to set tracking origin mode. It will remain as {_inputSubsystem.GetTrackingOriginMode()}.");
            }
        }
    }
}
