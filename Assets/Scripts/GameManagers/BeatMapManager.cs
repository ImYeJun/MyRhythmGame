using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;

public class BeatMapManager : GameManager<BeatMapManager> {
    private readonly static string BEAT_MAP_FOLDER_URL = Path.Combine(Application.dataPath, "Resources/BeatMap");

    private Dictionary<string, BeatMap> beatMaps = new Dictionary<string, BeatMap>();
    public Dictionary<string, BeatMap> BeatMaps { get => beatMaps; }

    private void Start()
    {
        InitiateBeatMap();
    }

    private void InitiateBeatMap() {
        string[] beatMapDataFolderPaths = Directory.GetDirectories(BEAT_MAP_FOLDER_URL);

        foreach (string folderPath in beatMapDataFolderPaths)
        {
            if (!TryAddBeatMap(folderPath))
            {
                Debug.LogError($"Failed To Load BeatMap (Path : {folderPath})");
            }
        }
    }

    public bool TryAddBeatMap(string folderPath)
    {
        string[] allFilePaths = Directory.GetFiles(folderPath);

        string jsonPath = allFilePaths.FirstOrDefault(f => Path.GetExtension(f).Equals(".json", StringComparison.OrdinalIgnoreCase));
        string coverImagePath = allFilePaths.FirstOrDefault(f =>
            {
                string ext = Path.GetExtension(f).ToLower();
                return ext == ".png" || ext == ".jpg" || ext == ".jpeg" || ext == ".bmp" || ext == ".gif";
            });
        string musicPath = allFilePaths.FirstOrDefault(f =>
            {
                string ext = Path.GetExtension(f).ToLower();
                return ext == ".mp3" || ext == ".wav" || ext == ".ogg" || ext == ".flac";
            });

        if (jsonPath == null || coverImagePath == null || musicPath == null) return false;

        BeatMap beatMap = new BeatMap(jsonPath, coverImagePath, musicPath);

        beatMaps.Add(beatMap.HashCode, beatMap);

        return true;
    }

    public bool TryGetBeatMap(string hashCode, out BeatMap beatMap) => beatMaps.TryGetValue(hashCode, out beatMap);
}