using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintProvider : MonoBehaviour
{
    private static int _touching;

    public string Text;
    
    private TextMesh _text;
    private bool _active;

    private void Start()
    {
        _text = GameObject.Find("Hint").GetComponent<TextMesh>();
        _text.GetComponent<MeshRenderer>().sortingOrder = 20;
        _touching = 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || other.gameObject.name == "PlayerDummy")
            return;
        
        _touching++;
        _active = true;
        _text.text = Text;
        _text.transform.position = transform.position + Vector3.up * 2F;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || other.gameObject.name == "PlayerDummy")
            return;
        
        _touching--;
        _active = false;
    }

    private void Update()
    {
        if (_touching == 0 && !_active)
            _text.color = new Color(1F, 1F, 1F, Mathf.Max(0F, _text.color.a - Time.deltaTime * 2F));
        else if (_active)
            _text.color = new Color(1F, 1F, 1F, Mathf.Min(1F, _text.color.a + Time.deltaTime * 2F));
    }
}
