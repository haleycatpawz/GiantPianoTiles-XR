using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Hands 
{
    public class XRHandHider : MonoBehaviour
    {
        [SerializeField] private UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInputInteractor _controller;
        [SerializeField] private Rigidbody _handRigidBody;
        [SerializeField] private ConfigurableJoint _configurableJoint;
        [SerializeField] private float _haandShowDelay = 0.16f;

        private Quaternion _originalHandRotation;


        // Start is called before the first frame update
        void Start()
        {
            _controller.selectEntered.AddListener(SelectEntered);
            _controller.selectExited.AddListener(SelectExited);
           /// Debug.Log("added listeners");
            _originalHandRotation = _handRigidBody.transform.rotation;
        }

        private void SelectEntered(SelectEnterEventArgs arg0)
        {
            if (arg0.interactableObject is UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.BaseTeleportationInteractable) return;
           // Debug.Log("select has been entered");
            _handRigidBody.gameObject.SetActive(false);
            _configurableJoint.connectedBody = null;
         //   Debug.Log("set active false should've been set");
          //  CancelInvoke(nameof(ShowHand));

        }

        private void SelectExited(SelectExitEventArgs arg0)
        {
            if (arg0.interactableObject is UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.BaseTeleportationInteractable) return;

            // Debug.Log("select exited has been triggers");
            Invoke(nameof(ShowHand), 1f);
        }

        private void ShowHand()
        {
            _handRigidBody.gameObject.SetActive(true);
            _handRigidBody.transform.position = _controller.transform.position;
            _handRigidBody.transform.rotation = Quaternion.Euler(_controller.transform.eulerAngles + _originalHandRotation.eulerAngles);
            _configurableJoint.connectedBody = _handRigidBody;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }


    // lesson learned : duplicated select entered twice
    // and forgot to rename it to select exited

 }