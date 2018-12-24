using System;

namespace Navi.Defaults
{
    public struct Exceptions
    {
        public const string NoFileFormatExtension = "You have to set the file format extension!";
        public const string NoOnChangeAction = "You have to define an OnChange-action for every group of widgets!";
        public const string VolumeNotSet = "You have to set the volume!";

        [Serializable]
        public class NoFileFormatExtensionException : Exception
        {
            public NoFileFormatExtensionException() : base(Exceptions.NoFileFormatExtension) { }
        }

        [Serializable]
        public class NoOnChangeActionException : Exception
        {
            public NoOnChangeActionException() : base(Exceptions.NoOnChangeAction) { }
        }

        [Serializable]
        public class VolumeNotSetException : Exception
        {
            public VolumeNotSetException() : base(Exceptions.VolumeNotSet) { }
        }
    }
}
