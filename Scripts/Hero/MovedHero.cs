using UnityEngine;

public class MovedHero : MonoBehaviour
{
    [SerializeField] private Selection _selection;
    [SerializeField] private Camera _camera;
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

    void Update()
    {
        if(_selected == null)return;
        if(Input.GetButtonDown("Fire2") && _selected.transform.gameObject.tag == "Hero")
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                _selected.transform.gameObject.GetComponent<Hero>().Move(hit.point);                
            }
            //Debug.Log("MovedHero  " + _selected.transform.gameObject.name + "  " + Input.mousePosition);
        }
    }
}
