using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class MusicProgressionController : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip[] _loopPhases;

        private int _loopPhase = 0;
        
        public void Play()
        {
            if (!_audioSource.isPlaying)
                _audioSource.Play();
        }

        public void Stop()
        {
            _audioSource.Stop();
        }
        
        public void SetLoopPhase(int loopPhase)
        {
            _loopPhase = loopPhase;
        }

        private void Update()
        {
            if (!_audioSource.isPlaying)
            {
                _audioSource.clip = _loopPhases[_loopPhase];
                _audioSource.Play();
            }
        }
    }
}