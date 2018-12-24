namespace Navi.GUI.Faces.Language
{
    public struct Patterns
    {
        public const string ArrayInitilization = @"^[a-zA-Z]+\s*\{\s*\}\s*(<\s*[0-9]+\s*>)?\s*=\s*[\s.0-9,a-z#\=A-Z\(\)]*$";
        public const string LinkCreation = @"^[a-zA-Z0-9]*\s*(\[\s*[a-zA-Z0-9]*\s*\])?\s*->\s*[a-zA-Z0-9#]*(\s*=\s*[A-Za-z0-9]*)?$";
        public const string MultipleSurfaceAddition = @"^(\(\s*[a-zA-Z]+\{([a-zA-Z0-9]+)*(\s*,\s*([a-zA-Z0-9]*))*\}\s*"
                                                    + @"(,\s*[0-9]*\.[0-9]+\s*(\[\s*[\+\-]\s*[0-9]*\.[0-9]+\s*\])?){2}\))|\(\s*"
                                                    + @"[a-zA-Z]+\{([a-zA-Z0-9]+)*(\s*,\s*([a-zA-Z0-9]+))*\}\s*(,\s*[0-9]*\.[0-9]+\s*(\[\s*[\+\-]\s*[0-9]*\.[0-9]+\s*\])?){0,4}\)$";

        public const string MultipleLinkCreation =  @"^[a-zA-Z]+\s*\{\s*([a-zA-Z0-9]*\s*(\[\s*[a-zA-Z0-9]*\s*\])?)(\s*,\s*[a-" +
                                                    @"zA-Z0-9]+\s*(\[\s*[a-zA-Z0-9]*\s*\])?)*\s*\}\s*->\s*[a-zA-Z0-9#]*(\s*=\" +
                                                    @"s*[A-Za-z0-9]*)?$";

        public const string MultipleObjectInstantion =  @"^[a-zA-Z]+\s*\{\s*([a-zA-Z0-9]*(\s*,\s*[a-zA-Z0-9]*\s*)*)\}\s*\(" + 
                                                        @"(\s*[a-zA-Z]+\s*(<\s*[0-9]+\s*>)?\s*)(\s*,\s*[a-zA-Z]+\s*(<\s*[0-9]+\s*>)?\s*)*\)$";

        public const string ObjectInstantion = @"^[a-zA-Z0-9]*\s*(\{\s*\})?\s*(\(\s*[a-zA-Z]+\s*(\{\s*\})?\s*(<[0-9]*>)?(\s*,\s*[a-zA-Z]+\s*(\{\s*\})?\s*(<[0-9]*>)?)*\))?$";
        public const string PartialSurfaceAddition = @"^\(\s*[a-zA-Z]+[0-9]*(\s*,\s*(x|y|w|h)\s*=\s*[0-9]*\.[0-9]+\s*){0,4}\)$";
        public const string Positioning = @"^\(\s*[a-zA-Z]+[0-9]*\s*(,\s*[0-9]*\.[0-9]+\s*){2}\)$";
        public const string SurfaceAddition = @"^\(\s*[a-zA-Z0-9]+\s*(,\s*[0-9]*\.[0-9]+\s*){1,4}\)$";
        public const string VariableInitialization = @"^[a-zA-Z]+\s*(\{\s*\})?\s*(<\s*[0-9]+\s*>)?\s*=\s*[\s.0-9,a-z#A-Z\(\)]$*";
        public const string CapitalLetters = @"(?=[A-Z])";

        public struct KeyWords
        {
            public const string Import = @"^import\s*\(\s*([a-zA-Z]+\s*)(.\s*[a-zA-Z]+)*\s*\)$";
            public const string ImportWithParameters = @"^import\s*\(\s*([a-zA-Z]+\s*)(.\s*[a-zA-Z]+)*\s*\)" +
                @"\{\s*([a-zA-Z]+\s*(<\s*[0-9]+\s*>)?)\s*" +
                @"=\s*[\s\.0-9a-z#=A-Z\(\)]+\s*(\s*,\s*([a-zA-Z]+\s*(<\s*[0-9]+\s*>)?)\s*=\s*[\s\.0-9a-z#=A-Z\(\)]+\s*)*\}$";
        }

        public struct Variables
        {
            #region full patterns
            // patterns that contain the left part and the middle part
            public const string Alignment = @"^alignment\s*=\s*(Left|Center|Right)$";
            public const string Dimensions = @"^dimensions\s*=\s*\((\s*[0-9.]+\s*,){3}\s*[0-9.]+\s*\)$";
            #endregion

            #region partial patterns
            // patterns that contain only the right part
            public const string Color = @"^(\(\s*(((2[0-5][0-5])|(2[0-4][0-9])|(1[0-9]{2})|([1-9]?[0-9]))\s*,\s*){3}"
                                    + @"((2[0-5][0-5])|(2[0-4][0-9])|(1[0-9]{2})|([1-9]?[0-9]))\s*\))$";

            public const string Effect = @"^Mirror\s*\.\s*(X|Y)$";
            public const string Path = @"^([a-zA-Z]+\s*)(.\s*[a-zA-Z]+)*$";
            public const string Pivot = @"^\(\s*[0-9]*.[0-9]+\s*,\s*[0-9]*.[0-9]+\s*\)$";
            public const string Rotation = @"^([0-9]|([1-9][0-9])|([1-2][0-9]{2})|([3][0-5][0-9]))$";
            public const string Scale = @"^[0-9]*.[0-9]+$";
            public const string State = @"^(Checked|Inactive|NoMouseActions)(\s*&\s*(Checked|Unactive|NoMouseActions))*$";
            public const string Text = @"^[a-zA-ZäöüÄÖÜß,.!?:;\\0-9²³°=+\-^§$%&\/\(\)\[\]<>`´'~*#""\s]*$";
            public const string TextAligment = @"^(Left|Center|Right)$";
            #endregion
        }
    }
}
