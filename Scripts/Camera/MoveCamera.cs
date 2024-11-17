using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private float _maxHeight = 8.0f;
    [SerializeField] private float _minHeight = 1.0f;
    [SerializeField] private float _maxRotation = 75.0f;
    [SerializeField] private float _minRotation = 0.0f;
    [SerializeField] private float _scale = 0.05f;
    [SerializeField] private float _stepMove = 1f;
    [SerializeField] private Selection _selection;
    [SerializeField] private float _maxDistanceX = 20f;
    [SerializeField] private float _maxDistanceZ = 20f;

    private GameObject _selected;
    private float _step;

    private void OnEnable() 
    {
        _selection.Choose += Selected;
    }
    private void OnDisable() 
    {
        _selection.Choose -= Selected;
    }
    private void Selected(List<GameObject> choose)
    {
        if(choose.Count == 1)
        foreach (var item in choose)
        {
            _selected = item;            
        }
    }

    
    private void Start()
    {
        _step = 1f;
        Approach();
    }
    private void Approach()
    {
        if(_selected == null) MovePositionAndRotation(transform.position);
        else MovePositionAndRotation(_selected.transform.position);
    }

    private void MovePositionAndRotation(Vector3 positionCamera)
    {
        transform.position = new Vector3(positionCamera.x, (_maxHeight - _minHeight) * _step + _minHeight, positionCamera.z);
        if(_step != 0 || _selected == null) transform.eulerAngles = new Vector3 ((_maxRotation - _minRotation) * _step, 0, 0);
        else transform.eulerAngles = new Vector3 ((_maxRotation - _minRotation) * _step, _selected.transform.eulerAngles.y, 0);
    }
    private void MovePosition(Vector3 movePosition)
    {
        Vector3 pos = transform.position;
        if(movePosition.x < 0 && pos.x > 0)pos.x -= _stepMove * Time.deltaTime;
        if(movePosition.x > Screen.width && pos.x < _maxDistanceX)pos.x += _stepMove * Time.deltaTime;
        if(movePosition.y < 0 && pos.z > 0)pos.z -= _stepMove * Time.deltaTime;
        if(movePosition.y > Screen.height && pos.z < _maxDistanceZ)pos.z += _stepMove * Time.deltaTime;
        transform.position = pos;
    }

    private void OnGUI()
    {
        if(Input.mouseScrollDelta.y == 0)return;
        _step += -Input.mouseScrollDelta.y * _scale;
        if(_step > 1) _step = 1;
        if(_step < 0) _step = 0;
        Approach();
    }
    private void Update() 
    {
        if(Input.mousePosition.x < 0 || Input.mousePosition.x > Screen.width || Input.mousePosition.y < 0 || Input.mousePosition.y > Screen.height)
            MovePosition(Input.mousePosition);
    }
}
