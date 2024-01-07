extends Node
# A Node used to change a scene

@export var scene_folder = "scenes"

func change_to_scene(scene_name):
	var f = scene_folder if scene_folder != "" else ""
	get_tree().change_scene_to_file("res://" + f + "/" + scene_name + ".tscn")
