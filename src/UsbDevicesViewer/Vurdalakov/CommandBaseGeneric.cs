namespace Vurdalakov
{
    using System;
    using System.Windows.Input;

    public class CommandBase<T> : ICommand
    {
        Action<T> actionExecute = null;

        Func<T, Boolean> functionCanExcute = null;

        public CommandBase(Action<T> actionExecute) : this(actionExecute, null)
        {
        }

        public CommandBase(Action<T> actionExecute, Func<T, Boolean> functionCanExcute)
        {
            if (null == actionExecute)
            {
                throw new ArgumentNullException("actionExecute");
            }

            this.actionExecute = actionExecute;

            this.functionCanExcute = functionCanExcute;
        }

        public event EventHandler CanExecuteChanged;

        public Boolean CanExecute(Object parameter)
        {
            return (null == this.functionCanExcute) || this.functionCanExcute.Invoke((T)parameter);
        }

        public void Execute(Object parameter)
        {
            if (this.CanExecute(parameter))
            {
                this.actionExecute.Invoke((T)parameter);
            }
        }
    }
}
