using ProjekatB.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjekatB.Model
{
    public class Category : ViewModelBase
    {
        public string categoryName;
       

        public Category (string text)
        {
            CategoryName = text;
        }
        public Category() { }

        public string CategoryName
        {
            get { return categoryName; }
            set
            {
                categoryName = value;
                onPropertyChanged("category");
            }
        }

    }
}
