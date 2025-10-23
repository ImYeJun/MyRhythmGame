using System;
using System.Collections.Generic;

[Serializable]
public struct BeatMapJsonData
{
    public BeatMapMetaData meta;
    public List<BpmPoint> bpmPoints;
    public List<Note> notes;

    public BeatMapJsonData(BeatMapMetaData meta, List<BpmPoint> bpmPoints, List<Note> notes)
    {
        this.meta = meta;
        this.bpmPoints = bpmPoints;
        this.notes = notes;
    }
}

[Serializable]
public struct BpmPoint
{
    public float bpm;
    public int time;

    public BpmPoint(float bpm, int time)
    {
        this.bpm = bpm;
        this.time = time;
    }
}

[Serializable]
public struct BeatMapMetaData
{
    public string trackTitle;
    public string composer;
    public int syncDelay;
    public int lobbyIntroStartTime;
    public int lobbyIntroDuration;

    public BeatMapMetaData(string trackTitle, string composer, int syncDelay, int lobbyIntroStartTime, int lobbyIntroDuration)
    {
        this.trackTitle = trackTitle;
        this.composer = composer;
        this.syncDelay = syncDelay;
        this.lobbyIntroStartTime = lobbyIntroStartTime;
        this.lobbyIntroDuration = lobbyIntroDuration;
    }
}