#if NORMCORE

using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace Normal.Realtime.Examples {
    public class CubePlayer : MonoBehaviour {
        public float speed = 5.0f;

        private RealtimeView      _realtimeView;
        private RealtimeTransform _realtimeTransform;

        private void Awake() {
            _realtimeView      = GetComponent<RealtimeView>();
            _realtimeTransform = GetComponent<RealtimeTransform>();
        }

        private void Update() {
            // If this CubePlayer prefab is not owned by this client, bail.
            if (!_realtimeView.isOwnedLocallySelf)
                return;

            // Make sure we own the transform so that RealtimeTransform knows to use this client's transform to synchronize remote clients.
            _realtimeTransform.RequestOwnership();

            // Grab the x/y input from WASD / a controller
            Vector2 input = GetMovementInput();

            // Apply to the transform
            Vector3 localPosition = transform.localPosition;
            localPosition.x += input.x * speed * Time.deltaTime;
            localPosition.y += input.y * speed * Time.deltaTime;
            transform.localPosition = localPosition;
        }
        
        private static Vector2 GetMovementInput()
        {
            Vector2 input;
#if ENABLE_INPUT_SYSTEM
            input = Vector2.zero;

            if (Keyboard.current != null)
            {
                if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
                    input.x += 1;
                if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
                    input.x -= 1;
                if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
                    input.y += 1;
                if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
                    input.y -= 1;
            }
            if (Gamepad.current != null)
                input += Gamepad.current.leftStick.ReadValue();
#else
            input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
#endif
            return Vector2.ClampMagnitude(input, 1f);
        }
    }
}

#endif
