using UnityEngine;

public class ExplosionEventTest : MonoBehaviour  {
    // Debug UI so we can trigger an event from the Unity editor.
    [SerializeField] private bool _emit;
    [SerializeField, Range(0.1f, 1.0f)] private float _scale = 0.3f;

    private void Update() {
        // Check if the emit button has been pressed.
        if (_emit) {
            // Fire an event at the current position with the scale value set in Unity.
            GetComponent<ExplosionEvent>().Emit(transform.position, _scale);

            _emit = false;
        }
    }
}
