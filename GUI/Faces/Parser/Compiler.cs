using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Navi.Audio.Player;
using Navi.Defaults;
using Navi.GUI.Faces.Debugging;
using Navi.GUI.Faces.Functions;
using Navi.GUI.Faces.Structs;
using Navi.GUI.Widgets;
using Navi.GUI.Widgets.Groups;
using Navi.Helper.Structures;
using Navi.Programming;
using Navi.Screen;
using Navi.System;
using Navi.System.State;
using System.Collections.Generic;

using ObjectTypes = Navi.GUI.Faces.Language.Vocabulary.ObjectTypes;
using State = Navi.GUI.Faces.Datatypes.State;
using Symbols = Navi.GUI.Faces.Language.Vocabulary.Symbols;

namespace Navi.GUI.Faces.Parser
{
    public sealed class Compiler
    {
        private ContentManagement content;
        private GraphicsDevice graphicsDevice;

        private SoundPlayer soundPlayer;
        private SurfaceManager surfaceManager;

        public Compiler(ContentManagement content, GraphicsDevice graphicsDevice, SoundPlayer soundPlayer, SurfaceManager surfaceManager, Interface face, bool showName)
        {
            InitFunctions();
            InitGroups();
            InitStructs();
            InitTools(content, graphicsDevice, soundPlayer, surfaceManager, face, showName);
            InitializeVariables();
            InitWidgetGroup();
            InitWidgets();


            this.surfaceManager = surfaceManager;
            this.graphicsDevice = graphicsDevice;
            this.soundPlayer = soundPlayer;
            this.content = content;
            if (Databases == null) Databases = new DatabasePackage();
            Face = face;
        }

        internal static DatabasePackage Databases { get; private set; }

        public Interface Face { get; private set; }

        #region functions
        public Create Creation { get; set; }

        public Define Definition { get; set; }

        public Link Linking { get; set; }

        public Add Adding { get; set; }
        #endregion

        #region groups
        public Dictionary<string, List<string>> TextArrays { get; set; }
        #endregion

        #region structs
        public Instructions Instruction { get; set; }

        public Structs.Object ObjectAnalyzer { get; set; }

        public Variable VariableAnalyzer { get; set; }
        #endregion

        #region tools 
        public Debugger Debugger { get; private set; }

        public Interpreter Interpreter { get; private set; }  // avoids loading again and again the same stuff from harddrive
        #endregion

        #region variables
        public Dictionary<string, Color> CheckedColorVariables { get; set; }

        public Dictionary<string, Color> ColorVariables { get; set; }

        public Dictionary<string, SpriteEffects> EffectVariables { get; set; }

        public Dictionary<string, string> FontVariables { get; set; }

        public Dictionary<string, Color> HighlightColorVariables { get; set; }

        public Dictionary<string, string> IconVariables { get; set; }

        public Dictionary<string, SpriteEffects> IconEffectVariables { get; set; }

        public Dictionary<string, Vector2> IconPivotVariables { get; set; }

        public Dictionary<string, float> IconRotationVariables { get; set; }

        public Dictionary<string, string> ImageVariables { get; set; }

        public Dictionary<string, Vector2> PivotVariables { get; set; }

        public Dictionary<string, float> RotationVariables { get; set; }

        public Dictionary<string, float> ScaleVariables { get; set; }

        public Dictionary<string, string> ShinyImageVariables { get; set; }

        public Dictionary<string, string> SoundVariables { get; set; }

        public Dictionary<string, string> StateImageVariables { get; set; }

        public Dictionary<string, State> StateVariables { get; set; }

        public Dictionary<string, Label.HorizontalAlignments> TextAligmentVariables { get; set; }

        public Dictionary<string, Color> TextColorVariables { get; set; }

        public Dictionary<string, SpriteEffects> TextEffectVariables { get; set; }

        public Dictionary<string, float> TextScaleVariables { get; set; }

        public Dictionary<string, Vector2> TextPivotVariables { get; set; }

        public Dictionary<string, float> TextRotationVariables { get; set; }

        public Dictionary<string, string> TextVariables { get; set; }
        #endregion

        #region widgets
        public Dictionary<string, CheckBox> CheckBoxes { get; set; }

        public Dictionary<string, IconButton> IconButtons { get; set; }

        public Dictionary<string, Image> Images { get; set; }

        public Dictionary<string, RadioButton> RadioButtons { get; set; }

        public Dictionary<string, TextButton> TextButtons { get; set; }

        public Dictionary<string, Label> Labels { get; set; }
        #endregion

        #region widget group
        public Dictionary<string, RadioGroup> RadioGroup { get; set; } 
        #endregion

        #region [loader]
        private void InitFunctions()
        {
            Definition = new Define(this);
            Creation = new Create(this);
            Linking = new Link(this);
            Adding = new Add(this);
        }

        private void InitGroups()
        {
            TextArrays = new Dictionary<string, List<string>>();
        }

        private void InitStructs()
        {
            Instruction = new Instructions(this);
            ObjectAnalyzer = new Structs.Object();
            VariableAnalyzer = new Variable();
        }

        private void InitTools(ContentManagement content, GraphicsDevice graphicsDevice, SoundPlayer soundPlayer, SurfaceManager surfaceManager, Surface surface, bool showName)
        {
            Debugger = new Debugger(surface.Name, showName, Settings.DebugMode);
            Interpreter = new Interpreter(content, graphicsDevice, soundPlayer, surfaceManager, this);
        }

        private void InitializeVariables()
        {
            CheckedColorVariables = new Dictionary<string, Color>();
            ColorVariables = new Dictionary<string, Color>();
            EffectVariables = new Dictionary<string, SpriteEffects>();
            FontVariables = new Dictionary<string, string>();
            HighlightColorVariables = new Dictionary<string, Color>();
            IconVariables = new Dictionary<string, string>();
            IconEffectVariables = new Dictionary<string, SpriteEffects>();
            IconPivotVariables = new Dictionary<string, Vector2>();
            IconRotationVariables = new Dictionary<string, float>();
            ImageVariables = new Dictionary<string, string>();
            PivotVariables = new Dictionary<string, Vector2>();
            RotationVariables = new Dictionary<string, float>();
            ScaleVariables = new Dictionary<string, float>();
            ShinyImageVariables = new Dictionary<string, string>();
            SoundVariables = new Dictionary<string, string>();
            StateImageVariables = new Dictionary<string, string>();
            StateVariables = new Dictionary<string, State>();
            TextAligmentVariables = new Dictionary<string, Label.HorizontalAlignments>();
            TextColorVariables = new Dictionary<string, Color>();
            TextEffectVariables = new Dictionary<string, SpriteEffects>();
            TextScaleVariables = new Dictionary<string, float>();
            TextPivotVariables = new Dictionary<string, Vector2>();
            TextRotationVariables = new Dictionary<string, float>();
            TextVariables = new Dictionary<string, string>();
        }

        private void InitWidgetGroup()
        {
            RadioGroup = new Dictionary<string, RadioGroup>();
        }

        private void InitWidgets()
        {
            CheckBoxes = new Dictionary<string, CheckBox>();
            IconButtons = new Dictionary<string, IconButton>();
            Images = new Dictionary<string, Image>();
            Labels = new Dictionary<string, Label>();
            RadioButtons = new Dictionary<string, RadioButton>();
            TextButtons = new Dictionary<string, TextButton>();
        }
        #endregion

        #region [loner]
        public SurfaceWidget GetWidget(string type, string name)
        {
            switch (type)
            {
                case ObjectTypes.Button:        return TextButtons.GetValue(name);
                case ObjectTypes.Checkbox:      return CheckBoxes.GetValue(name);
                case ObjectTypes.IconButton:    return IconButtons.GetValue(name);
                case ObjectTypes.Image:         return Images.GetValue(name);
                case ObjectTypes.Label:         return Labels.GetValue(name);
                case ObjectTypes.RadioButton:   return RadioButtons.GetValue(name);
            }

            return null;
        }
        #endregion

        public Interface Create()
        {
            string surfacePath = Paths.Surfaces + Face.Name + FileExtensions.Interface;
            string comment = Symbols.Comment;
            char escape = Symbols.Escape[0];
            char delimeter = Symbols.Delimeter[0];
            char lineTerminal = Symbols.FunctionMarker[0];
            return Execute(new ScriptReader().Commands(surfacePath, comment, escape, delimeter, lineTerminal));
        }

        private Interface Execute(List<string> commands)
        {
            Face.AspectRatio = Face.PixelResolution.X / Face.PixelResolution.Y;

            int line = 0;
            if (commands.Count > 0) Interpret(commands, ref line);
            return Face;
        }

        private void Interpret(List<string> commands, ref int line)
        {
            Instruction.Import(commands, content, graphicsDevice, soundPlayer, surfaceManager, ref line);
            Instruction.SetBounds(commands, ref line);
            Debugger.Message(string.Empty);

            Definition.ReadIn(commands, ref line);
            Creation.CreateObjects(commands, graphicsDevice, soundPlayer, content.Menu, ref line);
            Linking.SetLinks(commands, Face, ref line);
            Adding.AddObjects(commands, Face, ref line);

            Interpreter.Complete();
            Debugger.Message(Debugger.End);
        }
    }
}
