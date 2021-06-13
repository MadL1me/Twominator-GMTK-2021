using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleAnimation : MonoBehaviour
{
    private Vector3 _startPos;
    private RectTransform _rect;
    
    private void Start()
    {
        _rect = GetComponent<RectTransform>();
        _startPos = _rect.position;
    }

    private void Update()
    {
        _rect.position = new Vector3(_startPos.x, _startPos.y + Mathf.Cos(Mathf.PI * Time.timeSinceLevelLoad) * 0.1F);
    }
}
