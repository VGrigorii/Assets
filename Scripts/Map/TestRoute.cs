using System.Collections.Generic;
using UnityEngine;

public class TestRoute : MonoBehaviour
{
//--------------------------------------------------------------------------- Test ----------------------------------------------
[SerializeField] private GameObject gameObj;
[SerializeField] private GameObject test;
//-------------------------------------------------------------------------------------------------------------------------------

    [SerializeField] private MapCells _mapCells; // ссылка на карту
    private float _verificationStep = 0.1f; // шаг для поска свободного пространства при поиске маршрута
    private bool _stopFinding = true; // для цикла поиска пути
    private Coords _start; // координата стартовой ячейки поиска маршрута
    private Coords _finish; // координата финишной ячейки куда есть возможность дойти
    private Coords _backSteps;
    private Vector2 _second; // вектор куда будет стоиться маршнут в случае быбора занятой ячейки
    private int _step = 1;
    private List<Vector2> _listSteps = new List<Vector2>();
    private List<Coords> _listStepsCells = new List<Coords>();    
    private List<Coords> _finding = new List<Coords>();
    private List<Coords> _null;

    public Vector2 [] PathFinding(Vector2 first, Vector2 second) // возвращает массив векторов по которому происходит маршрут
    {
        ClearAll();
        if(_mapCells.CheckCell(HitCell(second))) _second = second;
        else _second = NearestFreeCell(second);
        Vector2 [] result;
        if(Straight(first,_second)) _listSteps.Add(_second);
        else
        {            
            _listStepsCells.Add(HitCell(first));
            _start = HitCell(first);
            _finish = HitCell(_second);
            _mapCells.SetStepCell(_start, _step);
            while (_stopFinding)
            {
                _step++;
                if(_listStepsCells.Count == 0)
                {
                    _stopFinding = false;
                    break;
                }
                _null = _finding;
                _finding = _listStepsCells;
                _listStepsCells = _null;
                _listStepsCells.Clear();

                foreach (var item in _finding)
                {
                    SetStepsCells(item);                  
                }
            }
        }

        _stopFinding = true;
        SearchPath(first, _second);
        int _countSteps = _listSteps.Count;
        result = new Vector2[_countSteps];
        _listSteps.CopyTo(result);
        //----------------------------------------------------- Test -------------------------------------------------------------
        /*
        foreach (var item in RouteOptimization(result))
        {
            GameObject closed = Instantiate(gameObj);
            closed.transform.position = new Vector3(item.x, 0, item.y);
        }
        */
        //------------------------------------------------------------------------------------------------------------------------
        return RouteOptimization(result);
    }
    private void ClearAll() // метод для очистки переменных используемых для поиска маршрута
    {
        _listSteps.Clear();
        _listStepsCells.Clear();
        _finding.Clear();
        _step = 1;
        _mapCells.ClearMapSteps();
    }

    private Vector2 NearestFreeCell(Vector2 second) //метод для поиска ближайшей точки в пространстве куда возможно постоить маршрут
    {
        List<Coords> listCellAround1 = new List<Coords>();
        List<Coords> listCellAround2 = new List<Coords>();
        List<Coords> listCellAroundZero;
        List<Coords> listCellFree = new List<Coords>();
        Coords sec = HitCell(second);
        listCellAround1.Add(sec);
        _mapCells.SetStepCell(sec, _step);
        Coords around;
        while (listCellFree.Count == 0)
        {
            if(listCellAround1.Count == 0) break;
            listCellAroundZero = listCellAround2;
            listCellAround2 = listCellAround1;
            listCellAround1 = listCellAroundZero;
            listCellAround1.Clear();
            foreach (var item in listCellAround2)
            {
                for (int i = item.x - 1; i <= item.x + 1; i++)        
                    for (int j = item.y - 1; j <= item.y + 1; j++)
                        if(InRange(around = new Coords(i, j)))
                        {
                            if(_mapCells.GetStepCell(around) == 0)
                            {
                                if(_mapCells.CheckCell(around))
                                {
                                    listCellFree.Add(around);
                                    continue;
                                }
                                listCellAround1.Add(around);  
                                _mapCells.SetStepCell(around, _step);
                            }
                        }                
            }
        }
        _mapCells.ClearMapSteps();
        float distance = 0f;
        float magnitude;
        Vector2 returnVector2 = Vector2.zero;
        foreach (var item in listCellFree)
        {
            Vector2 vect = new Vector2(item.x * _mapCells.size + _mapCells.size / 2, item.y * _mapCells.size + _mapCells.size / 2);
            magnitude = (vect - second).magnitude;
            if(distance == 0) 
            {
                distance = magnitude;
                returnVector2 = vect;
            }

            if(distance > magnitude) 
            {
                distance = magnitude;
                returnVector2 = vect;
            }
        }
        return returnVector2;
    }
    private Vector2 [] RouteOptimization(Vector2 [] listRoute) // метод для поиска оптимального маршрута по пути найденного маршрута ячеек
    {
        List<Vector2> routeOptimization = new List<Vector2>();
        routeOptimization.Add(listRoute[0]);
        Vector2 a = listRoute[0];
        for (int j = 1; j < listRoute.Length; j++)
        {
            if(Straight(a, listRoute[j]))
                if(j != listRoute.Length - 1)continue;
                else 
                {
                    routeOptimization.Add(listRoute[j]);
                    continue;
                }
            routeOptimization.Add(listRoute[j - 1]);
            a = listRoute[j - 1];
        }
        
        Vector2[] returnRoute = new Vector2[routeOptimization.Count];
        routeOptimization.CopyTo(returnRoute);
        return returnRoute;
    }
    private bool Straight(Vector2 first, Vector2 second) // проверка возможено ли прямое перемещение по координатам
    {
        Vector2 distance = first - second;        
        int quantitySteps = (int)(distance.magnitude/_verificationStep);
        float sizeStepX = distance.x /quantitySteps;
        float sizeStepY = distance.y /quantitySteps;
        Vector2 checkPos = Vector2.zero;
        Coords coordsCell;

        for (int i = 0; i < quantitySteps; i++)
        {
            checkPos.x = first.x - sizeStepX * i;
            checkPos.y = first.y - sizeStepY * i;
            coordsCell = HitCell(checkPos);
            
            if(!_mapCells.CheckCell(coordsCell)) 
            {
                return false;
            }
        }
        return true;
    }

    private Coords HitCell (Vector2 pos) // возвращает координату ячейки в которую попадает вектор
    {
        int a = (int)(pos.x/_mapCells.size);
        int b = (int)(pos.y/_mapCells.size);
        Coords addressCell = new Coords(a, b);
        return addressCell;
    }

    private List<Vector2> SearchPath(Vector2 first, Vector2 second) // поиск пути по сетке шагов
    {
        _listSteps.Add(second);
        _step--;
        _backSteps = _finish;
        while (_step > 1)
        {
            _listSteps.Add(SearchPathCells(_backSteps));
        }
                
        _listSteps.Add(first);
        return _listSteps;
    }

    private bool InRange(Coords coords) // проверяет не выходит ли координата за масштаб карты
    {
        return coords.x >= 0 && coords.x < _mapCells._quantityCellsWidth && coords.y >= 0 && coords.y < _mapCells._quantityCellsHight;
    }

    private List<Coords> SetStepsCells (Coords coords) // нумерует слой ячеек шагами от изночальной точки и возвращает лист с следующим слоем доступных клеточек
    {
        Coords around;
        for (int i = coords.x - 1; i <= coords.x + 1; i++)        
            for (int j = coords.y - 1; j <= coords.y + 1; j++)            
                if(InRange(around = new Coords(i, j)))
                {
                    if(!around.Equals(coords))
                         if(_mapCells.CheckCell(around))
                            if(_mapCells.GetStepCell(around) == 0)
                            {
                                _mapCells.SetStepCell(around, _step);
                                _listStepsCells.Add(around);
                                if(around.Equals(_finish)) 
                                    _stopFinding = false;
                            } 
                }
        
        return _listStepsCells;
    }
    private Vector2 SearchPathCells (Coords coords) // поиск ячеек по кротчайшему пути (если финишноя координата не достежима ищеться ближайшая доступная координата)
    {
        Vector2 resultVector2;
        
        int i;
        int j;
        if(coords.x < _finish.x && coords.y < _finish.y)
            for (i = coords.x + 1; i >= coords.x - 1; i--)
                for (j = coords.y + 1; j >= coords.y - 1; j--)
                    {
                        if((resultVector2 = SearchPathCells2(i, j, coords)) != Vector2.zero) return resultVector2;
                    }
        if(coords.x >= _finish.x && coords.y < _finish.y)
            for (i = coords.x - 1; i <= coords.x + 1; i++)
                for (j = coords.y + 1; j >= coords.y - 1; j--)
                    {
                        if((resultVector2 = SearchPathCells2(i, j, coords)) != Vector2.zero) return resultVector2;
                    }
        if(coords.x < _finish.x && coords.y >= _finish.y)
            for (i = coords.x + 1; i >= coords.x - 1; i--)
                for (j = coords.y - 1; j <= coords.y + 1; j++)
                    {
                        if((resultVector2 = SearchPathCells2(i, j, coords)) != Vector2.zero) return resultVector2;
                    }
        if(coords.x >= _finish.x && coords.y >= _finish.y)
            for (i = coords.x - 1; i <= coords.x + 1; i++)
                for (j = coords.y - 1; j <= coords.y + 1; j++)
                    {
                        if((resultVector2 = SearchPathCells2(i, j, coords)) != Vector2.zero) return resultVector2;
                    }       
        
        return WhenClosedCell(coords);
    }
    private Vector2 SearchPathCells2(int i, int j, Coords coords) // метод возвращает вектор для перемешения если он отвечает критериям
    {
        Coords around;
        if(InRange(around = new Coords(i, j)))
                {
                    if(!around.Equals(coords))
                        if(_mapCells.GetStepCell(around) == _step)
                        {
                            _step--;
                            _backSteps = around;
                            return new Vector2(around.x * _mapCells.size, around.y * _mapCells.size);
                        }
                }
        return Vector2.zero;
    }
    private Vector2 WhenClosedCell(Coords coord) // если координата места следования недоступна ищется ближайшая точка к ней и она является финишной координатой
    {
        _listSteps.Clear();
        List<Coords> listCellAround1 = new List<Coords>();
        List<Coords> listCellAround2 = new List<Coords>();
        List<Coords> listCellAroundZero;
        List<Coords> listCellFree = new List<Coords>();
        Coords sec = coord;
        listCellAround1.Add(sec);
        _mapCells.SetStepCell(sec, 1);
        Coords around;
        while (listCellFree.Count == 0)
        {
            if(listCellAround1.Count == 0) break;
            listCellAroundZero = listCellAround2;
            listCellAround2 = listCellAround1;
            listCellAround1 = listCellAroundZero;
            listCellAround1.Clear();
            foreach (var item in listCellAround2)
            {
                for (int i = item.x - 1; i <= item.x + 1; i++)        
                    for (int j = item.y - 1; j <= item.y + 1; j++)
                        if(InRange(around = new Coords(i, j)))
                        {
                            if(_mapCells.GetStepCell(around) > 1)
                            {
                                listCellFree.Add(around);
                                
                                //------------------------------------------------------------ test --------------------------------------------
                                // GameObject closed = Instantiate(test);
                                // closed.transform.position = new Vector3(around.x * _mapCells.size + _mapCells.size/2, 0, around.y * _mapCells.size + _mapCells.size/2);
                                //--------------------------------------------------------------------------------------------------------------
                                
                                continue;
                            }
                            if(_mapCells.GetStepCell(around) == 0)
                            {
                                _mapCells.SetStepCell(around, 1);
                                listCellAround1.Add(around);
                            }
                        }                
            }
        }
        float distance = 0f;
        float magnitude;
        Vector2 returnVector2 = Vector2.zero;
        Debug.Log(listCellFree.Count);
        foreach (var item in listCellFree)
        {
            Vector2 vect = new Vector2(item.x * _mapCells.size + _mapCells.size / 2, item.y * _mapCells.size + _mapCells.size / 2);
            magnitude = (vect - _second).magnitude;
            if(distance == 0) 
            {
                distance = magnitude;
                returnVector2 = vect;
                _step = _mapCells.GetStepCell(item) - 1;
                _backSteps = item;
        Debug.Log(_step);
            }

            if(distance > magnitude) 
            {
                distance = magnitude;
                returnVector2 = vect;
                _step = _mapCells.GetStepCell(item) - 1;
                _backSteps = item;
            }
        }
        return returnVector2;
    }
}

