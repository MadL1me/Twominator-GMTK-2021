using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class InstantSceneLoader : MonoBehaviour
    {
        private void Start()
        {
            SceneManager.LoadScene(2);
        }
    }
}