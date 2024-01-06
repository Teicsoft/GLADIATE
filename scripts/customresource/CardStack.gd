class_name CardStack
extends Resource

#Number of cards in the stack
signal card_stack_size_change(card_number: int)

#Ideally filled with Card objects when finished
@export var cards : Array[int] = [1,2,3,4,5,6]

#Check if stack is empty yes/no
func empty():
	return cards.is_empty()

#Throw away all cards in stack
func clear_deck():
	cards.clear()
	card_stack_size_change.emit(cards.size())

#Draw the front object in the stack
func draw_card():
	var card = cards.pop_front()
	card_stack_size_change.emit(cards.size())
	return card

#add an object to the back of the stack
func add_card(newcard):
	cards.append(newcard)
	card_stack_size_change.emit(cards.size())

#This method uses the global random number generator common to methods such as @GlobalScope.randi.
func shuffle():
	cards.shuffle()
	#randomize()   --this randomizes the internal seed every shuffle, seems overkill

#returns an amount of cards from the top of the stack in the format "\n 1: (card)", as a string
func search(amount):
	var card_search: Array = []
	for i in range (card_search.size()):
		if i <= amount:
			card_search.push_back("%s: %s" % [i+1, cards[i]])
	return "\n".join(card_search)
