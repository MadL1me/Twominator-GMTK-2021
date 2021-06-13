using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintProvider : MonoBehaviour
{

    [SerializeField] private TextMesh _text;
    
    private bool _touching;

    private void Start()
    {
        _text.GetComponent<MeshRenderer>().sortingOrder = 20;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") && other.gameObject.name != "PlayerDummy")
            return;

        _touching = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player") && other.gameObject.name != "PlayerDummy")
            return;
        
        _touching = false;
    }

    private void Update()
    {
        if (!_touching)
            _text.color = new Color(1F, 1F, 1F, Mathf.Max(0F, _text.color.a - Time.deltaTime * 2F));
        else if (_touching)
            _text.color = new Color(1F, 1F, 1F, Mathf.Min(1F, _text.color.a + Time.deltaTime * 2F));
    }
}
