using UnityEngine;
using System;
using System.Collections.Generic;

public class BeatMap {
    private string trackTitle;
    private string composer;
    private int syncDelay;
    private int lobbyIntroStartTime;
    private int lobbyIntroDuration;
    private List<Tuple<float, int>> bpmPoints;
    private AudioClip music;
    private Sprite coverImage;
    private string hashCode;
    
    /// <summary>
    /// @param jsonURL 
    /// @param coverImageURL 
    /// @param musicURL
    /// </summary>
    public BeatMap(string jsonPath, string coverImagePath, string musicPath)
    {
        // TODO implement here
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