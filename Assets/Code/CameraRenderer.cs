using UnityEngine;
using System;

[RequireComponent(typeof(Camera))]
public class CameraRenderer : MonoBehaviour
{
    public enum TargetEye { Left = 0, Right = 1 };
    public TargetEye targetEye;
    public StreamReceiver streamReceiver;
    private Material leftEyeMaterial;
    private Material rightEyeMaterial;


    void Start()
    {
        // Load the shader
        Shader unlitTextureShader = Shader.Find("Custom/UnlitTexture");
        if (unlitTextureShader == null)
        {
            Debug.LogError("Shader not found.");
            return;
        }

        leftEyeMaterial = new Material(unlitTextureShader);
        rightEyeMaterial = new Material(unlitTextureShader);
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {   
        if (targetEye == TargetEye.Left)
        {
            leftEyeMaterial.mainTexture = streamReceiver.leftEyeTexture;
            Graphics.Blit(streamReceiver.leftEyeTexture, dest, leftEyeMaterial);
        }
        else if (targetEye == TargetEye.Right)
        {   
            rightEyeMaterial.mainTexture = streamReceiver.rightEyeTexture;
            Graphics.Blit(streamReceiver.rightEyeTexture, dest, rightEyeMaterial);
        }
        else
        {
            Debug.Log("Eye target unknown");
        }
    }
}