using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace McMDK.Plugin.Gui.Controls
{
    public class StringToObjectConverter
    {
        public static GuiComponents StringToComponents(string obj)
        {
            GuiComponents component = GuiComponents.Null;
            try
            {
                component = (GuiComponents)Enum.Parse(typeof(GuiComponents), obj);
            }
            catch (Exception)
            {
                component = GuiComponents.Null;
            }
            return component;
        }

        public static Type StringTo<Type>(string obj) where Type : struct
        {
            var converter = TypeDescriptor.GetConverter(typeof(Type));
            if(converter != null)
            {
                return (Type)converter.ConvertFromString(obj);
            }
            return default(Type);
        }

        public static object StringToEnum(string obj, Type type)
        {
            try
            {
                return Enum.Parse(type, obj);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static object StringToProperty(string obj, Type type)
        {
            try
            {
                Type t = default(Type);
                PropertyInfo info = type.GetType().GetProperty(obj);
                return (Type)info.GetValue(t);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static Brush StringToBrush(string obj)
        {
            Brush brush = Brushes.White;
            try
            {
                PropertyInfo info = typeof(Brushes).GetProperty(obj);
                brush = (Brush)info.GetValue(brush);
            }
            catch (ArgumentNullException)
            {
                Color color = new Color();
                if(obj.StartsWith("#"))
                {
                    if(obj.Length == 9)
                    {
                        color.A = (byte)Convert.ToInt32(obj.Substring(1, 2), 16);
                        color.R = (byte)Convert.ToInt32(obj.Substring(3, 2), 16);
                        color.G = (byte)Convert.ToInt32(obj.Substring(5, 2), 16);
                        color.B = (byte)Convert.ToInt32(obj.Substring(7, 2), 16);
                    }
                    else if(obj.Length == 7)
                    {
                        color.R = (byte)Convert.ToInt32(obj.Substring(1, 2), 16);
                        color.G = (byte)Convert.ToInt32(obj.Substring(3, 2), 16);
                        color.B = (byte)Convert.ToInt32(obj.Substring(5, 2), 16);
                    }
                    else
                    {
                        color.R = (byte)Convert.ToInt32("00", 16);
                        color.G = (byte)Convert.ToInt32("00", 16);
                        color.B = (byte)Convert.ToInt32("00", 16);
                    }
                }
                
                brush = new SolidColorBrush(color);
            }
            catch (AmbiguousMatchException)
            {
                brush = Brushes.White;
            }
            return brush;
        }
    }
}
