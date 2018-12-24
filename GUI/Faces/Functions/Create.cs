using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Navi.Audio.Player;
using Navi.Defaults;
using Navi.GUI.Faces.Functions.Subfunctions;
using Navi.GUI.Faces.Language;
using Navi.GUI.Faces.Parser;
using Navi.GUI.Widgets;
using Navi.GUI.Widgets.Groups;
using System.Collections.Generic;

using ObjectTypes = Navi.GUI.Faces.Language.Vocabulary.ObjectTypes;

namespace Navi.GUI.Faces.Functions
{
    public struct Create
    {
        private Compiler compiler;
        private CreateObjects createObjects;
        private CreateObjectGroups createArrays;

        public Create(Compiler compiler)
        {
            this.compiler = compiler;
            createObjects = new CreateObjects(compiler);
            createArrays = new CreateObjectGroups(compiler);
        }

        public enum ReturnValues
        {
            NoErrors = 0,
            Errors = 1,
            ContainsArrays = 2,
            ContainsVariables = 3
        }

        public void CreateObjects(List<string> commands, GraphicsDevice graphicsDevice, SoundPlayer soundPlayer, ContentManager content, ref int line)
        {
            line++;  // skip "Creation"-line
            WidgetGroup group = null;

            if (line < commands.Count)
            {
                Structs.Object objectData = compiler.ObjectAnalyzer.GetData(commands[line]);
                bool doneCreation = true;
                string objectName = string.Empty;

                while (Vocabulary.IsDataType(objectData.Type))
                {
                    int nameStartPos = 0;
                    group = WidgetGroupType.Create(objectData.Type);

                    if (objectData.Parameters.Length > 0)
                    {
                        while (nameStartPos < objectData.Names.Count && !Vocabulary.IsVariableType(objectData.Names[nameStartPos]))  // iterating through names
                        {
                            objectName = objectData.Names[nameStartPos];
                            doneCreation = CreateWidget(objectData, graphicsDevice, soundPlayer, content, objectName, group);
                            nameStartPos++;
                        }
                    }
                    else  // if we don't have any parameters
                    {
                        objectName = objectData.Names[nameStartPos];
                        doneCreation = CreateWidget(objectData, graphicsDevice, soundPlayer, content, objectName, group);
                    }

                    compiler.Debugger.Message(commands[line], doneCreation);

                    line++;
                    objectData = compiler.ObjectAnalyzer.GetData(commands[line]);
                }

                compiler.Debugger.Message(string.Empty);
            }
        }

        private bool CreateWidget(Structs.Object objectData, GraphicsDevice graphicsDevice, SoundPlayer soundPlayer, ContentManager content, string objectName, WidgetGroup group)
        {
            bool doneCreation = true;

            switch (objectData.Type)
            {
                case ObjectTypes.Button:        doneCreation = CreateButtons(objectData, graphicsDevice, soundPlayer, content, objectName);                         break;
                case ObjectTypes.Checkbox:      doneCreation = CreateCheckBoxes(objectData, graphicsDevice, soundPlayer, content, objectName);                      break;
                case ObjectTypes.IconButton:    doneCreation = CreateIconButtons(objectData, graphicsDevice, soundPlayer, content, objectName);                     break;
                case ObjectTypes.Image:         doneCreation = CreateImages(objectData, graphicsDevice, soundPlayer, content, objectName);                          break;
                case ObjectTypes.Label:         doneCreation = CreateLabels(objectData, graphicsDevice, soundPlayer, content, objectName);                          break;
                case ObjectTypes.RadioButton:   doneCreation = CreateRadioButtons(objectData, graphicsDevice, soundPlayer, content, objectName, (RadioGroup)group); break;
                default:                        return false; 
            }

            return doneCreation;
        }

        private bool CreateButtons(Structs.Object objectData, GraphicsDevice graphicsDevice, SoundPlayer soundPlayer, ContentManager content, string objectName)
        {
            VariableData data = new VariableData();

            ReturnValues doneCreation = createObjects.SetObjectParameters(objectData, objectName, data);

            TextButton button;
            if (data.ImagePath != null)
                button = new TextButton(content, graphicsDevice, soundPlayer, data.ImagePath, data.ShinyImagePath, data.FontPath)
                {
                    DegreeRotation = data.Rotation,
                    Effect = data.Effect,
                    HorizontalAlignment = data.TextAlignment,
                    Name = objectData.Type + objectName,
                    PercentScale = data.Scale,
                    PercentPivotPoint = data.Pivot,
                    PercentTextPivotPoint = data.TextPivot,
                    Text = data.Text,
                    TextColor = data.TextColor,
                    TextDegreeRotation = data.TextRotation,
                    TextEffect = data.TextEffect,
                    PercentTextScale = data.TextScale,
                    ReceivesMouseActions = data.ReceivesMouseActions,
                    SoundPath = data.SoundPath,
                    Tint = data.Tint
                };
            else
                button = new TextButton(content, graphicsDevice, soundPlayer, data.Color, data.HighlightColor, data.FontPath)
                {
                    DegreeRotation = data.Rotation,
                    Effect = data.Effect,
                    HorizontalAlignment = data.TextAlignment,
                    Name = objectData.Type + objectName,
                    PercentScale = data.Scale,
                    PercentPivotPoint = data.Pivot,
                    PercentTextPivotPoint = data.TextPivot,
                    Text = data.Text,
                    TextColor = data.TextColor,
                    TextDegreeRotation = data.TextRotation,
                    TextEffect = data.TextEffect,
                    PercentTextScale = data.TextScale,
                    ReceivesMouseActions = data.ReceivesMouseActions,
                    SoundPath = data.SoundPath,
                    Tint = Color.White
                };

            compiler.TextButtons.Add(objectName, button);
            return doneCreation != ReturnValues.Errors;
        }

        public bool CreateCheckBoxes(Structs.Object objectData, GraphicsDevice graphicsDevice, SoundPlayer soundPlayer, ContentManager content, string objectName)
        {
            VariableData data = new VariableData();

            ReturnValues doneCreation = createObjects.SetObjectParameters(objectData, objectName, data);

            CheckBox checkBox;
            if (data.StateImagePath != null)
                checkBox = new CheckBox(content, graphicsDevice, soundPlayer, data.ImagePath, data.StateImagePath)
                {
                    DegreeRotation = data.Rotation, 
                    Effect = data.Effect,
                    IsSet = data.IsSet,
                    Name = objectData.Type + objectName, 
                    PercentPivotPoint = data.Pivot,
                    PercentScale = data.Scale, 
                    ReceivesMouseActions = data.ReceivesMouseActions,
                    SoundPath = data.SoundPath,
                    Tint = data.Tint
                };
            else
            {
                checkBox = new CheckBox(content, graphicsDevice, soundPlayer, data.ImagePath, data.CheckedColor)
                {
                    DegreeRotation = data.Rotation,
                    Effect = data.Effect,
                    IsSet = data.IsSet,
                    Name = objectData.Type + objectName,
                    PercentPivotPoint = data.Pivot,
                    PercentScale = data.Scale,
                    ReceivesMouseActions = data.ReceivesMouseActions,
                    SoundPath = data.SoundPath,
                    Tint = data.Tint
                };
            }

            compiler.CheckBoxes.Add(objectName, checkBox);
            return doneCreation != ReturnValues.Errors;
        }

        private bool CreateIconButtons(Structs.Object objectData, GraphicsDevice graphicsDevice, SoundPlayer soundPlayer, ContentManager content, string objectName)
        {
            VariableData data = new VariableData();

            ReturnValues doneCreation = createObjects.SetObjectParameters(objectData, objectName, data);

            IconButton iconButton;
            if (data.ImagePath != null)
            {
                iconButton = new IconButton(content, graphicsDevice, soundPlayer, data.ImagePath, data.IconPath)
                {
                    DegreeRotation = data.Rotation,
                    Effect = data.Effect,
                    IconDegreeRotation = data.IconRotation,
                    IconEffect = data.IconEffect,
                    Name = objectData.Type + objectName, 
                    PercentIconPivot = data.IconPivot,
                    PercentPivotPoint = data.Pivot,
                    PercentScale = data.Scale,
                    ReceivesMouseActions = data.ReceivesMouseActions,
                    SoundPath = data.SoundPath,
                    Tint = data.Tint
                };
            }
            else
                iconButton = new IconButton(content, graphicsDevice, soundPlayer, data.Color, data.IconPath)
                {
                    DegreeRotation = data.Rotation,
                    Effect = data.Effect,
                    IconDegreeRotation = data.IconRotation,
                    IconEffect = data.IconEffect,
                    Name = objectData.Type + objectName,
                    PercentIconPivot = data.IconPivot,
                    PercentPivotPoint = data.Pivot,
                    PercentScale = data.Scale,
                    ReceivesMouseActions = data.ReceivesMouseActions,
                    SoundPath = data.SoundPath,
                    Tint = data.Tint
                };

            compiler.IconButtons.Add(objectName, iconButton);
            return doneCreation != ReturnValues.Errors;
        }

        private bool CreateImages(Structs.Object objectData, GraphicsDevice graphicsDevice, SoundPlayer soundPlayer, ContentManager content, string objectName)
        {
            VariableData data = new VariableData();

            ReturnValues doneCreation = createObjects.SetObjectParameters(objectData, objectName, data);

            Image image;

            if (data.ImagePath != null)
                image = new Image(content, graphicsDevice, soundPlayer, data.ImagePath) 
                { 
                    DegreeRotation = data.Rotation, 
                    Effect = data.Effect,
                    Name = objectData.Type + objectName,
                    PercentPivotPoint = data.Pivot, 
                    PercentScale = data.Scale,
                    ReceivesMouseActions = data.ReceivesMouseActions,
                    SoundPath = data.SoundPath,
                    Tint = data.Tint 
                };
            else
                image = new Image(data.Color, graphicsDevice, soundPlayer) 
                { 
                    DegreeRotation = data.Rotation, 
                    Effect = data.Effect,
                    Name = objectData.Type + objectName,
                    PercentPivotPoint = data.Pivot, 
                    PercentScale = data.Scale,
                    ReceivesMouseActions = data.ReceivesMouseActions,
                    SoundPath = data.SoundPath,
                    Tint = Color.White 
                };

            compiler.Images.Add(objectName, image);
            return doneCreation != ReturnValues.Errors;
        }

        private bool CreateLabels(Structs.Object objectData, GraphicsDevice graphicsDevice, SoundPlayer soundPlayer, ContentManager content, string objectName)
        {
            VariableData data = new VariableData();

            ReturnValues doneCreation = createObjects.SetObjectParameters(objectData, objectName, data);

            if (doneCreation == ReturnValues.NoErrors) AddLabelToList(objectData, data, graphicsDevice, soundPlayer, content, objectName);
            else if (doneCreation != ReturnValues.Errors)
            {
                doneCreation = createArrays.SetObjectParameters(objectData, objectName, data);
                AddLabelsToList(objectData, data, graphicsDevice, soundPlayer, content, objectName);
            }

            return doneCreation != ReturnValues.Errors;
        }

        private void AddLabelToList(Structs.Object objectData, VariableData data, GraphicsDevice graphicsDevice, SoundPlayer soundPlayer, ContentManager content, string objectName)
        {
            Label label;
            if (data.ImagePath != null)
                label = new Label(content, graphicsDevice, soundPlayer, data.ImagePath, data.FontPath)
                {
                    DegreeRotation = data.Rotation,
                    Effect = data.Effect,
                    HorizontalAlignment = data.TextAlignment,
                    Name = objectData.Type + objectName,
                    PercentPivotPoint = data.Pivot,
                    PercentScale = data.Scale,
                    PercentTextPivotPoint = data.TextPivot,
                    Text = data.Text,
                    TextColor = data.TextColor,
                    TextDegreeRotation = data.TextRotation,
                    TextEffect = data.TextEffect,
                    PercentTextScale = data.TextScale,
                    ReceivesMouseActions = data.ReceivesMouseActions,
                    SoundPath = data.SoundPath,
                    Tint = data.Tint
                };
            else if (data.Color != Color.Transparent)
                label = new Label(content, graphicsDevice, soundPlayer, data.Color, data.FontPath)
                {
                    DegreeRotation = data.Rotation,
                    Effect = data.Effect,
                    HorizontalAlignment = data.TextAlignment,
                    Name = objectData.Type + objectName,
                    PercentPivotPoint = data.Pivot,
                    PercentScale = data.Scale,
                    PercentTextPivotPoint = data.TextPivot,
                    Text = data.Text,
                    TextColor = data.TextColor,
                    TextDegreeRotation = data.TextRotation,
                    TextEffect = data.TextEffect,
                    PercentTextScale = data.TextScale,
                    ReceivesMouseActions = data.ReceivesMouseActions,
                    SoundPath = data.SoundPath,
                    Tint = Color.White
                };
            else
            {
                label = new Label(content, data.FontPath)
                {
                    DegreeRotation = data.Rotation,
                    Effect = data.Effect,
                    HorizontalAlignment = data.TextAlignment,
                    Name = objectData.Type + objectName,
                    PercentPivotPoint = data.Pivot,
                    PercentScale = data.Scale,
                    PercentTextPivotPoint = data.TextPivot,
                    Text = data.Text,
                    TextColor = data.TextColor,
                    TextDegreeRotation = data.TextRotation,
                    TextEffect = data.TextEffect,
                    PercentTextScale = data.TextScale,
                    ReceivesMouseActions = data.ReceivesMouseActions,
                    SoundPath = data.SoundPath,
                    Tint = Color.White
                };
            }

            compiler.Labels.Add(objectName, label);
        }

        private void AddLabelsToList(Structs.Object objectData, VariableData data, GraphicsDevice graphicsDevice, SoundPlayer soundPlayer, ContentManager content, string objectName)
        {
            int textCounter = 0;  // needed for cyclical group iteration

            for (int i = 0; i < createArrays.Biggest; i++)
            {
                // manipulate data
                if (textCounter < data.TextGroup.Count) data.Text = data.TextGroup[textCounter];
                else textCounter = 0;

                //// other variables for groups also have to be modified here:
                //// if (colorCounter < data.ColorGroup.Count) data.Color = data.ColorGroup[colorCounter];
                //// else colorCounter = 0;
                //// ...

                textCounter++;
                AddLabelToList(objectData, data, graphicsDevice, soundPlayer, content, objectName + i);
            }
        }

        private bool CreateRadioButtons(Structs.Object objectData, GraphicsDevice graphicsDevice, SoundPlayer soundPlayer, ContentManager content, string objectName, RadioGroup group)
        {
            VariableData data = new VariableData();

            ReturnValues doneCreation = createObjects.SetObjectParameters(objectData, objectName, data);

            RadioButton radioButton;

            if (data.StateImagePath != null)
                radioButton = new RadioButton(content, graphicsDevice, soundPlayer, data.ImagePath, data.StateImagePath)
                {
                    DegreeRotation = data.Rotation,
                    Effect = data.Effect,
                    IsSet = data.IsSet,
                    Name = objectData.Type + objectName,
                    PercentPivotPoint = data.Pivot,
                    PercentScale = data.Scale,
                    ReceivesMouseActions = data.ReceivesMouseActions,
                    SoundPath = data.SoundPath,
                    Tint = data.Tint
                };
            else
            {
                radioButton = new RadioButton(content, graphicsDevice, soundPlayer, data.ImagePath, data.CheckedColor)
                {
                    DegreeRotation = data.Rotation,
                    Effect = data.Effect,
                    IsSet = data.IsSet,
                    Name = objectData.Type + objectName,
                    PercentPivotPoint = data.Pivot,
                    PercentScale = data.Scale,
                    ReceivesMouseActions = data.ReceivesMouseActions,
                    SoundPath = data.SoundPath,
                    Tint = data.Tint
                };
            }

            AddRadioButtons(objectName, radioButton, objectData, group);
            return doneCreation != ReturnValues.Errors;
        }

        private void AddRadioButtons(string objectName, RadioButton radioButton, Structs.Object objectData, RadioGroup group)
        {
            compiler.RadioButtons.Add(objectName, radioButton);
            group.Add(radioButton);

            if (objectData.Names.Count == group.Count)  // so if all elements placed in the group
            {
                string radioGroupName = string.Join(string.Empty, objectData.Names.ToArray());
                compiler.RadioGroup.Add(radioGroupName, group);
            }
        }

        public class VariableData
        {
            /// <remarks>
            /// All numerical values in percent.
            /// </remarks>
            public VariableData()
            {
                #region arrays
                TextGroup = new List<string>();
                #endregion

                #region variables
                CheckedColor = Color.Black;
                Color = Color.Transparent;
                Effect = SpriteEffects.None;
                FontPath = Paths.Fonts + "navigator";
                HighlightColor = Color.White;
                IconEffect = SpriteEffects.None;
                IconPath = null;
                IconPivot = new Vector2(50.0f, 50.0f);
                IconRotation = 0.0f;
                ImagePath = null;
                IsActive = true;
                IsSet = false;
                Pivot = new Vector2(50.0f, 50.0f);
                ReceivesMouseActions = true;
                Rotation = 0.0f;
                Scale = 100.0f;
                ShinyImagePath = null;
                SoundPath = null;
                StateImagePath = null;
                Text = string.Empty;
                TextAlignment = Label.HorizontalAlignments.Center;
                TextColor = Color.Black;
                TextEffect = SpriteEffects.None;
                TextPivot = new Vector2(50.0f, 50.0f);
                TextRotation = 0.0f;
                TextScale = 100.0f;
                Tint = Color.White;
                #endregion
            }

            #region arrays
            public List<string> TextGroup { get; set; }
            #endregion

            #region variables
            public Color Color { get; set; }

            public Color CheckedColor { get; set; }

            public SpriteEffects Effect { get; set; }

            public string FontPath { get; set; }

            public Color HighlightColor { get; set; }

            public SpriteEffects IconEffect { get; set; }

            public string IconPath { get; set; }

            public Vector2 IconPivot { get; set; } 

            public float IconRotation { get; set; } 

            public string ImagePath { get; set; }

            public bool IsActive { get; set; }

            public bool IsSet { get; set; }

            public Vector2 Pivot { get; set; }

            public bool ReceivesMouseActions { get; set; }

            public float Rotation { get; set; }

            public float Scale { get; set; }

            public string ShinyImagePath { get; set; }

            public string SoundPath { get; set; }

            public string StateImagePath { get; set; }

            public string Text { get; set; }

            public Label.HorizontalAlignments TextAlignment { get; set; }

            public Color TextColor { get; set; }

            public SpriteEffects TextEffect { get; set; }

            public Vector2 TextPivot { get; set; }

            public float TextRotation { get; set; }

            public float TextScale { get; set; }

            public Color Tint { get; set; }
            #endregion
        }
    }
}
