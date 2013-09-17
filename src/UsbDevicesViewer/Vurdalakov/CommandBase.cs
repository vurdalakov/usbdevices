namespace Vurdalakov
{
    using System;
    using System.Windows.Input;

    public class CommandBase : ICommand
    {
        Action actionExecute = null;

        Func<Boolean> functionCanExcute = null;

        public CommandBase(Action actionExecute) : this(actionExecute, null)
        {
        }

        public CommandBase(Action actionExecute, Func<Boolean> functionCanExcute)
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
            return (null == this.functionCanExcute) || this.functionCanExcute.Invoke();
        }

        public void Execute(Object parameter)
        {
            if (this.CanExecute(parameter))
            {
                this.actionExecute.Invoke();
            }
        }
    }
}
