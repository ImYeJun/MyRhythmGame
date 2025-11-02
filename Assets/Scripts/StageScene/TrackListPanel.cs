using System.Collections.Generic;
using UnityEngine;

public class TrackListPanel : MonoBehaviour
{
    private int selectedTrackIndex;
    private List<string> trackHashCodes = new List<string>();
    private bool hasReloaded = false;

    //TODO UI 관련 속성 추가하기

    public bool HasReloaded { get => hasReloaded; }

    private void Start()
    {
        ReloadPanel();
    }

    public void ReloadPanel()
    {
        //TODO TrackManager에서 hashcode 가져오기

        hasReloaded = true;
    }

    public void MoveSelection(int index)
    {
        if (index == selectedTrackIndex) { return; }

        if (index >= 0 && index <= (trackHashCodes.Count - 1))
        {
            selectedTrackIndex = index;
            //TODO UI 작업
        }
    }
    
    public void MovePreviousSelection() {
        int previousIndex = (selectedTrackIndex - 1) % trackHashCodes.Count;
        MoveSelection(previousIndex);
    }
    public void MoveNextSelection() {
        int nextIndex = (selectedTrackIndex + 1) % trackHashCodes.Count;
        MoveSelection(nextIndex);
    }

    public string GetSelectedTrackHashCode()
    {
        if (trackHashCodes == null || IsTrackListEmpty() ) return null;
        return trackHashCodes[selectedTrackIndex];
    }
    
    public bool IsTrackListEmpty() { return trackHashCodes.Count == 0;  }
}
