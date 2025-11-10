using System;
using System.Collections.Generic;
using System.IO;
using SFB;
using UnityEngine;

public class PlayResultManager : GameManager<PlayResultManager>
{
    private readonly static string PLAY_RESULT_SET_LIST_PATH = Path.Combine(Application.dataPath, "Resources/TrackMap/PlayResultSetList.json");
    private const string DEFAULT_PLAY_RESULT_SET_LIST_CONTENT = "{ \"playResultSetList\": [] }";
    private Dictionary<string, PlayResultSet> playResultSetDictionary = new Dictionary<string, PlayResultSet>(); //키 값은 Track의 HashCode임

    protected override void Awake()
    {
        base.Awake();

        if (this != Instance) return;

        LoadPlayResultSetList();
        // TestAddPlayResult();
    }

    public bool TryGetPlayResultSet(string trackHashCode, out PlayResultSet playResultSet)
    {
        if (playResultSetDictionary.ContainsKey(trackHashCode))
        {
            playResultSet = playResultSetDictionary[trackHashCode];
            return true;
        }

        playResultSet = null;
        return false;
    }

    
    public bool TryGetPlayResult(string trackHashCode, JudgeLevel judgeLevel, out PlayResult playResult)
    {
        PlayResultSet playResultSet;
        if (TryGetPlayResultSet(trackHashCode, out playResultSet))
        {
            playResult = playResultSet.GetPlayResult(judgeLevel);
            return true;
        }
        else if (TrackMapManager.Instance.HasTrackMapOfHashCode(trackHashCode))
        {
            playResultSet = new PlayResultSet();

            playResultSetDictionary.Add(trackHashCode, playResultSet);
            playResult = playResultSet.GetPlayResult(judgeLevel);

            return true;
        }
        else
        {
            playResult = null;
            return false;
        }
    }

    public void RenewPlayResultSetList(string trackHashCode, JudgeLevel judgeLevel, PlayResult playResult)
    {
        if (!playResultSetDictionary.ContainsKey(trackHashCode))
        {
            PlayResultSet playResultSet = new PlayResultSet();
            playResultSet.TryRenewPlayerResult(judgeLevel, playResult);
            playResultSetDictionary.Add(trackHashCode, playResultSet);
            RefreshPlayResultSetListFile();
        }
        else
        {
            PlayResultSet existingPlayResultSet = playResultSetDictionary[trackHashCode];
            if (existingPlayResultSet.TryRenewPlayerResult(judgeLevel, playResult))
            {
                RefreshPlayResultSetListFile();
            }
        }
    }

    private void RefreshPlayResultSetListFile()
    {
        PlayResultSetListJsonData playResultSetsJsonData = new PlayResultSetListJsonData();

        foreach (string trackHashCode in playResultSetDictionary.Keys)
        {
            PlayResultSetJsonData playResultSetJsonData = new PlayResultSetJsonData(trackHashCode, playResultSetDictionary[trackHashCode]);
            playResultSetsJsonData.playResultSetList.Add(playResultSetJsonData);
        }
        string jsonData = JsonUtility.ToJson(playResultSetsJsonData);

        EnsurePlayResultSetListFileExist();

        File.WriteAllText(PLAY_RESULT_SET_LIST_PATH, jsonData);

        LoadPlayResultSetList(); //! 성능 문제가 발생할 수도..?
    }

    private void LoadPlayResultSetList()
    {
        EnsurePlayResultSetListFileExist();

        playResultSetDictionary.Clear();

        string jsonData = File.ReadAllText(PLAY_RESULT_SET_LIST_PATH);
        PlayResultSetListJsonData playerResultSetsJsonData = JsonUtility.FromJson<PlayResultSetListJsonData>(jsonData);

        foreach (PlayResultSetJsonData playerResultJsonData in playerResultSetsJsonData.playResultSetList)
        {
            playResultSetDictionary.Add(playerResultJsonData.hashCode, playerResultJsonData.playResultSet);
        }
    }

    private void EnsurePlayResultSetListFileExist()
    {
        if (!File.Exists(PLAY_RESULT_SET_LIST_PATH))
        {
            Debug.Log("PlayResultSetList.json was created because no existing file was found.");
            string defaultContent = DEFAULT_PLAY_RESULT_SET_LIST_CONTENT;
            File.WriteAllText(PLAY_RESULT_SET_LIST_PATH, defaultContent);
        }
    }

    //! Test
    private void TestAddPlayResult()
    {
        string TEST_TRACK_HASH = "213";
        var result1 = new PlayResult(
            hasPlayed: true,
            isMaxCombo: false,
            isPerfect: false,
            perfectCount: 100,
            earlyNiceCount: 10,
            earlyGoodCount: 5,
            earlyTlqkfCount: 1,
            lateNiceCount: 7,
            lateGoodCount: 3,
            lateTlqkfCount: 0
        );

        RenewPlayResultSetList(TEST_TRACK_HASH, JudgeLevel.Basic, result1);

        // 2️⃣ Try to add LOWER score → should not replace
        var lowerResult = new PlayResult(
            hasPlayed: true,
            isMaxCombo: false,
            isPerfect: false,
            perfectCount: 80,
            earlyNiceCount: 5,
            earlyGoodCount: 4,
            earlyTlqkfCount: 1,
            lateNiceCount: 5,
            lateGoodCount: 4,
            lateTlqkfCount: 1
        );

        RenewPlayResultSetList(TEST_TRACK_HASH, JudgeLevel.Basic, lowerResult);

        // 3️⃣ Add HIGHER score → should replace
        var higherResult = new PlayResult(
            hasPlayed: true,
            isMaxCombo: true,
            isPerfect: true,
            perfectCount: 200, // better performance
            earlyNiceCount: 3,
            earlyGoodCount: 2,
            earlyTlqkfCount: 0,
            lateNiceCount: 4,
            lateGoodCount: 1,
            lateTlqkfCount: 0
        );

        RenewPlayResultSetList(TEST_TRACK_HASH, JudgeLevel.Basic, higherResult);
    }
}



