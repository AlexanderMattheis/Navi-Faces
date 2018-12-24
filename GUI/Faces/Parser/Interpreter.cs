using Microsoft.Xna.Framework.Graphics;
using Navi.Audio.Player;
using Navi.Content.Faces.HUD;
using Navi.Defaults;
using Navi.GUI.Faces.Language;
using Navi.GUI.Widgets.Groups;
using Navi.Screen;
using Navi.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

using Commands = Navi.GUI.Faces.Language.Vocabulary.Commands;
using Symbols = Navi.GUI.Faces.Language.Vocabulary.Symbols;

namespace Navi.GUI.Faces.Parser
{
    public sealed class Interpreter
    {
        private ContentManagement content;
        private GraphicsDevice graphicsDevice;

        private SoundPlayer soundPlayer;

        private Compiler compiler;
        private SurfaceManager surfaceManager;

        private List<object> faceLogics;
        private List<Tuple<Interface, SurfaceWidget, string>> openTasks;

        public Interpreter(ContentManagement content, GraphicsDevice graphicsDevice, SoundPlayer soundPlayer, SurfaceManager surfaceManager, Compiler compiler)
        {
            this.compiler = compiler;
            this.content = content;
            this.surfaceManager = surfaceManager;
            this.graphicsDevice = graphicsDevice;
            this.soundPlayer = soundPlayer;

            faceLogics = new List<object>();
            openTasks = new List<Tuple<Interface, SurfaceWidget, string>>();
        }

        #region interpret
        public object Interpret(ref string text, string objectName, WidgetGroup group, SurfaceWidget widget, Interface face, Surface parent)
        {
            if (text.Contains(Symbols.Command))
            {
                if (text.Contains(Vocabulary.Commands.NewLine)) AddNewLines(ref text);
                else return EvaluateCommands(ref text, objectName, group, widget, face, parent);
            }
            else
                CreateSurface(parent, widget, text);

            return null;
        }

        private void AddNewLines(ref string text)
        {
            string[] segments = text.Replace(Symbols.Command + Vocabulary.Commands.NewLine, Symbols.Command).Split(Symbols.Command[0]);

            text = string.Empty;
            for (int i = 0; i < segments.Length; i++)
                text += segments[i].Trim() + (i < segments.Length - 1 ? "\n" : string.Empty);
        }

        private object EvaluateCommands(ref string text, string objectName, WidgetGroup group, SurfaceWidget widget, Interface face, Surface parent)
        {
            object output = null;

            string[] segments = text.Split(Symbols.Command[0]);  // #

            foreach (string command in segments)
            {
                string[] parameterSplit = command.Split(Symbols.Equal[0]);  // example: #fx=Tasks
                string commandName = parameterSplit[0];
                string parameterName = parameterSplit.Length == 2 ? parameterSplit[1] : string.Empty;

                switch (commandName)
                {
                    case Vocabulary.Commands.Align:             return Align(parameterName);
                    case Vocabulary.Commands.Back:              Back(widget, parent);                       break;
                    case Vocabulary.Commands.Big:               output = Big(ref text, output);             break;
                    case Vocabulary.Commands.Color:             return Color(parameterName);
                    case Vocabulary.Commands.Database:          Translate(ref text, parameterName);         break;
                    case Vocabulary.Commands.Effect:            return Effect(parameterName);
                    case Vocabulary.Commands.Exit:              Exit(widget);                               break;
                    case Vocabulary.Commands.Font:              return Font(parameterName);
                    case Vocabulary.Commands.Function:          Function(face, widget, parameterName);      break;
                    case Vocabulary.Commands.GroupFunction:     GroupFunction(face, group, parameterName);  break;
                    case Vocabulary.Commands.Image:             return Image(parameterName);
                    case Vocabulary.Commands.Input:             output = Input(face, parameterName);        break;
                    case Vocabulary.Commands.Pivot:             return Pivot(parameterName);
                    case Vocabulary.Commands.Post:              Post(ref text, objectName, widget, parent); break;
                    case Vocabulary.Commands.Rotation:          return Rotation(parameterName);
                    case Vocabulary.Commands.Scale:             return Scale(parameterName);
                    case Vocabulary.Commands.Sound:             return Sound(parameterName);
                    case Vocabulary.Commands.Split:             output = Split(ref text, output);           break;
                    case Vocabulary.Commands.State:             return State(parameterName);
                }
            }

            return output;
        }

        private object Align(string name)
        {
            return Compiler.Databases.AlignmentBase.TextValue(name);
        }

        private void Back(SurfaceWidget widget, Surface parent)
        {
            widget.OnClick += new Methods(surfaceManager, parent).Back;
        }

        private object Big(ref string text, object list)
        {
            text = text.ToUpper().Trim();
            if (Regex.IsMatch(text, Commands.NewLine, RegexOptions.IgnoreCase))
                text = text.Replace(Commands.NewLine.ToUpper(), Commands.NewLine);

            if (list != null && list.GetType() == new List<string>().GetType())
            {
                List<string> texts = (List<string>)list;
                return (from value in texts select value.ToUpper()).ToList<string>();
            }

            return list;
        }

        private object Color(string name)
        {
            return Compiler.Databases.ColorBase.TextValue(name);
        }

        private object Effect(string name)
        {
            return Compiler.Databases.EffectBase.TextValue(name);
        }

        private void Exit(SurfaceWidget widget)
        {
            widget.OnClick += new Methods(surfaceManager).Exit;
        }

        private string Font(string name)
        {
            return Compiler.Databases.FontBase.TextValue(name);
        }

        private void Function(Interface face, SurfaceWidget widget, string function)
        {
            openTasks.Add(new Tuple<Interface, SurfaceWidget, string>(face, widget, function));
        }

        private void GroupFunction(Interface face, WidgetGroup group, string function)
        {
            object surfaceLogic = SelectLogic(face);

            MethodInfo method = surfaceLogic.GetType().GetMethod(function);
            if (!group.GroupActionIsSet) group.OnChange += (WidgetGroup.Action)Delegate.CreateDelegate(typeof(WidgetGroup.Action), surfaceLogic, method);
        }

        private object Image(string name)
        {
            return Compiler.Databases.ImageBase.TextValue(name);
        }

        private object Input(Interface face, string function)
        {
            string className = Paths.FacesNamesSpace + face.Name.Replace(Paths.XnaDirectorySymbols, Paths.Namespace);

            object surfaceLogic = SelectLogic(face);
            MethodInfo method = surfaceLogic.GetType().GetMethod(function);

            object data = method.Invoke(surfaceLogic, null);
            
            if (data.GetType() == new List<string>().GetType())
                return Translate(data);
            return data;
        }

        private List<string> Translate(object data)
        {
            List<string> texts = (List<string>)data;
            List<string> translatedTexts = new List<string>();

            foreach (string text in texts)
            {
                string translated = text;
                Translate(ref translated, null);
                translatedTexts.Add(translated);
            }

            return translatedTexts;
        }

        private void Translate(ref string text, string parameterName)
        {
            if (parameterName != null && text.Contains(Symbols.Command + Commands.Database)) text = parameterName;
            string newText = Compiler.Databases.LanguageBase.TextValue(text.Trim());

            if (newText != null)
                text = newText;
        }

        private object SelectLogic(Interface face)
        {
            string className = Paths.FacesNamesSpace + face.Name.Replace(Paths.XnaDirectorySymbols, Paths.Namespace);

            Type type = Type.GetType(className);
            object[] arguments = new object[] { content, graphicsDevice, soundPlayer, surfaceManager, face };
            object faceLogic = Activator.CreateInstance(type, arguments);

            // checks if the type of logic is already in use and if it's in use then it is selected instead of the new instance that has been created
            // this way all buttons of a face are connected with the same face logic
            bool contained = faceLogics.Any(obj => obj.GetType().ToString() == type.ToString());
            if (!contained) faceLogics.Add(faceLogic);
            else faceLogic = (from logic in faceLogics where logic.GetType().ToString() == type.ToString() select logic).ToArray()[0];

            return faceLogic;
        }

        private object Pivot(string name)
        {
            return Compiler.Databases.PivotBase.TextValue(name);
        }

        private void Post(ref string text, string variableName, SurfaceWidget widget, Surface surface)
        {
            text = variableName.Trim();
            CreateSurface(surface, widget, text); 
        }

        private void CreateSurface(Surface surface, SurfaceWidget widget, string text)
        {
            if (widget != null)
            {
                // it is not allowed to change the Push()-method during the runtime after it's assigned so we have to create everytime a new object
                Methods methods = new Methods(surfaceManager);
                methods.NextInterface = new Interface(content, graphicsDevice, soundPlayer, surfaceManager, surface.PixelResolution, text, surface).Instance();
                widget.OnClick += methods.Push;
            }
        }

        private object Rotation(string name)
        {
            return Compiler.Databases.RotationBase.TextValue(name);
        }

        private object Scale(string name)
        {
            return Compiler.Databases.ScaleBase.TextValue(name);
        }

        private object Sound(string name)
        {
            return Compiler.Databases.SoundBase.TextValue(name);
        }

        private object State(string name)
        {
            return Compiler.Databases.StateBase.TextValue(name);
        }

        private object Split(ref string text, object list)
        {
            text = PascalSplit(text);

            if (list != null && list.GetType() == new List<string>().GetType())
            {
                List<string> texts = (List<string>)list;
                List<string> textsWithSpace = new List<string>();

                foreach (string spaceless in texts)
                    textsWithSpace.Add(PascalSplit(spaceless));

                return textsWithSpace;
            }

            return list;
        }

        private string PascalSplit(string text)
        {
            string[] textSegments = Regex.Split(text, Patterns.CapitalLetters);  // split Pascal Case

            text = string.Empty;

            for (int i = 0; i < textSegments.Length; i++)
            {
                bool lastElement = i == textSegments.Length - 1;
                text += textSegments[i] + (!lastElement ? " " : string.Empty);
            }

            return text;
        }

        #endregion

        #region complete
        /// <remarks>
        /// Has to be called when the interpreter is no longer needed. I is used to finish open tasks.
        /// </remarks>
        public void Complete()
        {
            LinkWidgets();
        }

        /// <remarks>
        /// Linking widgets can only be done after all widgets has been added to the surface.
        /// </remarks>
        private void LinkWidgets()
        {
            foreach (Tuple<Interface, SurfaceWidget, string> triple in openTasks)
            {
                object surfaceLogic = SelectLogic(triple.Item1);

                MethodInfo method = surfaceLogic.GetType().GetMethod(triple.Item3);
                triple.Item2.OnClick += (SurfaceWidget.Action)Delegate.CreateDelegate(typeof(SurfaceWidget.Action), surfaceLogic, method);
            }
        }
        #endregion
    }
}
