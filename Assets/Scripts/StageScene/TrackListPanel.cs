using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class TrackListPanel : MonoBehaviour
{
    private int selectedTrackIndex;
    private bool hasReloaded = false;

    [SerializeField] private Transform contentTransform;
    [SerializeField] private GameObject trackListPanelItemPrefab;
    private List<TrackListPanelItem> trackItemList = new List<TrackListPanelItem>();

    public bool HasReloaded { get => hasReloaded; }
    public BeatMap SelectedTrack()
    {
        if (trackItemList == null || IsTrackListEmpty()) return null;
        return trackItemList[selectedTrackIndex]?.BeatMap;
    }
    
    private void Start()
    {
        ReloadPanel();
    }

    public void ReloadPanel()
    {
        hasReloaded = false;

        foreach (var gameObject in trackItemList)
        {
            Destroy(gameObject);
        }
        trackItemList.Clear();

        foreach (BeatMap beatMap in BeatMapManager.Instance.BeatMaps)
        {
            GameObject trackListPanelItemGameObject = Instantiate(trackListPanelItemPrefab, contentTransform, false);
            TrackListPanelItem trackListPanelItem = trackListPanelItemGameObject.GetComponent<TrackListPanelItem>(); //trackListPanelItemPrefab는 반드시 TrackListPanelItem을 갖고 있음이 보장 된다
            trackListPanelItem.Init(beatMap);

            trackItemList.Add(trackListPanelItem);
        }

        MoveSelection(0, clearPrevious : false); // Track List을 띄운 후 맨 앞에 있는 것을 선택함

        hasReloaded = true;
    }

    public void MoveSelection(int index, bool clearPrevious = true)
    {
        if ((index == selectedTrackIndex && clearPrevious) || IsTrackListEmpty()) { return; }

        if (index >= 0 && index <= (trackItemList.Count - 1))
        {
            if (clearPrevious) { trackItemList[selectedTrackIndex].SetDefault(); }
            selectedTrackIndex = index;
            trackItemList[selectedTrackIndex].SetSelected();
        }
    }
    
    public void MovePreviousSelection() {
        if (IsTrackListEmpty()) { return; }

        int previousIndex = selectedTrackIndex == 0 ? trackItemList.Count - 1 : (selectedTrackIndex - 1) % trackItemList.Count;
        MoveSelection(previousIndex);
    }
    public void MoveNextSelection() {
        if (IsTrackListEmpty()) { return; }

        int nextIndex = (selectedTrackIndex + 1) % trackItemList.Count;
        MoveSelection(nextIndex);
    }
    
    public bool IsTrackListEmpty() { return trackItemList.Count == 0;  }
}
