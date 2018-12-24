using System.Collections;
using System.Collections.Generic;

namespace Navi.System.State
{
    /// <remarks>
    /// Is based on the Save class of Navi (2014/2015).
    /// </remarks>
    public sealed class SaveGame<TStaticModel, TDynamicModel, TMapNode>
    {
        public SaveGame()
        {
            // 'Collision'
            Collision = new bool[0, 0];
            MapNodes = new TMapNode[0, 0];
            ////RegionDevider = new QuadTree<TDynamicModel>(new Rectangle());

            // 'HUD'
            // page 1
            PriorityQueue = new bool[3] { true, false, false };
            MainAlgorithm = new bool[8] { true, false, false, false, false, false, false, false };

            // page 2
            Expansion = new bool[2] { true, false };
            DistanceMeasure = new bool[4] { true, false, false, false };
            Options = new bool[1] { false };

            // page 3
            Optimizations = new bool[1] { false };
            Settings = new bool[2] { false, false };
            NumberOfUnits = 0;

            // 'Map Objects'
            Models = new ArrayList();
            SelectedUnits = new List<TDynamicModel>();
            Units = new List<TDynamicModel>();
            WalkingUnits = new List<TDynamicModel>();
        }

        #region collision
        public bool[,] Collision { get; set; }

        public TMapNode[,] MapNodes { get; set; }

        ////public QuadTree<TDynamicModel> RegionDevider { get; set; }
        #endregion

        #region hud
        // page 1
        public bool[] PriorityQueue { get; set; }

        public bool[] MainAlgorithm { get; set; }

        public bool[] DistanceMeasure { get; set; }

        // page 2
        public bool[] Expansion { get; set; }

        public bool[] Options { get; set; }

        // page 3
        public bool[] Optimizations { get; set; }

        public bool[] Settings { get; set; }

        public uint NumberOfUnits { get; set; }
        #endregion

        #region map
        public int MapWidth { get; set; }

        public int MapHeight { get; set; }

        public uint MapDepth { get; set; }

        public uint MapSize { get; set; }
        #endregion

        #region map movement
        ////public IntVector2 TargetPos { get; set; }

        public bool Recalculate { get; set; }
        #endregion

        #region map objects
        public ArrayList Models { get; set; }  // needed to draw all models

        public List<TDynamicModel> SelectedUnits { get; set; }

        public List<TDynamicModel> Units { get; set; }

        public List<TDynamicModel> WalkingUnits { get; set; }
        #endregion

        public void ResetHudSettings()
        {
            PriorityQueue[0] = true;
            PriorityQueue[1] = false;
            PriorityQueue[2] = false;

            MainAlgorithm[0] = true;
            MainAlgorithm[1] = false;
            MainAlgorithm[2] = false;
            MainAlgorithm[3] = false;
            MainAlgorithm[4] = false;
            MainAlgorithm[5] = false;
            MainAlgorithm[6] = false;
            MainAlgorithm[7] = false;

            DistanceMeasure[0] = true;
            DistanceMeasure[1] = false;
            DistanceMeasure[2] = false;
            DistanceMeasure[3] = false;

            // page 2
            Expansion[0] = true;
            Expansion[1] = false;

            Options[0] = false;

            // page 3
            Optimizations[0] = false;

            Settings[0] = false;
            Settings[1] = false;
        }
    }
}
