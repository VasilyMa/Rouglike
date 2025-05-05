using UnityEngine;
using System.Collections.Generic;
namespace Client {
    struct GlobalMapComponent {
        public int MaxLength;
        public int MaxWidth;
        public GlobalMapPoint[,] PointsArray;
        public Vector2Int CurrentGlobalMapPointPosition; //TODO Generation заполнить позицию по клику из UI
        //CHAPTER
        public int BiomCount;
        public int[] BiomsIndexes;
        public int CurrentBiomIndex;
        //ForUI
        public int BiomeLenLength;
    }
}