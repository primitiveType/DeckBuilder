function getValidTargets (cardId)
  return game.GetEnemyIds()--Basic attack that considers any enemy a valid target 
end

function playCard(cardId, target)
  game.DamageTarget(target, 5)
end

  function onDamageDealt(cardId, target, totalDamage, healthDamage)
      
  end

  function onThisCardPlayed (cardId)
      game.SendToDiscard(cardId)
  end