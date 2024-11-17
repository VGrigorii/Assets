using System.Collections.Generic;
using UnityEngine;

public class MovedHero : MonoBehaviour
{
    [SerializeField] private Selection _selection;
    [SerializeField] private Camera _camera;
    private List<GameObject> _selected = new List<GameObject>();
    

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
        _selected = choose;
    }

    void Update()
    {
        if(_selected == null)return;
        if(Input.GetButtonDown("Fire2")) 
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                foreach (var item in _selected)
                {
                    if(item.transform.gameObject.tag == "Hero")
                    {
                        item.transform.gameObject.GetComponent<Hero>().Move(hit.point);                
                    }
                }                
            }
        }
    }
}
