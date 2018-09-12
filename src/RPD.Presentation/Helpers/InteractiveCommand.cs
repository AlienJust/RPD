using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace RPD.Presentation.Helpers {
	/// <summary>
	/// Passes EventArgs to command from binding
	/// https://stackoverflow.com/questions/13565484/how-can-i-pass-the-event-argument-to-a-command-using-triggers
	/// </summary>
	public class InteractiveCommand : TriggerAction<DependencyObject> {
		protected override void Invoke(object parameter) {
			if (AssociatedObject != null) {
				ICommand command = ResolveCommand();
				if (command != null && command.CanExecute(parameter)) {
					command.Execute(parameter);
				}
			}
		}

		private ICommand ResolveCommand() {
			if (Command != null)
				return Command;

			ICommand command = null;
			if (AssociatedObject != null) {
				foreach (PropertyInfo info in
						base.AssociatedObject.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)) {
					if (typeof(ICommand).IsAssignableFrom(info.PropertyType) &&
							string.Equals(info.Name, CommandName, StringComparison.Ordinal)) {
						command = (ICommand)info.GetValue(AssociatedObject, null);
					}
				}
			}
			return command;
		}

		private string _commandName;
		public string CommandName {
			get {
				ReadPreamble();
				return _commandName;
			}
			set {
				if (CommandName != value) {
					WritePreamble();
					_commandName = value;
					WritePostscript();
				}
			}
		}

		#region Command
		public ICommand Command {
			get => (ICommand)GetValue(CommandProperty);
			set => SetValue(CommandProperty, value);
		}

		// Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty CommandProperty =
				DependencyProperty.Register("Command", typeof(ICommand), typeof(InteractiveCommand), new UIPropertyMetadata(null));
		#endregion
	}
}
