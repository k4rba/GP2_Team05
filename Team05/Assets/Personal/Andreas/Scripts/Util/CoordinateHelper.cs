using UnityEngine;

namespace Util
{
    public class CoordinateHelper
    {
        public static int WorldCoordsToChunkTileIndex(int x, int y, int chunkSize, Vector2Int offset)
        {
            int cx = x - offset.x * chunkSize;
            int cy = y - offset.y * chunkSize;
            return cx + cy * chunkSize;
        }
        
        public static void WorldCoordsToChunkCoords(int x, int y, int chunkSize, out int cx, out int cy)
        {
            cx = x >= 0 ? x / chunkSize : (x + 1) / chunkSize - 1;
            cy = y >= 0 ? y / chunkSize : (y + 1) / chunkSize - 1;
        }
        
        public static int WorldCoordsToHash(int x, int y, int chunkSize)
        {
            x = x >= 0 ? x / chunkSize : (x + 1) / chunkSize - 1;
            y = y >= 0 ? y / chunkSize : (y + 1) / chunkSize - 1;
            return GetHash(x, y);
        }

        public static int GetHash(int x, int y)
        {
            int hash = new Vector3(x, 0, y).GetHashCode();
            // (x, y).GetHashCode();
            return hash;
        }
        
        public static void PositionToWorldCoords(int px, int py, int tileSize, out int x, out int y)
        {
            x = px / tileSize;
            y = py / tileSize;
            if(px <= 0)
                x--;
            if(py <= 0)
                y--;
        }

        public static void PositionToWorldCoords(float px, float py, int tileSize, out int x, out int y)
        {
            PositionToWorldCoords((int)px, (int)py, tileSize, out x, out y);
        }
    }
}