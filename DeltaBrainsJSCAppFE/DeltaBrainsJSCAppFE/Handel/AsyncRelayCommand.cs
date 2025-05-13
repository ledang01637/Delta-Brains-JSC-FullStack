using System;
using System.Threading.Tasks;
using System.Windows.Input;

public class AsyncRelayCommand<T> : ICommand
{
    private readonly Func<T, Task> _execute;
    private readonly Predicate<T> _canExecute;

    public AsyncRelayCommand(Func<T, Task> execute, Predicate<T> canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute ?? (_ => true);
    }

    public bool CanExecute(object parameter) => _canExecute((T)parameter);

    public async void Execute(object parameter)
    {
        await _execute((T)parameter);
    }

    public event EventHandler CanExecuteChanged;
    public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}
