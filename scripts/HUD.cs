using Godot;
using System;

public partial class HUD : CanvasLayer
{
    private CanvasLayer canvasLayerNode;
    private Vector2 originalViewportSize;
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        canvasLayerNode = this;
        originalViewportSize = new Vector2(1920,1080); // Set the original viewport size (width, height
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        // Calculate the scale factor based on the ratio of the current viewport size to the original viewport size
        Vector2 scaleFactor = GetViewport().GetVisibleRect().Size / originalViewportSize;

        // Update CanvasLayer scale to match the viewport size
        canvasLayerNode.Scale = scaleFactor;
    }
}
