extends Node

var id: String:
	get:
		return id
	set(value):
		id = value

var loc_name: String:
	get:
		return loc_name
	set(value):
		loc_name = value

var image_ref: String:
	get:
		return image_ref
	set(value):
		image_ref = value

var animation_refs: Array[String]:
	get:
		return animation_refs
	set(value):
		animation_refs = value


func _init(id: String, loc_name: String, image_ref: String, animation_refs: Array[String] = []) -> void:
	print("Location_Model: _init")
	self.id = id
	self.loc_name = loc_name
	self.image_ref = image_ref
	self.animation_refs = animation_refs
