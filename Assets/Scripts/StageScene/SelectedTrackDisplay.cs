using System;
using UnityEngine;

public class SelectedTrackDisplay : MonoBehaviour
{
    //TODO GUI 관련 속성 추가하기
    private BeatMap selectedTrack;

    private void Awake()
    {
        SetDefault();
    }

    public void SetDefault()
    {
        selectedTrack = null;
        //TODO GUI 관련 작업
    }
    
    public void SetSelectedTrack(string hashCode, JudgeLevel judgeLevel)
    {
        //TODO 입력값에 따라서 현재 selectedTrack을 바꾸고 UI 작업을 함
        SetJudgeLevel(judgeLevel);
    }

    public void SetJudgeLevel(JudgeLevel judgeLevel)
    {
        //TODO GUI 관련 작업을 함
    }
}
