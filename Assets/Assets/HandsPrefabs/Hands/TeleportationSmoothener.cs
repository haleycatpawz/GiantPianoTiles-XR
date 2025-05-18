using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportationSmoothener : TeleportationArea
{
    [SerializeField] float teleportDuration;
    [SerializeField] float teleportSpeed = 0.5f;
    [SerializeField] float vignetteFadeDuration = 0.2f;
    [SerializeField] XROrigin originTransform;

    [SerializeField] float maxVignetteIntensity = 1.0f;
    [SerializeField] float minVignetteIntensity = 0.0f;


    [SerializeField] GameObject postProcessingObj;
   // [SerializeField] Vignette vignetteEffect;
   //
   //[SerializeField] Volume volumeComponent;
    private bool currentlyTeleporting = false;


    // overriding the OnSelectedEntered from teleportationArea component
    protected override void OnSelectEntered(SelectEnterEventArgs args0)
    {
        if (currentlyTeleporting == true) return;

        if (args0.interactorObject is XRRayInteractor rayInteractor)
        {
            if (!rayInteractor.TryGetCurrent3DRaycastHit(out var hit)) return;

            if (!hit.collider.TryGetComponent(out TeleportationArea area)) return;

            // calculating distance from the user's current position and raycast hit point (where the user is trying to teleport)
            // to detemine how long it takes to get to position, and keep a dynamic speed
            float teleportToDestDistance = Vector3.Distance(originTransform.transform.position, hit.point);
            Debug.Log("distance from current pos to new pos = " + teleportToDestDistance);

            teleportDuration = teleportToDestDistance * teleportSpeed;


         //   SmoothVignette(true);
            StartCoroutine(TeleportSmoothly(hit.point));
        
        }
        

    }

    // Start is called before the first frame update
    void Start()
    {
        SetVignetteIntensity(minVignetteIntensity);
    }

    IEnumerator TeleportSmoothly(Vector3 destinationPosition)
    {
        currentlyTeleporting = true;

   
        SetVignetteIntensity(maxVignetteIntensity);

        Transform xrOriginTransform = originTransform.transform;

        // lerping between current pos and target pos to smoothen the teleport transitioning. 
        for(float elapsedT = 0; elapsedT < teleportDuration; elapsedT += 0.10f)
        {
            xrOriginTransform.position = Vector3.Lerp(xrOriginTransform.position, destinationPosition, elapsedT);
        
            yield return null;
        }

        xrOriginTransform.position = destinationPosition;

        currentlyTeleporting = false;

      //  SmoothVignette(false);
       SetVignetteIntensity(minVignetteIntensity);
    }



    IEnumerator SmoothVignette(bool fadeIn)
    {
        
        // lerping between current pos and target pos to smoothen the teleport transitioning. 
        for (float elapsedT = 0; elapsedT < vignetteFadeDuration; elapsedT += 0.15f)
        {
            float intensityToSet;

            if (fadeIn) { 
             intensityToSet = Mathf.Lerp(minVignetteIntensity, maxVignetteIntensity, elapsedT);
            }
            else
            {
             intensityToSet = Mathf.Lerp(maxVignetteIntensity, minVignetteIntensity, elapsedT);
            }
            SetVignetteIntensity(intensityToSet);
            Debug.Log(intensityToSet);
            yield return null;
        }
        
    }



    private void SetVignetteIntensity(float intensity)
    {
        postProcessingObj.TryGetComponent<Volume>(out Volume vol);
        vol.profile.TryGet(out Vignette vignetteEffect);

    //    volumeComponent.TryGetComponent<Vignette>(out var vignetteEffect);

        if (vignetteEffect != null)
        {
            vignetteEffect.intensity.value = intensity;
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
