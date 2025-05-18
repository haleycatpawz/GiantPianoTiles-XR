using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace Hands
{
    public class XRHandAnimator : MonoBehaviour
    {
        [SerializeField] private ActionBasedController _controller;
        [SerializeField] private Animator _animator;


        // Start is called before the first frame update
        void Start()
        {
            _controller.selectAction.action.started += Point;
            _controller.selectAction.action.canceled += PointReleased;

            _controller.activateAction.action.started += Fist;
            _controller.activateAction.action.canceled += FistReleased;
        }

        private void Point(InputAction.CallbackContext ctx)
        {
            _animator.SetBool("Pointed", true);
        
        }
        private void PointReleased(InputAction.CallbackContext obj)
        {
            _animator.SetBool("Pointed", false);
        }

        private void Fist(InputAction.CallbackContext context)
        {
            _animator.SetBool("Fist", true) ;
        }
        private void FistReleased(InputAction.CallbackContext context)
        {
            _animator.SetBool("Fist", false) ;
        }


        // control stick, read value, x y z, x * value, to move postiion of something, 
        // 09:44 video 2 week 6 hand anyimator

        // Update is called once per frame
        void Update()
        {

        }
    }
}