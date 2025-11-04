using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayResultManager : GameManager<PlayResultManager>
{
    private readonly static string PLAY_RESULT_SETS_PATH = Path.Combine(Application.dataPath, "Resources/BeatMap/PlayResultSets.json");
    private const string DEFAULT_PLAY_RESULT_SETS_FILE_CONTENT = "{ \"playResultSets\": [] }";
    private Dictionary<string, PlayResultSet> playResultSetDict = new Dictionary<string, PlayResultSet>();

    protected override void Awake()
    {
        base.Awake();

        if (this != Instance) return;

        LoadPlayResultSet();
        TestAddPlayResult();
    }

    public bool TryGetPlayResult(string trackHashCode, out PlayResultSet playResultSet)
    {
        if (playResultSetDict.ContainsKey(trackHashCode))
        {
            playResultSet = playResultSetDict[trackHashCode];
            return true;
        }

        playResultSet = null;
        return false;
    }

    public void RenewPlayResultSet(string trackHashCode, JudgeLevel judgeLevel, PlayResult playResult)
    {
        if (!playResultSetDict.ContainsKey(trackHashCode))
        {
            PlayResultSet playResultSet = new PlayResultSet();
            playResultSet.TryToRenewPlayerResult(judgeLevel, playResult);
            playResultSetDict.Add(trackHashCode, playResultSet);
            RefreshPlayResultSetsFile();
        }
        else
        {
            PlayResultSet playResultSet = playResultSetDict[trackHashCode];
            if (playResultSet.TryToRenewPlayerResult(judgeLevel, playResult))
            {
                RefreshPlayResultSetsFile();
            }
        }
    }

    private void RefreshPlayResultSetsFile()
    {
        PlayResultSetsJsonData playResultSetsJsonData = new PlayResultSetsJsonData(new List<PlayResultSetJsonData>());

        foreach (string trackHashCode in playResultSetDict.Keys)
        {
            PlayResultSetJsonData playResultSetJsonData = new PlayResultSetJsonData(trackHashCode, playResultSetDict[trackHashCode]);
            playResultSetsJsonData.playResultSets.Add(playResultSetJsonData);
        }
        string jsonData = JsonUtility.ToJson(playResultSetsJsonData);

        if (!File.Exists(PLAY_RESULT_SETS_PATH))
        {
            Debug.Log("PlayResultSets.json was created since there'd been no such a file.");
            string defaultData = DEFAULT_PLAY_RESULT_SETS_FILE_CONTENT;
            File.WriteAllText(PLAY_RESULT_SETS_PATH, defaultData);
        }

        File.WriteAllText(PLAY_RESULT_SETS_PATH, jsonData);

        LoadPlayResultSet(); //! 성능 문제가 발생할 수도..?
    }

    private void LoadPlayResultSet()
    {
        if (!File.Exists(PLAY_RESULT_SETS_PATH))
        {
            Debug.Log("PlayResultSets.json was created since there'd been no such a file.");
            string defaultData = "{}";
            File.WriteAllText(PLAY_RESULT_SETS_PATH, defaultData);
        }

        playResultSetDict.Clear();

        string jsonData = File.ReadAllText(PLAY_RESULT_SETS_PATH);
        PlayResultSetsJsonData playerResultSetsJsonData = JsonUtility.FromJson<PlayResultSetsJsonData>(jsonData);

        foreach (PlayResultSetJsonData playerResultJsonData in playerResultSetsJsonData.playResultSets)
        {
            playResultSetDict.Add(playerResultJsonData.hashCode, playerResultJsonData.playResultSet);
        }
    }

    //! Test
    private void TestAddPlayResult()
    {
        string TEST_TRACK_HASH = "213";
        var result1 = new PlayResult(
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

        RenewPlayResultSet(TEST_TRACK_HASH, JudgeLevel.Basic, result1);

        // 2️⃣ Try to add LOWER score → should not replace
        var lowerResult = new PlayResult(
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

        RenewPlayResultSet(TEST_TRACK_HASH, JudgeLevel.Basic, lowerResult);

        // 3️⃣ Add HIGHER score → should replace
        var higherResult = new PlayResult(
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

        RenewPlayResultSet(TEST_TRACK_HASH, JudgeLevel.Basic, higherResult);
    }
}



