extends PanelContainer


# Called when the node enters the scene tree for the first time.
func _ready():
	print("_ready")
	read_dialogue_XMl("dialogue_example")


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass


func read_dialogue_XMl(file: String):
	print("read_dialogue_XMl")
	var parser = XMLParser.new()
	var dialogue_path = "res://assets/writing/dialogue/"
	parser.open(dialogue_path + file + ".xml")
	
	while parser.read() != ERR_FILE_EOF:
		if parser.get_node_type() == XMLParser.NODE_ELEMENT:
			var node_name = parser.get_node_name()
			var attributes_dict = {}
			for idx in range(parser.get_attribute_count()):
				attributes_dict[parser.get_attribute_name(idx)] = parser.get_attribute_value(idx)
			print("The ", node_name, " element has the following attributes: ", attributes_dict)