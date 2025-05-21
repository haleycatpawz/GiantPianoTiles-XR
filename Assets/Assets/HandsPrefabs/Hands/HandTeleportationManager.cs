using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using static UnityEditor.VersionControl.Message;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class HandTeleportationManager : MonoBehaviour
{
    [SerializeField] private UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor _teleportController;
    [SerializeField] private UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor _mainController;

    [SerializeField] private Animator _handAnimator;
    [SerializeField] private GameObject _pointer;


    private void Start()
    {
        _teleportController.selectEntered.AddListener(MoveSelectedToMainController);
    }

    void Update()
    {
        
        _pointer.SetActive(_handAnimator.GetCurrentAnimatorStateInfo(0).IsName("Pointed") && _handAnimator.gameObject.activeSelf);
    }

    private void MoveSelectedToMainController(SelectEnterEventArgs arg0)
    {
        var interactable = arg0.interactableObject;
        if (interactable is UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.BaseTeleportationInteractable) return;

    //    if (interactable != null) _teleportController.interactionManager.ForceSelect(_mainController, interactable);

        if (interactable != null)
        {
            XRBaseInteractable baseInteractable = interactable as XRBaseInteractable;
            if (baseInteractable != null)
            {
                _teleportController.interactionManager.SelectEnter(_mainController, interactable);

             //   _teleportController.interactionManager.ForceSelect(_mainController, (baseInteractable);
            }
            else
            {
                UnityEngine.Debug.LogError("Error: Selected interactable is not an XRBaseInteractable.", interactable as UnityEngine.Object);
            }
        }
    }

}
