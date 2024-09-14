/* using System.Collections.Generic; 
using UnityEngine;

public class RoutePlanning : MonoBehaviour
{
    private float _step = 0.1f;
    private int _stepCell = 0;
    private float size = 0.4f;
    private MapCells _mapCells = new MapCells(); 
    private List<Vector2> _route = new List<Vector2>();
    private List<Coords> _around = new List<Coords>();
    private List<Coords> _survey = new List<Coords>();
    private List<Coords> _agent;
    private Coords secondCoords;
    private bool finish = false;
    private bool CheckBackRoute = false;

    private void Start() 
    {

    }

    public List<Vector2> Route(Vector2 firstPos, Vector2 secondPos)
    {
        _mapCells.ClearMapSteps();
        _route.Clear();
        _around.Clear();
        _survey.Clear();
        finish = false;
        _route.Add(secondPos);
        _around.Add(DefinitionCell(firstPos.x, firstPos.y));
        secondCoords = DefinitionCell(secondPos.x, secondPos.y);
        Debug.Log(" Second " + secondCoords.x + "  " + secondCoords.y);
        if(RaycastRoute(firstPos, secondPos))
        {
            return _route;
        }
        else 
        {
            while (true)
            {
                Debug.Log("Завис !!!!!!!!!!!!!!");
                _agent = _survey;
                _survey = _around;
                _around = _agent;
                _around.Clear();

                foreach (var item in _survey)
                {
                    if(_survey.Count == 0) 
                    {
                        _route.Clear();
                        return _route;
                    }
                    if(finish)
                    {
                        Debug.Log("//////////////////////////////////////////////////////////////////////////////////////////////////////////");
                        while (_stepCell != 0)
                        {
                            Debug.Log("Завис2 !!!!!!!!!!!!!!");
                            //Debug.Log("step" + _stepCell);
                            _stepCell -= 1;
                            BackRoute();                        
                        }
                        _route.Add(firstPos);
                        return _route;
                    }
                    SetStepCellAround(item.x, item.y);
                }                
            }
        }
        return _route;
    }
    private void BackRoute()
    {
        if(secondCoords.x > 0 && secondCoords.x < 50)
        {
            if(secondCoords.y > 0 && secondCoords.y < 50)
            {
                if(!CheckBackRoute)SearchRoute(secondCoords.x - 1, secondCoords.y + 1);
                if(!CheckBackRoute)SearchRoute(secondCoords.x - 1, secondCoords.y);
                if(!CheckBackRoute)SearchRoute(secondCoords.x - 1, secondCoords.y - 1);
                if(!CheckBackRoute)SearchRoute(secondCoords.x, secondCoords.y - 1);
                if(!CheckBackRoute)SearchRoute(secondCoords.x + 1, secondCoords.y - 1);
                if(!CheckBackRoute)SearchRoute(secondCoords.x + 1, secondCoords.y);
                if(!CheckBackRoute)SearchRoute(secondCoords.x + 1, secondCoords.y + 1);
                if(!CheckBackRoute)SearchRoute(secondCoords.x, secondCoords.y + 1);
            }
        }
        CheckBackRoute = false;
    }
    private void SearchRoute(int x, int y)
    {
        if(_mapCells.GetStepCell(x, y) == _stepCell) 
        {
            //Debug.Log("BackRoute" + x + "   " + y);
            _route.Add(new Vector2(x * size, y * size));


            GameObject closed = GameObject.CreatePrimitive(PrimitiveType.Cube);
            closed.transform.position = new Vector3(x * size, 0, y * size);


            secondCoords.x = x;
            secondCoords.y = y;
            CheckBackRoute = true;
        }
        //else Debug.Log("BackRoute /////////////////////////////////////// " + x + "   " + y + "  " + _mapCells.GetStepCell(x, y));
    }

    private bool RaycastRoute(Vector2 firstPos, Vector2 secondPos)
    {
        Vector2 distance = firstPos - secondPos;
        Debug.Log("distance " + distance);
        //Debug.Log("distance.magnitude " + distance.magnitude);
        Debug.Log("firstPos " + firstPos + "  " + secondPos);
        int quantitySteps = (int)(distance.magnitude/_step);
        //Debug.Log("quantitySteps " + quantitySteps);
        float sizeStepX = distance.x /quantitySteps;
        float sizeStepY = distance.y /quantitySteps;
        //Debug.Log()
        for (int i = 0; i < quantitySteps; i++)
        {
            Vector2 checkPos = new Vector2(firstPos.x - sizeStepX * i, firstPos.y - sizeStepY * i);
            Coords coordsCell = DefinitionCell(checkPos.x, checkPos.y);
            Debug.Log("coordsCell = " + coordsCell.x + "  " + coordsCell.y);
            //Debug.Log(_mapCells.CheckCell(coordsCell.x, coordsCell.y));
            if(!_mapCells.CheckCell(coordsCell.x, coordsCell.y)) 
            {
                Debug.Log("------------------------------------------------------------------------------------------------");
                return false;
            }
        }
        return true;
    }
    private Coords DefinitionCell (float x, float y)
    {
        int a = (int)(x/size);
        int b = (int)(y/size);
        Coords addressCell = new Coords(a, b);
        return addressCell;
    }
    private void SetStepCellAround(int x, int y)
    {
        _stepCell = _mapCells.GetStepCell(x, y) + 1;
        Debug.Log("поиск пути " + x + "  " + y + "   " + _stepCell);
        if(x > 0 && x < 49)
        {
            if(y > 0 && y < 49)
            {
                ChecCell(x - 1, y + 1, _stepCell);
                ChecCell(x - 1, y, _stepCell);
                ChecCell(x - 1, y - 1, _stepCell);
                ChecCell(x, y - 1, _stepCell);
                ChecCell(x + 1, y - 1, _stepCell);
                ChecCell(x + 1, y, _stepCell);
                ChecCell(x + 1, y + 1, _stepCell);
                ChecCell(x, y + 1, _stepCell);    
            }
        }
    }
    private void ChecCell(int x, int y, int k)
    {
        //Debug.Log("поиск пути" + x + "  " + y);
        if(_mapCells.CheckCell(x, y))
        {
            if(_mapCells.GetStepCell(x, y) == 0)
            {
                _around.Add(new Coords(x, y));
                _mapCells.SetStepCell(x, y, k);
                if(secondCoords.x == x && secondCoords.y == y) finish = true;
            }
        } 
    }
}
 */