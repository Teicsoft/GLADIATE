extends Node

var id
var char_name
var font_ref
var color_hex
var sprite_ref
var animation_refs
var default_animation

func _init(id: String, char_name: String, font_reference: String, color_hex: String, sprite_ref: String, animation_refs: Array[String] = [], default_animation: String ):
    self.id = id
    self.char_name = char_name
    self.font_reference = font_reference
    self.color_hex = color_hex
    self.sprite_ref = sprite_ref
    self.animation_refs = animation_refs
    self.default_animation = default_animation
    
func _get( property: StringName, ) -> Variant:
    match property:
        "id":
            return id
        "char_name":
            return char_name
        "font_reference":
            return font_reference
        "color_hex":
            return color_hex
        "sprite_reference":
            return sprite_ref
        "animations":
            return animation_refs
        "default_animation":
            return default_animation
        _:
            return null

func _set( property: StringName, value: Variant ) -> void:
    match property:
        "id":
            id = value
        "char_name":
            char_name = value
        "font_reference":
            font_reference = value
        "color_hex":
            color_hex = value
        "sprite_ref":
            sprite_ref = value
        "animations":
            animation_refs = value
        "default_animation":
            default_animation = value
        _:
            pass