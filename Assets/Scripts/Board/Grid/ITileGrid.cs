using System.Collections.Generic;

public interface ITileGrid<T>
{
    T GetTileAt(int x, int y);
    void SetTileAt(int x, int y, T tile);
    HashSet<(int x, int y)> GetPositionsInRange(int x, int y, int range);
}
