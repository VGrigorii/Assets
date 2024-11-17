/*using UnityEngine;

public class MapFreedomCells : MonoBehaviour
{    
    private int _quantityCellsWidth;
    private int _quantityCellsHight;
    private bool[, ] _mapFreedomCells;
    private int [, ] _mapStepsCells;
    public float size {get; private set;} = 0.4f;

    private void Start() 
    {
        _quantityCellsWidth = (int)(GetComponent<MeshFilter>().sharedMesh.bounds.size.x / size);
        _quantityCellsHight = (int)(GetComponent<MeshFilter>().sharedMesh.bounds.size.y / size);
        _mapFreedomCells = new bool[_quantityCellsWidth, _quantityCellsHight];
        _mapStepsCells = new int[_quantityCellsWidth, _quantityCellsHight];

        int LayerMask = 1 << 6;
        LayerMask = ~LayerMask;
        int iSize = _mapFreedomCells.GetLength(0);
        int jSize = _mapFreedomCells.GetLength(1);
        for (int i = 0; i < iSize; i++)
        {
            for (int j = 0; j < jSize; j++)
            {
                //Ray ray = new Ray(new Vector3(i * size, 3, j * size), Vector3.down);
                if(Physics.Raycast(new Vector3(i * size, 3, j * size), Vector3.down, out RaycastHit hit, 2.5f, LayerMask))
                {
                    Debug.Log("CloseCell");
                    Debug.Log(hit.transform.gameObject.name);
                    CloseCell(i,j);

                    
                    GameObject closed = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    closed.transform.position = new Vector3(i * size, 0, j * size);
                }
                else 
                {
                    Debug.Log("OpenCell");
                    OpenCell(i,j);
                }
            }
        }
    }
    public void CloseCell(int x, int y)
    {
        _mapFreedomCells[x, y] = false;
    }
    public void OpenCell(int x, int y)
    {
        _mapFreedomCells[x, y] = true;
    }
    public bool CheckCell(int x, int y)
    {
        return _mapFreedomCells[x, y];
    }
    public void SetStepCell(int x, int y, int step)
    {
        _mapStepsCells[x, y] = step;
    }

    public int GetStepCell(int x, int y)
    {
        return _mapStepsCells[x, y];
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
*/
