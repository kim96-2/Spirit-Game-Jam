using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class PlaneReflection : MonoBehaviour
{
    public Camera mainCamera;
    public Camera reflectionCamera;
    public Transform reflectionPlane;
    RenderTexture outputTexture;

    public float verticalOffset;
    bool isReady;

    Transform mainCamTransform;
    Transform reflectionCamTransform;

    
    // Update is called once per frame
    void FixedUpdate()
    {
        if (isReady) RenderReflection();
    }

    void RenderReflection()
    {
        Vector3 camDirectionWorldSpace = mainCamTransform.forward;
        Vector3 camUpWorldSpace = mainCamTransform.up;
        Vector3 camPositionWorldSpace = mainCamTransform.position;

        camPositionWorldSpace.y += verticalOffset;

        Vector3 camDirectionPlaneSpace = reflectionPlane.InverseTransformDirection(camDirectionWorldSpace);
        Vector3 camUpPlaneSpace = reflectionPlane.InverseTransformDirection(camUpWorldSpace);
        Vector3 camPositinoPlaneSpace = reflectionPlane.InverseTransformPoint(camPositionWorldSpace);

        camDirectionPlaneSpace.y *= -1;
        camUpPlaneSpace.y *= -1;
        camPositinoPlaneSpace.y *= -1;

        camDirectionWorldSpace = reflectionPlane.TransformDirection(camDirectionPlaneSpace);
        camUpWorldSpace = reflectionPlane.TransformDirection(camUpPlaneSpace);
        camPositionWorldSpace = reflectionPlane.TransformPoint(camPositinoPlaneSpace);

        reflectionCamTransform.position = camPositionWorldSpace;
        reflectionCamTransform.LookAt(camPositionWorldSpace + camDirectionWorldSpace, camUpWorldSpace);

        Shader.SetGlobalTexture("_PlaneReflectionTex", outputTexture);
    }

    private void Awake()
    {
        if (mainCamera != null)
        {
            mainCamTransform = mainCamera.transform;
            isReady = true;
        }
        else isReady = false;

        if (reflectionCamera != null)
        {
            reflectionCamTransform = reflectionCamera.transform;
            isReady = true;
        }
        else isReady = false;

        if(isReady)
        {
            reflectionCamera.fieldOfView = mainCamera.fieldOfView;
            reflectionCamera.nearClipPlane = mainCamera.nearClipPlane;
            reflectionCamera.farClipPlane = mainCamera.farClipPlane;

            outputTexture = new RenderTexture(mainCamera.pixelWidth / 2, mainCamera.pixelHeight / 2, 24, 
                                RenderTextureFormat.RGB111110Float
            );

            reflectionCamera.targetTexture = outputTexture;
        }
    }

    private void OnDestroy()
    {
        reflectionCamera.targetTexture?.Release();
    }
}