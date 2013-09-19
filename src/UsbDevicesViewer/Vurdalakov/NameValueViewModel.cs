namespace Vurdalakov
{
    using System;

    public class NameValueViewModel : ViewModelBase
    {
        public NameValueViewModel(String name, Object value)
        {
            this.name = name;
            this.value = value;
        }
        
        private String name;
        public String Name
        {
            get
            {
                return this.name;
            }
            set
            {
                if (value != this.name)
                {
                    this.name = value;
                    OnPropertyChanged(() => Name);
                }
            }
        }

        private Object value;
        public Object Value
        {
            get
            {
                return this.value;
            }
            set
            {
                if (value != this.value)
                {
                    this.value = value;
                    OnPropertyChanged(() => Value);
                }
            }
        }
    }
}
