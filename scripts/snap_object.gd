extends Node2D

# todo: set number of node positions based on hand size

var draggable = false
var is_inside_dropable = 0 
var body_ref
var prev_body_ref
var offset: Vector2
var initialPos: Vector2

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	if draggable:
		if Input.is_action_just_pressed("left_mouse"):
			initialPos = global_position
			offset = get_global_mouse_position() - global_position
			snap_global.is_dragging = true
			
		if Input.is_action_pressed("left_mouse"):
			global_position = get_global_mouse_position() - offset
			
		elif Input.is_action_just_released("left_mouse"):
			snap_global.is_dragging = false
			var tween = get_tree().create_tween()
			
			if is_inside_dropable > 0:
				var overlapping_bodies = $Area2D.get_overlapping_bodies()
				var lowestDistance
				print("test1")
				
				var pos_snap =  $Area2D.global_transform.origin
				var i = 0
				print(typeof(overlapping_bodies))
				for body in overlapping_bodies:
					i += 1
					if body.is_in_group('droppable'):
						var pos_overlap = body.global_position
						if lowestDistance == null:
							lowestDistance = pos_snap.distance_to(pos_overlap)
							body_ref = body
							print(lowestDistance)
						else:
							if pos_snap.distance_to(pos_overlap) < lowestDistance:
								lowestDistance = pos_snap.distance_to(pos_overlap)
								body_ref = body
				print(i)
				
			if is_inside_dropable > 0:
				tween.tween_property(self, "position", body_ref.global_position, 0.2).set_ease(Tween.EASE_OUT)
				print("pos")
			else:
				tween.tween_property(self, "global_position", initialPos, 0.2).set_ease(Tween.EASE_OUT)
				print("globalpos")


func _on_area_2d_mouse_entered():
	if not snap_global.is_dragging:
		draggable = true
		scale = Vector2(1.05, 1.05)


func _on_area_2d_mouse_exited():
	if not snap_global.is_dragging:
		draggable = false
		scale = Vector2(1,1)

func _on_area_2d_body_entered(body):
	if body.is_in_group('droppable'):
		is_inside_dropable +=1
		print("_on_area_2d_body_entered: " + str(is_inside_dropable))
		body.modulate = Color(Color.REBECCA_PURPLE, 1)
		#if body_ref != null:
			#prev_body_ref = body_ref
		#body_ref = body


func _on_area_2d_body_exited(body):
	if body.is_in_group('droppable'):
		is_inside_dropable -= 1 
		body.modulate = Color(Color.MEDIUM_PURPLE, 0.7)
		
		#if is_inside_dropable > 0:
			#body_ref = prev_body_ref
