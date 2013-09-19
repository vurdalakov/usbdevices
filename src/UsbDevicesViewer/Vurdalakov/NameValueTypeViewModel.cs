namespace Vurdalakov
{
    using System;

    public class NameValueTypeViewModel : NameValueViewModel
    {
        public NameValueTypeViewModel(String name, Object value, Object type) : base(name, value)
        {
            this.type = type;
        }

        private Object type;
        public Object Type
        {
            get
            {
                return this.type;
            }
            set
            {
                if (value != this.type)
                {
                    this.type = value;
                    OnPropertyChanged(() => Type);
                }
            }
        }
    }
}
