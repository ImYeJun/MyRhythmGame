using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using UnityEngine.Networking;

public class TrackMap
{
    private string trackTitle;
    private string composer;
    private int syncDelay;
    private int lobbyIntroStartTime;
    private int lobbyIntroDuration;
    private List<BpmPoint> bpmPoints = new List<BpmPoint>();
    private float minBpm;
    private float maxBpm;
    private List<Note> notes = new List<Note>();
    private AudioClip clip;
    private Sprite coverImage;
    private string hashCode;
    
    public string TrackTitle { get => trackTitle; }
    public string Composer { get => composer; }
    public int SyncDelay { get => syncDelay; }
    public int LobbyIntroStartTime { get => lobbyIntroStartTime; }
    public int LobbyIntroDuration { get => lobbyIntroDuration; }
    public List<BpmPoint> BpmPoints { get => bpmPoints; }
    public float MinBpm { get => minBpm; }
    public float MaxBpm { get => maxBpm; }
    public List<Note> Notes { get => notes; set => notes = value; }
    public AudioClip Clip { get => clip; }
    public Sprite CoverImage { get => coverImage; }
    public string HashCode { get => hashCode; }

    /// <summary>
    /// all path parameters must be absoulte path.
    /// </summary>
    public TrackMap(string jsonPath, string coverImagePath, string musicPath)
    {
        // JSON 읽고 필드 초기화
        string json = File.ReadAllText(jsonPath);
        TrackMapJsonData data = JsonUtility.FromJson<TrackMapJsonData>(json);

        trackTitle = data.meta.trackTitle;
        composer = data.meta.composer;
        syncDelay = data.meta.syncDelay;
        lobbyIntroStartTime = data.meta.lobbyIntroStartTime;
        lobbyIntroDuration = data.meta.lobbyIntroDuration;
        bpmPoints = data.bpmPoints;
        notes = data.notes;

        // 최소, 최대 bpm 구하기 (단, bpmPoint가 한개 이상일 때만)
        if (bpmPoints.Count != 0)
        {
            float currentMinBpm, currentMaxBpm;
            currentMinBpm = currentMaxBpm = bpmPoints[0].bpm; // TrackMap 생성시 bpmPoints의 원소가 반드시 한 개 이상임이 보장된다.
            foreach (BpmPoint bpmPoint in bpmPoints)
            {
                float currentBpm = bpmPoint.bpm;
                if (bpmPoint.bpm < currentMinBpm) { currentMinBpm = currentBpm; }
                if (bpmPoint.bpm > currentMaxBpm) { currentMaxBpm = currentBpm; }
            }

            minBpm = currentMinBpm;
            maxBpm = currentMaxBpm;
        }

        // Sprite, AudioClip 직접 로드
        coverImage = LoadSpriteFromFile(coverImagePath);
        LoadAudioClipAsync(musicPath).ContinueWith(task =>
        {
            clip = task.Result;
        });
        
        // hashCode 구하기
        hashCode = GenerateSHA256FromFiles(jsonPath, coverImagePath, musicPath);
    }

    private Sprite LoadSpriteFromFile(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogWarning($"Cover image not found: {path}");
            return null;
        }

        byte[] imageBytes = File.ReadAllBytes(path);
        Texture2D tex = new Texture2D(2, 2);
        if (!tex.LoadImage(imageBytes))
        {
            Debug.LogWarning($"Failed to load image: {path}");
            return null;
        }
        return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
    }

    private async Task<AudioClip> LoadAudioClipAsync(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogWarning($"Audio file not found: {path}");
            return null;
        }

        string url = "file://" + path;
        AudioType audioType = GetAudioTypeFromExtension(path);

        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, audioType))
        {
            var operation = www.SendWebRequest();

            while (!operation.isDone)
                await Task.Yield();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogWarning($"Failed to load audio: {path}, error: {www.error}");
                return null;
            }

            return DownloadHandlerAudioClip.GetContent(www);
        }
    }

    private AudioType GetAudioTypeFromExtension(string path)
    {
        string ext = Path.GetExtension(path).ToLower();
        switch (ext)
        {
            case ".wav": return AudioType.WAV;
            case ".mp3": return AudioType.MPEG;
            case ".ogg": return AudioType.OGGVORBIS;
            default:
                Debug.LogWarning($"Unknown audio format for {path}, defaulting to WAV.");
                return AudioType.WAV;
        }
    }

    private string GenerateSHA256FromFiles(string jsonPath, string coverPath, string musicPath)
    {
        using (SHA256 sha = SHA256.Create())
        {
            using (MemoryStream combinedStream = new MemoryStream())
            {
                // JSON
                if (File.Exists(jsonPath))
                {
                    byte[] jsonBytes = File.ReadAllBytes(jsonPath);
                    combinedStream.Write(jsonBytes, 0, jsonBytes.Length);
                }

                // Cover image
                if (File.Exists(coverPath))
                {
                    byte[] imageBytes = File.ReadAllBytes(coverPath);
                    combinedStream.Write(imageBytes, 0, imageBytes.Length);
                }

                // Music file
                if (File.Exists(musicPath))
                {
                    byte[] musicBytes = File.ReadAllBytes(musicPath);
                    combinedStream.Write(musicBytes, 0, musicBytes.Length);
                }

                // 스트림 처음으로 이동
                combinedStream.Position = 0;

                // SHA256 계산
                byte[] hashBytes = sha.ComputeHash(combinedStream);

                // 16진수 문자열 변환
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }
    }

}
