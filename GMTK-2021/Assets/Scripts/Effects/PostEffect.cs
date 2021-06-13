using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostEffect : MonoBehaviour
{
    public Material PostMaterial;
    public float Intensity = 1F;

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        PostMaterial.SetInt("_Width", Screen.width);
        PostMaterial.SetInt("_Height", Screen.height);
        PostMaterial.SetFloat("_Aspect", Screen.height / (float)Screen.width);
        PostMaterial.SetFloat("_Intensity", Intensity);
        Graphics.Blit(src, dest, PostMaterial);
    }
}
