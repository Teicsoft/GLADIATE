using Godot;
using System;
using TeicsoftSpectacleCards.scripts.audio;
using TeicsoftSpectacleCards.scripts.autoloads;

public partial class DeckSelect : Control
{
    private AudioEngine audioEngine;
    public override void _Ready()
    {
        audioEngine = GetNode<AudioEngine>("/root/audio_engine");
    }


    public void handleClickEvent(InputEvent @event, string deckId)
    {
        if (@event is InputEventMouseButton mouseButton)
        {
            if (mouseButton.Pressed)
            {
                var sceneLoader = GetNode<SceneLoader>("/root/scene_loader");
                
                sceneLoader.DeckSelected = deckId;
                audioEngine.PlayMusic("venividivichy.wav");
                sceneLoader.GoToNextBattle();
            }
        }
    }
    
    private void OnDeck1GuiInput(InputEvent @event) { handleClickEvent(@event, "deck_Player1"); }
    private void OnDeck2GuiInput(InputEvent @event) { handleClickEvent(@event, "deck_Player2"); }
    private void OnDeck3GuiInput(InputEvent @event) { handleClickEvent(@event, "deck_Player3"); }
    private void OnDeck4GuiInput(InputEvent @event) { handleClickEvent(@event, "deck_Player4"); }
    private void OnDeck5GuiInput(InputEvent @event) { handleClickEvent(@event, "deck_Player5"); }
    private void OnDeck6GuiInput(InputEvent @event) { handleClickEvent(@event, "deck_Player6"); }
}



