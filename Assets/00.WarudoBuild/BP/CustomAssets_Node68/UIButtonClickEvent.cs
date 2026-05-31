using Warudo.Core.Events;

public class UIButtonClickEvent : Event {
    public string CanvasAssetId { get; set; }
    public string ElementName { get; set; }
}
