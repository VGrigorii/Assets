using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private float _maxHeight = 8.0f;
    [SerializeField] private float _minHeight = 0.0f;
    [SerializeField] private float _maxRotation = 75.0f;
    [SerializeField] private float _minRotation = 0.0f;
    [SerializeField] private float _scale = 0.05f;
    [SerializeField] private float _stepMove = 1f;
    [SerializeField] private Selection _selection;
    private GameObject _selected;

    private void OnEnable() 
    {
        _selection.Choose += Selected;
    }
    private void OnDisable() 
    {
        _selection.Choose -= Selected;
    }
    private void Selected(GameObject choose)
    {
        _selected = choose;
    }
    private float _step;

    
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
        transform.position = new Vector3(positionCamera.x, (_maxHeight - _minHeight) * _step, positionCamera.z);
        if(_step != 0 || _selected == null) transform.eulerAngles = new Vector3 ((_maxRotation - _minRotation) * _step, 0, 0);
        else transform.eulerAngles = new Vector3 ((_maxRotation - _minRotation) * _step, _selected.transform.eulerAngles.y, 0);
    }
    private void MovePosition(Vector3 movePosition)
    {
        Vector3 pos = transform.position;
        if(movePosition.x < 0)pos.x -= _stepMove * Time.deltaTime;
        if(movePosition.x > Screen.width)pos.x += _stepMove * Time.deltaTime;
        if(movePosition.y < 0)pos.z -= _stepMove * Time.deltaTime;
        if(movePosition.y > Screen.height)pos.z += _stepMove * Time.deltaTime;
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
