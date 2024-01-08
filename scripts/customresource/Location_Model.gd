extends Node

var id
var loc_name
var image_ref
var animation_refs


func _init(id: String, loc_name: String, image_ref: String, animation_refs: Array[String] = []) -> void:
    self.id = id
    self.loc_name = loc_name
    self.image_ref = image_ref
    self.animation_refs = animation_refs
    
func get( property: StringName, ) -> Variant:
    match property:
        "id":
            return id
        "loc_name":
            return loc_name
        "image_ref":
            return image_ref
        "animation_refs":
            return animation_refs
        _:
            return null
    
func set( property: StringName, value: Variant ) -> void:
    match property:
        "id":
            id = value
        "loc_name":
            loc_name = value
        "image_ref":
            image_ref = value
        "animation_refs":
            animation_refs = value
        _:
            pass