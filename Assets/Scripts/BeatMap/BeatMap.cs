using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using Mono.Cecil;

public class BeatMap
{
    private string trackTitle;
    private string composer;
    private int syncDelay;
    private int lobbyIntroStartTime;
    private int lobbyIntroDuration;
    private List<Tuple<float, int>> bpmPoints = new List<Tuple<float, int>>();
    private List<Note> notes = new List<Note>();
    private AudioClip music;
    private Sprite coverImage;
    private string hashCode;

    /// <summary>
    /// all path parameters must be relative path from the 'Resources' floder.
    /// </summary>
    public BeatMap(string jsonPath, string coverImagePath, string musicPath)
    {
        // string json = File.ReadAllText(jsonPath);

        // var shit = JsonUtility.FromJson<BeatMapJsonData>(json);

        coverImage = Resources.Load<Sprite>(coverImagePath);
        music = Resources.Load<AudioClip>(musicPath);
    }

    public string TrackTitle { get => trackTitle; }
    public string Composer { get => composer; }
    public int SyncDelay { get => syncDelay; }
    public int LobbyIntroStartTime { get => lobbyIntroStartTime; }
    public int LobbyIntroDuration { get => lobbyIntroDuration; }
    public List<Tuple<float, int>> BpmPoints { get => bpmPoints; }
    public AudioClip Music { get => music; }
    public Sprite CoverImage { get => coverImage; }
    public string HashCode { get => hashCode; }
}

[Serializable]
public class BeatMapJsonData
{
    public string trackTitle;
    public string composer;
    public int syncDelay;
    public int lobbyIntroStartTime;
    public int lobbyIntroDuration;
    public List<Tuple<float, int>> bpmPoints = new List<Tuple<float, int>>();
    public List<Note> notes = new List<Note>();
}