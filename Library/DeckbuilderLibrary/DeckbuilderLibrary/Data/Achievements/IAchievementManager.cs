using System;
using System.Collections.Generic;
using System.Text;

namespace DeckbuilderLibrary.Data.Achievements {
	public interface IAchievementManager {

		void Init();

		void TriggerAchievement(string key);

	}
}
