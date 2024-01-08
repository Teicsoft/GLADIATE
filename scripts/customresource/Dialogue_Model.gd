extends Node

var characters_present = []
var locations_present = []
var script


func add_character(character):
    characters_present.append(character)
    
func add_location(location):
    locations_present.append(location)
    
func add_script(script):
    self.script = script