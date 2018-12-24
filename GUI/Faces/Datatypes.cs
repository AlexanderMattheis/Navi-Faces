namespace Navi.GUI.Faces
{
    public struct Datatypes
    {
        public struct State
        {
            public const string Inactive = "Inactive";
            public const string NoMouseActions = "NoMouseActions";
            public const string Set = "Set";

            public bool IsActive { get; set; }

            public bool IsSet { get; set; }

            public bool ReceivesMouseActions { get; set; }
        }
    }
}
