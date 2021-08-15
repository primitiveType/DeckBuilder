function getValidTargets (cardId)
  return game.GetEnemyIds()--Basic attack that considers any enemy a valid target 
end

function playCard(cardId, target)
  game.DamageTarget(target, 10)
end

function onThisCardPlayed (cardId)
      game.SendToExhaust(cardId)
end