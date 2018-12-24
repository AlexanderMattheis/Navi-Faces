using Navi.Models;

namespace Navi.AI.Arrangement
{
    public sealed class Node
    {
        public bool IsOccupied { get; set; }

        public Dynamic Occluder { get; set; }
    }
}
