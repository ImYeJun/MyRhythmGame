using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayResult : IComparable<PlayResult>
{
    public const float NICE_SCORE_RATIO = 0.7f;
    public const float GOOD_SCORE_RATIO = 0.2f;
    public const float MAX_SCORE = 1_000_000;

    public bool isMaxCombo;
    public bool isPerfect;
    public int score;
    public float rate;
    public int perfectCount;
    public int earlyNiceCount;
    public int earlyGoodCount;
    public int earlyTlqkfCount;
    public int lateNiceCount;
    public int lateGoodCount;
    public int lateTlqkfCount;

    // public bool IsMaxCombo { get => isMaxCombo; }
    // public bool IsPerfect { get => isPerfect; }
    // public int Score { get => score; set => score = value; }
    // public float Rate { get => rate; set => rate = value; }
    // public int PerfectCount { get => perfectCount; }
    // public int EarlyNiceCount { get => earlyNiceCount; }
    // public int EarlyGoodCount { get => earlyGoodCount; }
    // public int EarlyTlqkfCount { get => earlyTlqkfCount; }
    // public int LateNiceCount { get => lateNiceCount; }
    // public int LateGoodCount { get => lateGoodCount; }
    // public int LateTlqkfCount { get => lateTlqkfCount; }
    public int NiceCount { get => earlyNiceCount + lateNiceCount; }
    public int GoodCount { get => earlyGoodCount + lateGoodCount; }
    public int TlqkfCount { get => earlyTlqkfCount + lateTlqkfCount; }
    public int FullComboCount { get => perfectCount + NiceCount + GoodCount + TlqkfCount; }

    public PlayResult(bool isMaxCombo, bool isPerfect, int score, float rate, int perfectCount, int earlyNiceCount, int earlyGoodCount, int earlyTlqkfCount, int lateNiceCount, int lateGoodCount, int lateTlqkfCount)
    {
        this.isMaxCombo = isMaxCombo;
        this.score = score;
        this.rate = rate;
        this.isPerfect = isPerfect;
        this.perfectCount = perfectCount;
        this.earlyNiceCount = earlyNiceCount;
        this.earlyGoodCount = earlyGoodCount;
        this.earlyTlqkfCount = earlyTlqkfCount;
        this.lateNiceCount = lateNiceCount;
        this.lateGoodCount = lateGoodCount;
        this.lateTlqkfCount = lateTlqkfCount;
    }

    public PlayResult(bool isMaxCombo, bool isPerfect, int perfectCount, int earlyNiceCount, int earlyGoodCount, int earlyTlqkfCount, int lateNiceCount, int lateGoodCount, int lateTlqkfCount)
        : this(
            isMaxCombo, isPerfect,
            CalculateScore(perfectCount, earlyNiceCount, earlyGoodCount, earlyTlqkfCount, lateNiceCount, lateGoodCount, lateTlqkfCount),
            CalculateRate(perfectCount, earlyNiceCount, earlyGoodCount, earlyTlqkfCount, lateNiceCount, lateGoodCount, lateTlqkfCount),
            perfectCount, earlyNiceCount, earlyGoodCount, earlyTlqkfCount, lateNiceCount, lateGoodCount, lateTlqkfCount)
    {}

    public int CompareTo(PlayResult other)
    {
        if (rate != other.rate)
        {
            return rate.CompareTo(other.rate);
        }

        return score.CompareTo(other.score);
    }

    /// <summary>
    /// Calculates a weighted accuracy percentage based on the counts of different hit judgments.
    /// </summary>
    /// <returns>
    /// A floating-point percentage in the range [0,100] representing the weighted accuracy:
    /// (weightedScore / totalHits) * 100. If no notes were hit (totalHits == 0). Returns -1 if no notes were hit (all counts sum to zero)
    /// </returns>
    public static float CalculateRate(int perfectCount, int earlyNiceCount, int earlyGoodCount, int earlyTlqkfCount, int lateNiceCount, int lateGoodCount, int lateTlqkfCount)
    {
        int niceCount = earlyNiceCount + lateNiceCount;
        int goodCount = earlyGoodCount + lateGoodCount;
        int tlqkfCount = earlyTlqkfCount + lateTlqkfCount;
        int fullComboCount = perfectCount + niceCount + goodCount + tlqkfCount;

        if (fullComboCount == 0)
        {
            Debug.Log("User didn't hit any Note");
            return -1;
        }

        float value = (perfectCount * 1.0f) + (niceCount * NICE_SCORE_RATIO) + (goodCount * GOOD_SCORE_RATIO) + (tlqkfCount * 0.0f);
        float ratio = value / fullComboCount * 100;

        return ratio;
    }
    
    /// <summary>
    /// Calculates a final score based on counts of different hit accuracies.
    /// </summary>
    /// <returns>
    /// An integer score computed by taking the weighted sum of hit categories, dividing by the total possible hits (max combo),
    /// multiplying by <c>MAX_SCORE</c>, and rounding to the nearest integer. Returns -1 if no notes were hit (all counts sum to zero).
    /// </returns>
    public static int CalculateScore(int perfectCount, int earlyNiceCount, int earlyGoodCount, int earlyTlqkfCount, int lateNiceCount, int lateGoodCount, int lateTlqkfCount)
    {
        int niceCount = earlyNiceCount + lateNiceCount;
        int goodCount = earlyGoodCount + lateGoodCount;
        int tlqkfCount = earlyTlqkfCount + lateTlqkfCount;
        int fullComboCount = perfectCount + niceCount + goodCount + tlqkfCount;

        if (fullComboCount == 0) {
            Debug.Log("User didn't hit any Note");
            return -1;
        }

        float value = (perfectCount * 1.0f) + (niceCount * NICE_SCORE_RATIO) + (goodCount * GOOD_SCORE_RATIO) + (tlqkfCount * 0.0f);
        int score = (int)Mathf.Round(value / fullComboCount * MAX_SCORE);

        return score;
    }
    
}

[Serializable]
public class PlayResultSet
{
    public PlayResult basicPlayResult;
    public PlayResult intermediatePlayResult;
    public PlayResult professionalPlayResult;

    // public PlayResult BasicPlayerResult { get => basicPlayerResult; }
    // public PlayResult IntermediatePlayerResult { get => intermediatePlayerResult; }
    // public PlayResult ProfessionalPlayerResult { get => professionalPlayerResult; }

    public PlayResultSet()
    {
        basicPlayResult = null;
        intermediatePlayResult = null;
        professionalPlayResult = null;
    }

    public PlayResultSet(PlayResult basicPlayResult, PlayResult intermediatePlayResult, PlayResult professionalPlayResult)
    {
        this.basicPlayResult = basicPlayResult;
        this.intermediatePlayResult = intermediatePlayResult;
        this.professionalPlayResult = professionalPlayResult;
    }

    public bool TryRenewPlayerResult(JudgeLevel judgeLevel, PlayResult playResult)
    {
        switch (judgeLevel)
        {
            case JudgeLevel.Basic:
                if (basicPlayResult == null || playResult.CompareTo(basicPlayResult) > 0) { basicPlayResult = playResult; return true; }
                break;

            case JudgeLevel.Intermediate:
                if (intermediatePlayResult == null || playResult.CompareTo(intermediatePlayResult) > 0) { intermediatePlayResult = playResult; return true; }
                break;

            case JudgeLevel.Professional:
                if (professionalPlayResult == null || playResult.CompareTo(professionalPlayResult) > 0) { professionalPlayResult = playResult; return true; }
                break;

            default:
                Debug.LogError($"JudgeLevel of {judgeLevel} is not existing");
                break;
        }

        return false;
    }
}

[Serializable]
public class PlayResultSetJsonData
{
    public string hashCode;
    public PlayResultSet playResultSet;

    public PlayResultSetJsonData(string hashCode, PlayResultSet playResultSet)
    {
        this.hashCode = hashCode;
        this.playResultSet = playResultSet;
    }
}

[Serializable]
public class PlayResultSetListJsonData
{
    public List<PlayResultSetJsonData> playResultSetList = new List<PlayResultSetJsonData>();

    public PlayResultSetListJsonData(List<PlayResultSetJsonData> playResultSetList)
    {
        this.playResultSetList = playResultSetList;
    }

    public PlayResultSetListJsonData() {}
}