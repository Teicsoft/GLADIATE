extends Node

var id
var characters_present
var locations_present
var dialogue_script: Array[Shot]

class Shot:
    var id
    var lines: Array[Line]
    var options: Array[Options]
    
    class Line:
        var id: String
        var text
        var override_pos
    
    class Options:
        var text: String
        var next_shot_id: String
        var end_dialogue: bool
    
        func execute():
            if end_dialogue:
                return null
            else:
                return next_shot_id
    
    func run_shot():
        for line in lines:
            print(line.text)
            if line.override_pos:
                pass
            else:
                pass
                
        for option in options:
            print(option.text)
            return option.execute()
        
func run_dialogue():
    var current_shot = dialogue_script[0]
    current_shot.run_shot()