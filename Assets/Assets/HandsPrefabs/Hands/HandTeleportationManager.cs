using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HandTeleportationManager : MonoBehaviour
{
    [SerializeField] private XRBaseInteractor _teleportController;
    [SerializeField] private XRBaseInteractor _mainController;

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
        var interactable = arg0.interactable;
        if (interactable is BaseTeleportationInteractable) return;

        if (interactable) _teleportController.interactionManager.ForceSelect(_mainController, interactable);
    }

}
