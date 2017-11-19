using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DWClient
{
    public class ListBoxItem
    {
        public string Text { get; set; }
        public object Value { get; set; }

        public ListBoxItem() { }

        public ListBoxItem(string text, object value)
        {
            Text = text;
            Value = value;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
