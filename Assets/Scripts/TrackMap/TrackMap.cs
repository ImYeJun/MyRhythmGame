using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Security.Cryptography;

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

        // Resources 상대 경로 변환
        string coverResourcePath = ConvertToResourcesPath(coverImagePath);
        string musicResourcePath = ConvertToResourcesPath(musicPath);

        // Sprite, AudioClip 로드
        coverImage = Resources.Load<Sprite>(coverResourcePath);
        clip = Resources.Load<AudioClip>(musicResourcePath);
        
        // hashCode 구하기
        hashCode = GenerateSHA256FromFiles(jsonPath, coverImagePath, musicPath);
    }

    /// <summary>
    /// 절대 경로나 Resources 하위 경로를 Resources.Load에서 쓸 수 있는 형태로 변환
    /// </summary>
    private string ConvertToResourcesPath(string path)
    {
        // 확장자 제거
        path = Path.ChangeExtension(path, null);

        // 경로 정규화 (\\ → /)
        path = path.Replace("\\", "/");

        // "Assets/Resources/" 포함 시 해당 부분 제거
        int index = path.IndexOf("Resources/");
        if (index >= 0)
        {
            path = path.Substring(index + "Resources/".Length);
        }

        return path;
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
