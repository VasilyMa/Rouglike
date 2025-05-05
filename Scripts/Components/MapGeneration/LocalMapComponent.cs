using UnityEngine;
namespace Client {
    struct LocalMapComponent {
        public LocalMapPoint[,] PointsArray;
        public LocalMapPoint CurrentLocalMapPoint;
        public Transform StartTransform;
        public Vector2Int LastPosition;
        public Vector2Int PreLastPosition;
        public int MaxWidth;
        public int MaxLength;

    }
}