extends Control

func _on_start_game_button_pressed():
	#get_tree().change_scene_to_file("res://scenes/Playmat.tscn")
	scene_loader.scene_folder = "scenes/main"
	scene_loader.change_to_scene("Playmat")


func _on_dialogue_button_pressed():
	scene_loader.scene_folder = "scenes/main"
	scene_loader.change_to_scene("dialogue_display")


func _on_settings_button_pressed():
	pass # Replace with function body.


func _on_exit_button_pressed():
	get_tree().quit()

