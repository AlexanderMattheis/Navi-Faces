using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Navi.AI.Arrangement;
using Navi.Audio.Player;
using Navi.Models;
using Navi.System.State;
using System;
using System.IO;

using NaviModel = Navi.Models.Model;

namespace Navi.World.Tools
{
    /// <remarks>
    /// Is based on the MapLoader class of Navi (2014/2015).
    /// </remarks>
    public sealed class MapLoader
    {
        private const int SkipFileHeader = 1;

        public static Map Load(string path, SaveGame<Static, Dynamic, Node> saveGame, ContentManager content, GraphicsDevice graphics, SoundPlayer soundPlayer)
        {
            ModelsLoader modelLoader = new ModelsLoader(content, graphics, soundPlayer);
            return ReadInMap(path, modelLoader, saveGame);
        }

        private static Map ReadInMap(string path, ModelsLoader modelLoader, SaveGame<Static, Dynamic, Node> saveGame)
        {
            string[] mapData = File.ReadAllLines(path);

            MapFileFormats.Names format = MapFileFormats.FileFormat(mapData);

            switch (format)
            {
                case MapFileFormats.Names.Benchmark:    return LoadBenchmark(mapData);
                case MapFileFormats.Names.Default:      return LoadDefault(saveGame, mapData, modelLoader);
            }

            return null;
        }

        private static Map LoadBenchmark(string[] mapData)
        {
            return null;
        }

        private static Map LoadDefault(SaveGame<Static, Dynamic, Node> savegame, string[] mapData, ModelsLoader modelLoader)
        {
            Map map = new Map(savegame);
            StoreMapDimensions(map, mapData[0].Split(' '));  // 33 19 2
            CreateEmptyMap(map);
            SetMapModels(map, mapData, modelLoader);
            return map;
        }

        private static void StoreMapDimensions(Map map, string[] mapDimensions)
        {
            map.Width = Convert.ToInt32(mapDimensions[0]);
            map.Height = Convert.ToInt32(mapDimensions[1]);
            map.Depth = Convert.ToUInt32(mapDimensions[2]);
            map.Size = Convert.ToUInt32(map.Width * map.Height);
        }

        private static void CreateEmptyMap(Map map)
        {
            map.Collision = new bool[map.Width, map.Height];
            ////map.Nodes = map.FreshNodes;
        }

        private static void SetMapModels(Map map, string[] mapData, ModelsLoader modelLoader)
        {
            for (int z = 0; z < map.Depth; z++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    for (int x = 0; x < map.Width; x++)
                    {
                        NaviModel model;
                        SetMapModel(out model, map, modelLoader, x, y, z, mapData);
                        PlaceStatic(map, model);  // model can be null
                    }
                } 
            }

            map.Models.AddRange(map.Units);  // to place Units above everything
        }

        private static void SetMapModel(out NaviModel model, Map map, ModelsLoader modelLoader, int posX, int posY, int posZ, string[] mapData)
        {
            string[] dataLine = mapData[SkipFileHeader + posX + posY * map.Width].Split(' ');

            if (posZ < dataLine.Length)
            {
                string fullModelName = dataLine[posZ].Trim();  // reading from a specific map layer
                model = ComputeModelData(map, modelLoader, fullModelName, posX, posY);
            }
            else model = null;
        }

        private static NaviModel ComputeModelData(Map map, ModelsLoader modelLoader, string fullModelName, int posX, int posY)
        {
            NaviModel model = Model(modelLoader, fullModelName, posX, posY);
            SetCollisionData(map, model, posX, posY);
            return model;
        }

        private static NaviModel Model(ModelsLoader modelLoader, string fullModelName, int posX, int posY)
        {
            string[] nameSplit = fullModelName.Split('_');
            string packageName = nameSplit[0];
            string modelName = nameSplit[1];
            return modelLoader.CreateModel(packageName, modelName, posX, posY);
        }

        private static void SetCollisionData(Map map, NaviModel model, int posX, int posY)
        {
            if (model is Dynamic)
            {
                map.NumberOfUnits++;
                ////map.Nodes[posX, posY].IsOccupied = true;
                ////map.Nodes[posX, posY].Occluder = (Dynamic)model;
            }
            //// if (model is Static)
            else map.Collision[posX, posY] = model.IsBlocked;
        }

        private static void PlaceStatic(Map map, NaviModel model)
        {
            if (model != null)
            {
                if (!(model is Dynamic)) map.Models.Add((Static)model);
                else map.Units.Add((Dynamic)model);
            }
        }
    }
}
