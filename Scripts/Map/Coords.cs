using System;

public class Coords
{
    public int x;
    public int y;
    public Coords(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    public override bool Equals(Object obj)
    {
        return Equals(obj as Coords);
    }
    public bool Equals(Coords obj)
    {
        return obj != null && x == obj.x && y == obj.y;
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(x, y);
    }
}
