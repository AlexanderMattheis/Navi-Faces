using Navi.AI.Arrangement;
using Navi.Models;
using Navi.System.State;
using System.Collections;
using System.Collections.Generic;

namespace Navi.World
{
    /// <remarks>
    /// Some source code is taken from the Map class of Navi (2014/2015).
    /// </remarks>
    public sealed class Map
    {
        // game textures were made for the maximum resolution 2560 * 1440
        public const float PercentCellDimension = 0.0535f;  // 77 / 1440

        public const float PercentGapDistance = 1.0325f;  // some parameter that seems to look good

        private SaveGame<Static, Dynamic, Node> save;

        public Map(SaveGame<Static, Dynamic, Node> save)
        {
            this.save = save;
        }

        #region collision
        public bool[,] Collision
        {
            get { return save.Collision; }
            set { save.Collision = value; }
        }

        public Node[,] Nodes 
        {
            get { return save.MapNodes; }
            set { save.MapNodes = value; }
        }     
        #endregion

        #region general
        public int Width
        {
            get { return save.MapWidth; }
            set { save.MapWidth = value; }
        }

        public int Height
        {
            get { return save.MapHeight; }
            set { save.MapHeight = value; }
        }

        public uint Depth
        {
            get { return save.MapDepth; }
            set { save.MapDepth = value; }
        }

        public uint Size
        {
            get { return save.MapSize; }
            set { save.MapSize = value; }
        }

        public uint NumberOfUnits
        {
            get { return save.NumberOfUnits; }
            set { save.NumberOfUnits = value; }
        }
        #endregion

        #region objects
        public ArrayList Models
        {
            get { return save.Models; }
            set { save.Models = value; }
        }

        public List<Dynamic> Units
        {
            get { return save.Units; }
            set { save.Units = value; }
        }

        public List<Dynamic> SelectedUnits
        {
            get { return save.SelectedUnits; }
            set { save.SelectedUnits = value; }
        }

        public List<Dynamic> WalkingUnits
        {
            get { return save.WalkingUnits; }
            set { save.WalkingUnits = value; }
        }

        /*
        public QuadTree<Dynamic> RegionDevider
        {
            get { return save.RegionDevider; }
            set { save.RegionDevider = value; }
        }
        */
        #endregion
    }
}
