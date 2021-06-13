using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSwitch : MonoBehaviour
{
    public GameObject GameTitle;
    public GameObject Credits;

    private bool _switched;
    
    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space) && !Input.GetKeyDown(KeyCode.Return))
            return;

        if (!_switched)
        {
            GameTitle.SetActive(false);
            Credits.SetActive(true);
            _switched = true;
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }
}
