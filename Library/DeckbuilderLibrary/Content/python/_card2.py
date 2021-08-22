import enum
from abc import ABCMeta, abstractmethod

ID = int


class PileType(enum.Enum):
    # This should be a temporary enum; we can read this from C#
    DrawPile = enum.auto()
    HandPile = enum.auto()
    DiscardPile = enum.auto()
    ExhaustPile = enum.auto()


class Card(metaclass=ABCMeta):

    class Context:
        pass
        

    @property
    @abstractmethod
    def name(self) -> str:
        raise NotImplementedError

    @abstractmethod
    def get_valid_targets(self):
        raise NotImplementedError

    @property
    @abstractmethod
    def requires_target(self) -> bool:
        raise NotImplementedError

    @property
    @abstractmethod
    def owner(self):
        raise NotImplementedError

    # @abstractmethod
    # def on_card_created(self, card_id: ID):
    #     raise NotImplementedError

    # @abstractmethod
    # def on_card_moved(self, moved_card: ID, previous_pile_type: PileType, new_pile: PileType):
    #     raise NotImplementedError

    # @abstractmethod
    # def on_damage_dealt(self, actor_id: ID, health_damage: int, total_damage: int):
    #     pass


