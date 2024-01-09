extends PanelContainer

var Dialogue_Model

func _init():
	print("dialogue_display: _init")

	Dialogue_Model = load("res://scripts/customresource/Dialogue_Model.gd")
	var model_node = Node.new()
	model_node.set_script(Dialogue_Model)


# Called when the node enters the scene tree for the first time.
func _ready():
	print("dialogue_display: _ready")
	pass
	
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
	
