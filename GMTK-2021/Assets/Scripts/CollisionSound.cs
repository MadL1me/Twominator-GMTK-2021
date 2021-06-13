using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class CollisionSound : MonoBehaviour
    {
        [SerializeField] private AudioSource _dragSource;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private List<AudioClip> _clips;

        private const float _moveThreshold = 0.1f;
        private Vector2 _lastPosition;
        
        public void OnCollisionEnter2D(Collision2D other)
        {
            var clip = _clips[UnityEngine.Random.Range(0, _clips.Count)];
            _audioSource.clip = clip;
            _audioSource.Play();
        }

        public void OnCollisionStay2D(Collision2D other)
        {
            if (transform.hasChanged)
                _dragSource.Play();
            else
                _dragSource.Stop();
        }
    }
}