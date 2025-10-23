using System;

[Serializable]
public class Note
{
    public readonly static int SHORT_NOTE = 0;
    public readonly static int LONG_NOTE = 1;

    private float beat;
    private int lane;
    private int type;
    private int length;

    /// <summary>
    /// @param beat 
    /// @param lane 
    /// @param type 
    /// @param length
    /// </summary>
    public Note(float beat, int lane, int type, int length)
    {
        this.beat = beat;
        this.lane = lane;
        this.type = type;
        this.length = length;
    }

    public float Beat { get => beat; }
    public int Lane { get => lane; }
    public int Type { get => type; }
    public int Length { get => length; }
}