from typing import Dict, Callable, Union, Tuple

import click


CardText = str
CSharpFunc = str
Effect = Tuple[CardText, CSharpFunc]
EffectMaker = Callable[[], Effect]
NamedFunction = Tuple[str, EffectMaker]
LookupTable = Dict[int, NamedFunction]


@click.group()
def cli():
    pass


continue_hint = "(Enter c to continue)"


def continue_() -> Effect:
    """ Sentinel representing a quit command. """
    return "", ""


def lookup(choice: Union[str, int], *, lookup_table: LookupTable):
    if str(choice).isnumeric():
        return lookup_table[int(choice)]
    return {named_func[0]: named_func for number, named_func in lookup_table.items()}[choice]


def name_to_number_hint(lookup_table: LookupTable) -> str:
    # "deal damage (1), gain block (2), draw cards (3), apply weak (4), apply vulnerable (5)"
    return ",".join([f"{named_func.name}, ({num})" for num, named_func in sorted(lookup_table)])


########################################################################################
# Target

def target_self() -> Effect:
    return "to yourself", """\
public override IReadOnlyList<Actor> GetValidTargets()
{
return null;
}

public override bool RequiresTarget => false;

"""


def target_all_enemies() -> Effect:
    return "", """\
public override IReadOnlyList<Actor> GetValidTargets()
{
return Context.GetEnemies();
}

public override bool RequiresTarget => true;

"""


lookup_target: LookupTable = {
    1: ("self", target_self),
    2: ("all enemies", target_all_enemies)
}


########################################################################################
# Attack

def create_attack():
    names_to_numbers = name_to_number_hint(lookup_attack_effect)
    effects = []
    effect_choice = input(f"Primary effect? \n{names_to_numbers}. {continue_hint}\n")
    while True:
        choice = lookup(effect_choice, lookup_table=lookup_attack_effect, names_to_numbers=names_to_numbers_attack_effects)
        if choice == continue_:
            break
        effect = choice()
        effects.append(effect)
        effect_choice = input(f"Additional effect?\n{names_to_numbers}. {continue_hint}\n")
    return effects


def apply_damage() -> Effect:
    # target = input("Target? ")
    pass


def apply_block() -> Effect:
    pass


def draw_cards() -> Effect:
    pass


def apply_weak() -> Effect:
    pass


def apply_vulnerable() -> Effect:
    pass


lookup_attack_effect = {
    1: ("apply damage", apply_damage),
    2: ("apply block", apply_block),
    3: ("draw cards", draw_cards),
    4: ("apply vulnerable", apply_vulnerable),
    5: ("apply weak", apply_weak),
}


########################################################################################
# Skill

def create_skill():
    pass


def create_power():
    pass
        


@click.command()
def new_card():
    click.echo("Welcome to the card builder! Fill in the prompts.\n")
    card_name = input("Card name? ")
    card_type = input("Card type? attack (1), skill (2), or power (3) ")
    card = lookup(card_type, lookup_table=card_type_generator, name_to_number=card_types)
    click.echo(f"You said {card_name}")



card_type_generator = {
    "attack": create_attack,
    "skill": create_skill,
    "power": create_power
}

card_types = {
    "attack": 1,
    "skill": 2,
    "power": 3
}


cli.add_command(new_card)





if __name__ == "__main__":
    cli()
