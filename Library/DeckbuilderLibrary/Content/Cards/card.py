ID = int


class PileType:
    pass



class Card:
    def on_card_created(self, card_id: ID):
        pass

    def on_card_moved(self, moved_card: ID, previous_pile_type: PileType, new_pile: PileType):
        pass
    
    def on_damage_dealt(self, actor_id: ID, health_damage: int, total_damage: int):
        pass
