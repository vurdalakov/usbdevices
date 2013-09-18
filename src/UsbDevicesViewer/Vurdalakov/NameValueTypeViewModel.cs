namespace Vurdalakov
{
    using System;

    public class NameValueTypeViewModel : NameValueViewModel
    {
        public NameValueTypeViewModel(String name, String value, String type) : base(name, value)
        {
            this.type = type;
        }

        private String type;
        public String Type
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
