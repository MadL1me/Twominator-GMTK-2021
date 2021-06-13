using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FontFix : MonoBehaviour
{
    public Font Font;

    private void Awake()
    {
        Font.material.mainTexture.filterMode = FilterMode.Point;
    }
}
