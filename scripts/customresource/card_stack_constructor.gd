#class_name CardStackConstructor
#extends Button
#
#@export var counter: Label
#@export var card_stack: CardStack : set = set_card_stack
#
#func set_card_stack(new_value: CardStack):
#	card_stack = new_value
#	
#	if not card_stack.card_stack_size_change.is_connected(_on_card_stack_size_changed):
#		card_stack.card_stack_size_changed.connect(_on_card_stack_size_changed)
#		_on_card_stack_size_changed(card_stack.cards.size())
#	
#func _on_card_stack_size_changed(cards_amount: int):
#	counter.text = str(cards_amount)
