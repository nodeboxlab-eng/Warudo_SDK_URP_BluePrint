using Warudo.Core.Events;

public class UIToggleValueChangedEvent : Event
{
    public string CanvasAssetId { get; set; }
    public string ElementName { get; set; }
    public bool IsOn { get; set; }
}
