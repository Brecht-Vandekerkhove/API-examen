using System.Windows.Input;

namespace API_examen.ViewModel.Commands
{
    internal class RelayCommand : ICommand
    {
        private readonly Action<object>? _action;

        public RelayCommand(Action<object>? action) => _action = action;

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter) => _action?.Invoke(parameter ?? "foutmelding");
    }
}
