extends TextureButton

var master_bus = AudioServer.get_bus_index("Master")



func _on_TextureButton_pressed():
    AudioServer.set_bus_mute(master_bus, not AudioServer.is_bus_mute(master_bus))
