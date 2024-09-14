using System;
using UnityEngine;

public class Selection : MonoBehaviour
{

    [SerializeField] private Camera _camera;
    public event Action<GameObject> Choose;
    private GameObject _selected;
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                _selected = hit.transform.gameObject;
                Choose?.Invoke(_selected);
                Debug.Log("Selection  " + _selected.transform.gameObject.name);
            }
        }
    }
}
