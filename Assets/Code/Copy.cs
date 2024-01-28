using UnityEngine;
using WebSocketSharp;
using System;
[RequireComponent(typeof(Camera))]
public class Copy : MonoBehaviour
{
    public Texture2D imageToRender;
    private Material renderMaterial;

    void Start()
    {
        // Load the shader
        Shader unlitTextureShader = Shader.Find("Custom/UnlitTexture");
        if (unlitTextureShader == null)
        {
            Debug.LogError("Shader not found.");
            return;
        }

        renderMaterial = new Material(unlitTextureShader);

        // Assign the image as texture
        if (imageToRender != null)
        {
            renderMaterial.mainTexture = imageToRender;
        }
        else
        {
            Debug.LogError("Image to render is not assigned.");
        }
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (renderMaterial != null)
        {
            Graphics.Blit(imageToRender, dest, renderMaterial);
        }
    }
}
