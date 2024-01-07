extends Control


# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass


func _on_start_game_button_pressed():
	get_tree().change_scene_to_file("res://scenes/Playmat.tscn")


func _on_dialogue_button_pressed():
	pass # Replace with function body.


func _on_settings_button_pressed():
	pass # Replace with function body.


func _on_exit_button_pressed():
	get_tree().quit() 

