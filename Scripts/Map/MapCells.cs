using System;
using UnityEngine;

public class MapCells : MonoBehaviour
{    
//------------------------------------------------ test -----------------------------------------------------------------
[SerializeField] private GameObject close;
//-------------------------------------------------------------------------------------------------------------------------
    [SerializeField] public float size {get; private set;} = 0.2f; // размер ячейки
    private bool[, ] _mapFreedomCells; // Карта ячеек (свободны или заняты)
    private int [, ] _mapStepsCells; // карта значений шагов от стартовой до финишной позиции
    public int _quantityCellsWidth {get; private set;} // Количество ячеек по ширине
    public int _quantityCellsHight {get; private set;}  // Количество ячеек по высоте

    private void Start() 
    {
        _quantityCellsWidth = Convert.ToInt32(GetComponent<MeshFilter>().sharedMesh.bounds.size.x * transform.localScale.x / size);
        _quantityCellsHight = Convert.ToInt32(GetComponent<MeshFilter>().sharedMesh.bounds.size.z * transform.localScale.z / size);
        Debug.Log(GetComponent<MeshFilter>().sharedMesh.bounds.size.z);
        Debug.Log(_quantityCellsWidth + "    " + _quantityCellsHight);
        _mapFreedomCells = new bool[_quantityCellsWidth, _quantityCellsHight];
        _mapStepsCells = new int[_quantityCellsWidth, _quantityCellsHight];

        int LayerMask = 1 << 6;
        LayerMask = ~LayerMask;
        _quantityCellsWidth = _mapFreedomCells.GetLength(0);
        _quantityCellsHight = _mapFreedomCells.GetLength(1);
        for (int i = 0; i < _quantityCellsWidth; i++)
        {
            for (int j = 0; j < _quantityCellsHight; j++)
            {
                if(Physics.SphereCast(new Vector3(i * size, 3, j * size), size / 2, Vector3.down, out RaycastHit hit, 2.5f, LayerMask))
                {
                    Debug.Log(hit.transform.gameObject.name);
                    CloseCell(new Coords(i,j));

                    //------------------------------------------------------------ test --------------------------------------------
                        //GameObject closed = Instantiate(close);
                        //closed.transform.position = new Vector3(i * size + size/2, 0, j * size + size/2);
                    //--------------------------------------------------------------------------------------------------------------
                }
                else 
                {
                    OpenCell(new Coords(i,j));
                }
            }
        }
    }
    public void CloseCell(Coords coords)
    {
        _mapFreedomCells[coords.x, coords.y] = false;
    }
    public void OpenCell(Coords coords)
    {
        _mapFreedomCells[coords.x, coords.y] = true;
    }
    public bool CheckCell(Coords coords)
    {
        return _mapFreedomCells[coords.x, coords.y];
    }
    public void SetStepCell(Coords coords, int step)
    {
        _mapStepsCells[coords.x, coords.y] = step;
    }

    public int GetStepCell(Coords coords)
    {
        return _mapStepsCells[coords.x, coords.y];
    }
    public void ClearMapSteps()
    {
        for (int i = 0; i < _mapStepsCells.GetLength(0); i++)
        {
            for (int j = 0; j < _mapStepsCells.GetLength(1); j++)
            {
                _mapStepsCells[i, j] = 0;
            }
        }
    }
}
