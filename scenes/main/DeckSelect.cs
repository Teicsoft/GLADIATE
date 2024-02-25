using GLADIATE.scenes.sub;
using Godot;
using GLADIATE.scripts.audio;

public partial class DeckSelect : Control
{
    private AudioEngine audioEngine;
    public override void _Ready()
    {
        audioEngine = GetNode<AudioEngine>("/root/audio_engine");
        saveData.ParseJson();
        for (int i = 1; i <= 6; i++) { ReadHighScore(i); }
    }

    public void ReadHighScore(int deckNumber)
    {
        var highScore = saveData.GetHighScoreByDeck("deck_Player" + deckNumber);
        if (highScore == null) { return; }

        Label label = GetNode<Label>("TextureRect/HBoxContainer/Deck" + deckNumber + "/High Score");
        label.Text = "High Score: " + highScore.Score;
        label.Show();
        
        GetNode<TextureRect>($"TextureRect/HBoxContainer/Deck{deckNumber}/Thumbsup").Show();
    }

    public void handleClickEvent(InputEvent @event, string deckId)
    {
        if (@event is InputEventMouseButton mouseButton)
        {
            if (mouseButton.Pressed)
            {
                var sceneLoader = GetNode<GLADIATE.scripts.autoloads.SceneLoader>("/root/SceneLoader");
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



