using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArrowSelector : MonoBehaviour
{
    private Vector3 _startPos;
    private RectTransform _rect;
    private int _selected;

    private void Start()
    {
        _rect = GetComponent<RectTransform>();
        _startPos = _rect.position;
    }

    private void Update()
    {
        var y = _startPos.y;

        if (_selected == 1)
            y = _startPos.y - 1.9F;
        
        _rect.position = new Vector3(_startPos.x + Mathf.Sin(2F * Mathf.PI * Time.timeSinceLevelLoad) * 0.2F, y);

        if (Input.GetKeyDown(KeyCode.DownArrow))
            _selected = _selected == 1 ? 0 : 1;
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            _selected = _selected == 0 ? 1 : 0;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_selected == 0)
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            else if (_selected == 1)
                Application.Quit();
        }
    }
}
