baseDamage = 1
                                          
function getValidTargets (cardId)
  return game.GetEnemyIds()--Basic attack that considers any enemy a valid target 
end

function playCard(cardId, target)
  timesPlayed = game.GetInt(cardId, "TimesPlayed")
  damage =  timesPlayed + baseDamage
  game.DamageTarget(target, damage)
  game.SetInt(cardId, "TimesPlayed", timesPlayed + 1)

end



  function onThisCardPlayed (cardId)
        game.SendToExhaust(cardId)
  end
                                                   