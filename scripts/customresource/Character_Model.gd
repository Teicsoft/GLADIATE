extends Node

var id: String:
	get:
		return id
	set(value):
		id = value

var char_name: String:
	get:
		return char_name
	set(value):
		char_name = value

var font_ref: String:
	get:
		return font_ref
	set(value):
		font_ref = value

var color_hex: String:
	get:
		return color_hex
	set(value):
		color_hex = value

var sprite_ref: String:
	get:
		return sprite_ref
	set(value):
		sprite_ref = value

var animation_refs: Array[String]:
	get:
		return animation_refs
	set(value):
		animation_refs = value

var default_animation: String:
	get:
		return default_animation
	set(value):
		default_animation = value

func _init(id: String, char_name: String, font_ref: String, color_hex: String, sprite_ref: String, default_animation: String, animation_refs: Array[String] = []):
	print("Character_Model: _init")
	self.id = id
	self.char_name = char_name
	self.font_ref = font_ref
	self.color_hex = color_hex
	self.sprite_ref = sprite_ref
	self.animation_refs = animation_refs
	self.default_animation = default_animation
