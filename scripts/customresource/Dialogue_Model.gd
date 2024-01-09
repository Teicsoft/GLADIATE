extends Node

var characters_present = []
var locations_present = []

var Dialogue_Script_Model
var Character_Model
var Location_Model

func _init():
	print("Dialogue_Model: _init")
	read_dialogue_XMl("dialogue_example")
	Dialogue_Script_Model = preload("res://scripts/customresource/Dialogue_Script_Model.gd")
	Character_Model = preload("res://scripts/customresource/Character_Model.gd")
	Location_Model = preload("res://scripts/customresource/Location_Model.gd")

func add_character(character):
	characters_present.append(character)
	
func add_location(location):
	locations_present.append(location)
	
func add_script(script):
	self.script = script

func read_dialogue_XMl(file: String):
	print()
	print("Dialogue_Model: read_dialogue_XMl")
	print()
	var parser = XMLParser.new()
	var dialogue_path = "res://assets/writing/dialogue/"
	var dialogue_file = dialogue_path + file + ".xml"
	parser.open(dialogue_file)
	
	var parsed_XML = XML.parse_file(dialogue_file)
	#print(addon_parser)
	
	var XMLDict: Dictionary
	XMLDict = parsed_XML.root.to_dict()
	#print(typeof(XMLDict))
	#print(XMLDict)
	
	print()#
	print (XMLDict["children"]["characters_present"]["children"])
	
	#for key in XMLDict["children"]["characters_present"]["children"]:
		#print (key["character"])
	
	print()#
	
	for child in XMLDict["children"]["characters_present"]["children"]:
		var parsed_Child = XML.parse_str(child)
		print(parsed_Child)
	
	
	
	#while parser.read() != ERR_FILE_EOF:
		#if parser.get_node_type() == XMLParser.NODE_ELEMENT and parser.get_node_name() == "text":
			#
			#
			#match parser.get_node_name():
				#"text":
					#print(parser.get_node_name())
					#print(typeof(parser.get_node_data()))
				#
				#
				#if parser.get_node_name() != "dialogue":
					#parser.skip_section()
					#print()


	#while parser.read() != ERR_FILE_EOF:
		#if parser.get_node_type() == XMLParser.NODE_ELEMENT:
			#var node_name = parser.get_node_name()
			#var attributes_dict = {}
			#for idx in range(parser.get_attribute_count()):
				#attributes_dict[parser.get_attribute_name(idx)] = parser.get_attribute_value(idx)
			#print("The ", node_name, " element has the following attributes: ", attributes_dict)
			
			
func _ready():
	print("Dialogue_Model: _ready")
