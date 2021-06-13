using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class Rotator : MonoBehaviour
    {
        public void Update()
        {
            transform.Rotate(new Vector3(0,0,5));
        }
    }
}