using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;

public class TrackMapManager : GameManager<TrackMapManager> {
    private readonly static string TRACK_MAP_FOLDER_URL = Path.Combine(Application.dataPath, "Resources/TrackMap");

    private class TrackMapDictionaryItem
    {
        private readonly string trackMapDataFolderPath;
        private readonly TrackMap trackMap;

        public string TrackMapDataFolderPath => trackMapDataFolderPath;
        public TrackMap TrackMap => trackMap;

        public TrackMapDictionaryItem(string trackMapDataFolderPath, TrackMap trackMap)
        {
            this.trackMapDataFolderPath = trackMapDataFolderPath;
            this.trackMap = trackMap;
        }
    }

    private Dictionary<string, TrackMapDictionaryItem> trackMapDictionary = new Dictionary<string, TrackMapDictionaryItem>();
    public List<TrackMap> TrackMaps => trackMapDictionary.Values
        .Select(item => item.TrackMap)
        .ToList();

    protected override void Awake()
    {
        base.Awake();
        if (Instance != this) return;

        InitiateTrackMap();
    }

    private void InitiateTrackMap() {
        string[] trackMapDataFolderPaths = Directory.GetDirectories(TRACK_MAP_FOLDER_URL);

        foreach (string folderPath in trackMapDataFolderPaths)
        {
            if (!TryAddTrackMap(folderPath))
            {
                Debug.LogError($"Failed To Load TrackMap (Path : {folderPath})");
            }
        }
    }

    public bool TryAddTrackMap(string folderPath)
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

        if (jsonPath == null || coverImagePath == null || musicPath == null) {
            Debug.LogError($"폴더 내에 필수 파일이 모두 존재하지 않습니다. (Path : {folderPath})");
            return false;
        }

        TrackMap trackMap = new TrackMap(jsonPath, coverImagePath, musicPath);

        if (trackMap.BpmPoints.Count == 0)
        {
            Debug.LogError($"파일내의 Bpm 표시 형식이 유효하지 않습니다. (Path : {folderPath})");
            return false;
        }

        TrackMapDictionaryItem trackMapDictionaryItem = new TrackMapDictionaryItem(folderPath, trackMap);
        trackMapDictionary.Add(trackMap.HashCode, trackMapDictionaryItem);

        return true;
    }

    public bool TryGetTrackMap(string hashCode, out TrackMap trackMap) {
        TrackMapDictionaryItem trackMapDictionaryItem;

        if (trackMapDictionary.TryGetValue(hashCode, out trackMapDictionaryItem))
        {
            trackMap = trackMapDictionaryItem.TrackMap;
            return true;
        }
        else
        {
            trackMap = null;
            return false;
        }
    }
}