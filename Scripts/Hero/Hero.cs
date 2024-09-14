using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private TestRoute _testRoute;
    private Vector3 newPosition;
    //private RoutePlanning _routePlanning = new RoutePlanning();
    private int _countSteps;
    private Vector2 [] _route;
    
    private void Start() 
    {
        newPosition = transform.position;
    }
    public void Move(Vector3 nextPosition)
    {
        //newPosition = nextPosition;
        //List<Vector2> list = _routePlanning.Route(new Vector2(transform.position.x, transform.position.z), new Vector2(nextPosition.x, nextPosition.z));
        //_countSteps = list.Count;
        //if(_countSteps == 0) return;
        //_route = new Vector2[_countSteps];
        //list.CopyTo(_route);
        //foreach (var item in _route)
        //{
            //Debug.Log("item  " + item);
        //}
        _route = _testRoute.PathFinding(new Vector2(transform.position.x, transform.position.z), new Vector2(nextPosition.x, nextPosition.z));
        _countSteps = _route.Length;
        newPosition = new Vector3(_route[_countSteps - 1].x, 0, _route[_countSteps - 1].y);
    }
    private void NextStep()
    {
        _countSteps--;
        if(transform.position.x == _route[_countSteps].x && transform.position.z == _route[_countSteps].y)
        {
            if(_countSteps == 0) return;
            newPosition = new Vector3(_route[_countSteps - 1].x, 0, _route[_countSteps - 1].y);
        }
        
    }
    private void Update()
    {
        if(transform.position.x == newPosition.x && transform.position.z == newPosition.z)
        {
            //Debug.Log("safsdfsadfsdf/////////////////////////////////////////afdfdfdsf" + _countSteps);
            if(_countSteps == 0) return;
            NextStep();            
        }

        //Debug.Log("Hero  " + transform.position + "   " + newPosition);
        transform.position = Vector3.MoveTowards(transform.position, newPosition, _speed * Time.deltaTime);
    }
}
