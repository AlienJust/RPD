namespace RPD.DalRelease.Configuration.CommandConfiguration {
	class CommandConfigurationBuilderDefault : ICommandConfigurationBuilder {
		public ICommandConfiguration BuildConfiguration() {
			return new CommandConfigurationSimple(null,
			                                      false,
			                                      false,
			                                      false, 
															  false, 
															  false, 
															  false, 
															  false, 
															  false, 
															  false, 
															  false, 
															  false, 
															  null, 
															  0);
		}
	}
}