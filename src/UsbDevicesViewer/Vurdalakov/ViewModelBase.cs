namespace Vurdalakov
{
    using System;
    using System.ComponentModel;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Threading;
    using System.Windows;
    using System.Windows.Threading;

    public class ViewModelBase : INotifyPropertyChanged
    {
        public ViewModelBase()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> propertyExpresssion)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChangedEventArgs e = new PropertyChangedEventArgs(this.GetPropertyName(propertyExpresssion));
                this.InvokeIfRequired(() => this.PropertyChanged(this, e));
            }
        }

        private String GetPropertyName<T>(Expression<Func<T>> propertyExpresssion)
        {
            if (null == propertyExpresssion)
            {
                throw new ArgumentNullException("propertyExpression");
            }

            MemberExpression memberExpression = propertyExpresssion.Body as MemberExpression;
            if (null == memberExpression)
            {
                throw new ArgumentException("The expression is not a member access expression.", "propertyExpression");
            }

            PropertyInfo property = memberExpression.Member as PropertyInfo;
            if (null == property)
            {
                throw new ArgumentException("The member access expression does not access a property.", "propertyExpression");
            }

            MethodInfo methodInfo = property.GetGetMethod(true);
            if (methodInfo.IsStatic)
            {
                throw new ArgumentException("The referenced property is a static property.", "propertyExpression");
            }

            return memberExpression.Member.Name;
        }

        private readonly Dispatcher dispatcher = Dispatcher.CurrentDispatcher;

        protected void InvokeIfRequired(Action action)
        {
            if (this.dispatcher.Thread != Thread.CurrentThread)
            {
                this.dispatcher.Invoke(DispatcherPriority.DataBind, action);
            }
            else
            {
                action();
            }
        }
    }
}
