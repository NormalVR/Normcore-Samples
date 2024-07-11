using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionParticleSystem : MonoBehaviour {
    [SerializeField]
    private bool _emit;

    [SerializeField]
    private AudioClip _audioClip = default;

    private ParticleSystem _particleSystem;
    private AudioSource    _audioSource;

    private void Awake() {
        // Store references for later
        _particleSystem = GetComponent<ParticleSystem>();
        _audioSource    = GetComponent<AudioSource>();
    }

    private void Update() {
        if (_emit) {
            Emit(transform.position, 0.7f);

            _emit = false;
        }
    }

    public void Emit(Vector3 position, float scale) {
        // Move to emit point
        transform.position = position;

        // Emit particles
        float cubedScale = scale*scale*scale;
        int   numberOfParticles = (int)(1000.0f * cubedScale);

        _particleSystem.Emit(numberOfParticles);

        // Trigger sound effect if the scale is >75%
        if (scale > 0.75f)
            _audioSource.PlayOneShot(_audioClip, scale);
    }
}
