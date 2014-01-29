using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace McMDK.Plugin.Gui.Controls
{
    public class TextControl : UIControl
    {
        public string Text { set; get; }

        public double FontSize { set; get; }

        public FontWeight FontWeight { set; get; }

        public FontFamily FontFamily { set; get; }

        public FontStretch FontStretch { set; get; }

        public FontStyle FontStyle { set; get; }

        public TextAlignment TextAlignment { set; get; }

        public TextDecoration TextDecoration { set; get; }

        public TextWrapping TextWrapping { set; get; }
    }
}
