using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatInPlace : MonoBehaviour {

    public Transform PlaceToFloat;
    public float FloatOffset;
    public float FloatSpeed;
    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private Vector3 _currentPosition;

    void Start () {
        if (PlaceToFloat == null)
            PlaceToFloat = this.gameObject.transform;

        _startPosition = new Vector3(PlaceToFloat.transform.position.x, 
            PlaceToFloat.transform.position.y + FloatOffset, 
            PlaceToFloat.transform.position.z);
        _endPosition = new Vector3(PlaceToFloat.transform.position.x, 
            PlaceToFloat.transform.position.y - FloatOffset, 
            PlaceToFloat.transform.position.z);

        _currentPosition = _startPosition;
       
    }
	
	void Update () {
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, _currentPosition, FloatSpeed * Time.deltaTime);

        if (gameObject.transform.position.y == _startPosition.y)
        {
            _currentPosition.y = _endPosition.y;
        } else if (gameObject.transform.position.y == _endPosition.y)
        {
            _currentPosition.y = _startPosition.y;
        }
	}
}
