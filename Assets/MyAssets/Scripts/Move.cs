using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Move : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 1;
   // [SerializeField] Vector3 _moveDirection;
    [SerializeField] Transform _startPosition;

    [SerializeField] bool useEndPositionTransform = false;
    [SerializeField] Vector3 _endPositionVector;
    [SerializeField] Transform _endPositionTransform;
    [SerializeField] float _distToTargetPosThreshold;

    [SerializeField] bool _restartWhenTargetPosReached = false;


    private Vector3 startPosition;


    private void Start()
    {
       startPosition = _startPosition.localPosition;

        if (useEndPositionTransform && _endPositionTransform != null)
        {
            _endPositionVector = _endPositionTransform.localPosition;
        }

       StartMoveCoroutine();
    }

    private void StartMoveCoroutine() { 

        StartCoroutine(MoveCoroutine());
    }

    IEnumerator MoveCoroutine()
    {
      while(Vector3.Distance(transform.localPosition, _endPositionVector) > _distToTargetPosThreshold) {
            var xPos = transform.localPosition.x;
            var xDest = _endPositionVector.x;
            var yPos = transform.localPosition.y;
            var yDest = _endPositionVector.y;
            var zPos = transform.localPosition.z;
            var zDest = _endPositionVector.z;


            var newX = moveToPosition(xPos, xDest);
            var newY = moveToPosition(yPos, yDest);
            var newZ = moveToPosition(zPos, zDest);
            
            transform.localPosition = new Vector3(newX, newY, newZ);

       //     Debug.Log(Vector3.Distance(transform.localPosition, _endPositionVector));

            yield return null;
       }

        if (_restartWhenTargetPosReached)
        {
            transform.localPosition = startPosition;
            StartMoveCoroutine();
            
            GetComponent<TileSet>().ResetThisTileSet();
        }
    }


    private float moveToPosition(float axisCurrentPosition, float axisEndPosition)
    {
        if (axisCurrentPosition < axisEndPosition) { 
              
            axisCurrentPosition = axisCurrentPosition + (_moveSpeed);
        }
        else if (axisCurrentPosition > axisEndPosition) { 
              
            axisCurrentPosition = axisCurrentPosition - (_moveSpeed);
        }
        else
          {
          //  Debug.Log("dist within range");
          }

        return axisCurrentPosition;
    }

}


