using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Selection : MonoBehaviour
{

    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private GameObject SelectionPlane;

    public event Action<List<GameObject>> Choose;
    private Vector3 DownPosition;
    private Vector3 UpPosition;
    private Vector3 UnitsPosition;
    private Vector3 SelectedPosition;
    private Vector3 _moveSelectedPosition = new Vector3(0, 0.01f, 0);
    private GameObject _selectionPlane;
    private bool block = false;
    private List<GameObject> _selected = new List<GameObject>();
    private GameObject [] _selectedAll;
    private void Start() 
    {
        _selectionPlane = Instantiate(SelectionPlane);
        _selectionPlane.SetActive(false);    
    }
    private void SelectionColor(List<GameObject> _list, Color color)
    {
        if(_selected.Count == 0) return;
        foreach (var item in _list)
        {
            item.transform.GetChild(1).GetComponent<Renderer>().material.color = color;
        }
    }
    
    void Update()
    {
        if(Input.GetButtonDown("Fire1") && !block)
        {
            Ray rayDown = _camera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(rayDown, out RaycastHit hitDown, Mathf.Infinity, layerMask))
            {
                if(hitDown.transform.gameObject.tag == "Earth")
                {
                    DownPosition = hitDown.point;
                    _selectionPlane.SetActive(true);
                }
            }

            block = true;
        }
        if(Input.GetButtonUp("Fire1"))
        {               
            Ray rayUP = _camera.ScreenPointToRay(Input.mousePosition);
            if(_selected.Count > 0) 
            {
                SelectionColor(_selected, Color.white);
                _selected.Clear();
            }
            if(Physics.Raycast(rayUP, out RaycastHit hitUp, Mathf.Infinity, layerMask))
            {
                if(hitUp.transform.gameObject.tag == "Earth")UpPosition = hitUp.point;
            }
            else UpPosition = SelectedPosition;
                if(Mathf.Abs(DownPosition.x - UpPosition.x) > 0.2 || Mathf.Abs(DownPosition.y - UpPosition.y) > 0.2)
                {
                    _selectedAll = GameObject.FindGameObjectsWithTag ("Hero");
                    foreach (var item in _selectedAll)
                    {
                        UnitsPosition = item.transform.position;
                    Debug.Log(DownPosition + "   " + UpPosition + "  " + UnitsPosition);
                        if(((UnitsPosition.x > DownPosition.x && UnitsPosition.x < UpPosition.x) ||
                        (UnitsPosition.x < DownPosition.x && UnitsPosition.x > UpPosition.x)) &&
                        ((UnitsPosition.z > DownPosition.z && UnitsPosition.z < UpPosition.z) ||
                        (UnitsPosition.z < DownPosition.z && UnitsPosition.z > UpPosition.z))) _selected.Add(item);
                    }
                    SelectionColor(_selected, Color.green);
                    Choose?.Invoke(_selected);
                }
                else
                {
                    if(Physics.Raycast(rayUP, out RaycastHit hit, Mathf.Infinity))
                    {
                        _selected.Add(hit.transform.gameObject);
                        Choose?.Invoke(_selected);
                        if(hit.transform.gameObject.tag != "Hero")
                        {
                            _selected.Clear();
                        }
                        SelectionColor(_selected, Color.green);
                    }
                    else
                    {
                        SelectionColor(_selected, Color.white);
                        _selected.Clear();
                    }
                }
            block = false;
            _selectionPlane.SetActive(false);
        }
        //-------------------------------------- Для рисования выделяемой области ---------------------------------
        if(block)
        {
            Ray raySelected = _camera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(raySelected, out RaycastHit hitSelected, Mathf.Infinity, layerMask))
            {
                if(hitSelected.transform.gameObject.tag == "Earth")SelectedPosition = hitSelected.point;
                //Debug.Log(SelectedPosition);
            }            
            Vector3 _localScaleSelectionPlane = new Vector3(Mathf.Abs(DownPosition.x - SelectedPosition.x) * 0.1f,
                _selectionPlane.transform.localScale.y, Mathf.Abs(DownPosition.z - SelectedPosition.z) * 0.1f);
            _moveSelectedPosition.x = DownPosition.x + (SelectedPosition.x - DownPosition.x)/2;
            _moveSelectedPosition.z = DownPosition.z + (SelectedPosition.z - DownPosition.z)/2;
            _selectionPlane.transform.position = _moveSelectedPosition;
            _selectionPlane.transform.localScale = _localScaleSelectionPlane;
        }
        //---------------------------------------------------------------------------------------------------------
    }
}
