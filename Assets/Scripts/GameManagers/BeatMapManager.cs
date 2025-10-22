using UnityEngine;
using System.Collections.Generic;

public class BeatMapManager : GameManager<BeatMapManager>{
    [SerializeField] private readonly static string BEAT_MAP_FOLDER_URL;

    private Dictionary<string, BeatMap> beatMaps;

    private void Start()
    {
        InitiateBeatMap();
    }
    private bool InitiateBeatMap() {
        // TODO implement here
        return false;
    }

    /// <summary>
    /// @param folderURL 
    /// @return
    /// </summary>
    public bool AddBeatMap(string folderURL) {
        // TODO implement here
        return false;
    }

    /// <summary>
    /// @param hashCode  
    /// @return
    /// </summary>
    public BeatMap GetBeatMap(string hashCode) {
        // TODO implement here
        return null;
    }

}