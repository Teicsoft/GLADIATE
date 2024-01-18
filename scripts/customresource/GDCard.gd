#class_name GDCard
#extends Resource
#
#enum Target {SELF, SINGLE_ENEMY, ALL_ENEMIES, EVERYONE}
#
#@export_group("Card Attributes")
#
##internal ids
#@export var id: String
#@export var target: Target
#
##stat block
#@export var attack: int
#@export var defense: int
#@export var heal: int
#@export var status: int
#@export var special: int
#@export var active: int
#
#
#@export_group("Card Visuals")
#
#@export var cardimage: Texture
#@export_multiline var tooltip_text: String
#@export var sound: AudioStream
