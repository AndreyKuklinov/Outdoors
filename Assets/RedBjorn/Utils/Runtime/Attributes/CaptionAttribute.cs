using System;

namespace RedBjorn.Utils
{
    public class CaptionAttribute : Attribute
    {
        public string Text;

        public CaptionAttribute(string text)
        {
            Text = text;
        }
    }
}