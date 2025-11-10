using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using SFB;
using System.IO.Compression;

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
    public bool HasTrackMapOfHashCode(string hashCode) => trackMapDictionary.ContainsKey(hashCode);
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
        string[] allFilePaths = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories);

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

    public bool TryGetTrackMap(string hashCode, out TrackMap trackMap)
    {
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
    
    public bool TryInsertTrack()
    {
        //This code line implies user selecting .zip file for adding TrackMap.
        //If user selected any .zip file, trackMapZipFilePath has one element for the path user selected
        //If user didn't selected, trackMapZipFilePath has no elements. 
        string[] trackMapZipFilePath = StandaloneFileBrowser.OpenFilePanel("테스트", "", "zip", false);

        if (trackMapZipFilePath.Length == 0) return false;
        
        try
        {
            string zipFilePath = trackMapZipFilePath[0];
            
            // ZIP 내부 검사
            using (ZipArchive archive = ZipFile.OpenRead(zipFilePath))
            {
                bool hasTrackMapJson = archive.Entries.Any(e => e.FullName.EndsWith(".json"));
                bool hasTrackCoverImage = archive.Entries.Any(e => e.FullName.EndsWith(".png") || e.FullName.EndsWith(".jpg") || e.FullName.EndsWith(".jpeg") || e.FullName.EndsWith(".bmp") || e.FullName.EndsWith(".gif"));
                bool hasTrackMusicFile = archive.Entries.Any(e => e.FullName.EndsWith(".mp3") || e.FullName.EndsWith(".wav") || e.FullName.EndsWith(".ogg") || e.FullName.EndsWith(".flac"));

                if (hasTrackMapJson && hasTrackCoverImage && hasTrackMusicFile)
                {
                    Debug.Log($"ZIP 구조가 조건을 충족합니다. {TRACK_MAP_FOLDER_URL} 에 압축을 풉니다...");

                    // 폴더 없으면 자동 생성
                    if (!Directory.Exists(TRACK_MAP_FOLDER_URL))
                        Directory.CreateDirectory(TRACK_MAP_FOLDER_URL);
                    
                    // ✅ 기존 폴더 유지하면서 파일 덮어쓰기
                    foreach (var entry in archive.Entries)
                    {
                        string destinationPath = Path.Combine(TRACK_MAP_FOLDER_URL, entry.FullName);

                        // 폴더 엔트리는 건너뜀
                        if (string.IsNullOrEmpty(entry.Name))
                            continue;

                        // 상위 폴더 없으면 생성
                        string directory = Path.GetDirectoryName(destinationPath);
                        if (!Directory.Exists(directory))
                            Directory.CreateDirectory(directory);

                        // 파일 추출 (덮어쓰기)
                        entry.ExtractToFile(destinationPath, overwrite: true);
                    }

                    string extractFolderPath = Path.Combine(TRACK_MAP_FOLDER_URL, Path.GetFileNameWithoutExtension(zipFilePath));
                    Debug.Log($"압축 해제 완료: {extractFolderPath}");

                    TryAddTrackMap(extractFolderPath);
                    return true;
                }
                else
                {
                    Debug.LogWarning("ZIP 파일의 구조가 요구 조건을 충족하지 않습니다. 압축 해제 중단.");
                    return false;
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("ZIP 처리 중 오류 발생: " + ex.Message);
            return false;
        }
    }

    public bool TryDeleteTrack(TrackMap selectedTrack)
    {
        return false;
    }
}