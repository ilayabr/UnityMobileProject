using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public SaveData currentSaveData;

    public string gameSavesPath { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        gameSavesPath = Application.persistentDataPath + "/GameSaves";
    }

    public void SaveData()
    {
        CreateSaveDirectory();

        string currentSavePath = Path.Combine(gameSavesPath, $"{DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")}");

        Directory.CreateDirectory(currentSavePath);

        string jsonData = JsonConvert.SerializeObject(currentSaveData);

        File.WriteAllText(Path.Combine(currentSavePath, "save.json"), jsonData);

        var bytes = GetCameraView().EncodeToPNG();

        File.WriteAllBytes(Path.Combine(currentSavePath, "snapshot.png"), bytes);
    }

    private Texture2D GetCameraView()
    {
        //get camera
        var camera = Camera.main;

        //create a render texture to capture the current frame :D
        RenderTexture renderTex = new RenderTexture(854, 480, 24);
        camera.targetTexture = renderTex;

        //call the camera to render do the render texture gets the frame data
        camera.Render();

        //activating this render texture so we can read it onto a new texture2D
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = renderTex;

        //create new texture2D and read the activated texture ^^^^
        Texture2D tex = new Texture2D(854, 480, TextureFormat.RGBA32, false);
        tex.ReadPixels(new Rect(0, 0, 854, 480), 0, 0);
        tex.Apply();

        //remove all the stuffs from existence
        camera.targetTexture = null;
        RenderTexture.active = currentRT;

        return tex;
    }

    /// <summary>
    /// gets a game save and loads it into the current game save
    /// </summary>
    /// <param name="saveName">name of the save folder</param>
    public void LoadData(string saveName)
    {
        currentSaveData = GetSaveData(saveName);
    }
    
    /// <summary>
    /// gets a game save and loads it into the current game save
    /// </summary>
    /// <param name="extendedSave">an extended save</param>
    public void LoadData(ExtendedSaveData extendedSave)
    {
        currentSaveData = extendedSave.GetSaveData();
    }

    /// <summary>
    /// gets the game save data from a save folder
    /// </summary>
    /// <param name="saveName">the name of the game save folder</param>
    /// <returns>the found save data, if not found returns null</returns>
    public SaveData GetSaveData(string saveName)
    {
        string savePath = Path.Combine(gameSavesPath, saveName);
        if (!Directory.Exists(savePath)) return null;

        var saveFiles = Directory.GetFiles(savePath);

        foreach (var file in saveFiles)
        {
            if (Path.GetFileName(file).Equals("save.json"))
            {
                return JsonConvert.DeserializeObject<SaveData>(File.ReadAllText(file));
            }
        }

        return null;
    }

    /// <summary>
    /// returns ALL existing game saves as 'ExtendedSaveData's with their snapshot
    /// </summary>
    /// <returns>list of all saves</returns>
    public List<ExtendedSaveData> GetAllExtSaves()
    {
        List<ExtendedSaveData> toReturn = new();

        var saveFolders = Directory.GetDirectories(gameSavesPath).ToList().OrderByDescending(dir => Directory.GetCreationTime(dir)).ToArray();

        foreach (var folder in saveFolders)
        {
            if (!Directory.Exists(folder)) continue;

            var files = Directory.GetFiles(folder, "*.png");
            if (files == null || files.Length == 0) continue;

            string currentSaveName = Path.GetFileName(folder);

            var imageBytes = File.ReadAllBytes(files[0]);

            var snapshotTexture = new Texture2D(854, 480);
            snapshotTexture.LoadImage(imageBytes);

            toReturn.Add(new ExtendedSaveData(currentSaveName, snapshotTexture));
        }

        return toReturn;
    }

    private void CreateSaveDirectory()
    {
        if (!Directory.Exists(gameSavesPath))
            Directory.CreateDirectory(gameSavesPath);
    }
}
