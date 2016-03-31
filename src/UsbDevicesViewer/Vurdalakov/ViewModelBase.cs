namespace Vurdalakov
{
    using System;
    using System.ComponentModel;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Threading;
    using System.Windows.Input;
    using System.Windows.Threading;

    public class ViewModelBase : INotifyPropertyChanged
    {
        public ViewModelBase()
        {
            this.OpenLinkCommand = new CommandBase<String>(this.OnOpenLinkCommand);
            this.SendEmailCommand = new CommandBase<String>(this.OnSendEmailCommand);
        }

        public String ApplicationTitle { get { return ((Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyTitleAttribute), false)) as AssemblyTitleAttribute).Title; } }
        public String ApplicationTitleAndVersion { get { return String.Format("{0} {1}", ApplicationTitle, ApplicationVersion); } }
        public String ApplicationVersion { get { var version = Assembly.GetExecutingAssembly().GetName().Version; return String.Format("{0}.{1:D2}", version.Major, version.Minor); } }
        public String ApplicationFullVersion { get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); } }
        public String ApplicationCopyright { get { return ((Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyCopyrightAttribute), false)) as AssemblyCopyrightAttribute).Copyright; } }

        public ICommand OpenLinkCommand { get; private set; }
        private void OnOpenLinkCommand(String url)
        {
            try
            {
                System.Diagnostics.Process.Start(url);
            }
            catch { }
        }

        public ICommand SendEmailCommand { get; private set; }
        private void OnSendEmailCommand(String email)
        {
            try
            {
                var subject = "";
                if (email.Contains("|"))
                {
                    var parts = email.Split('|');
                    email = parts[0];
                    subject = "?subject=" + parts[1];
                }
                const String prefix = "mailto:";
                System.Diagnostics.Process.Start((email.StartsWith(prefix, StringComparison.CurrentCultureIgnoreCase) ? email : prefix + email) + subject);
            }
            catch { }
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
