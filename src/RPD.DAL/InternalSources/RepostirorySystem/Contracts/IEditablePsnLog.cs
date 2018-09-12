namespace RPD.DAL.RepostirorySystem.Contracts {
	interface IEditablePsnLog {
		void SetSavedToRepository(bool isSaved);
		void SetOwnerSection(ISection section);
	}
}