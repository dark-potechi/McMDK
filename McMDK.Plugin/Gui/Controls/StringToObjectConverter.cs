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
            if(!String.IsNullOrEmpty(obj))
            {
                var converter = TypeDescriptor.GetConverter(typeof(Type));
                if (converter != null)
                {
                    return (Type)converter.ConvertFromString(obj);
                }
            }
            return default(Type);
        }

        public static object StringToEnum(string obj, Type type)
        {
            return StringToObjectConverter.StringToEnum(obj, type, null);
        }

        public static object StringToEnum(string obj, Type type, object def)
        {
            try
            {
                return Enum.Parse(type, obj);
            }
            catch (Exception)
            {
                return def;
            }
        }

        public static object StringToProperty(string obj, Type type)
        {
            return StringToObjectConverter.StringToProperty(obj, type, null);
        }

        public static object StringToProperty(string obj, Type type, object def)
        {
            try
            {
                Type t = default(Type);
                PropertyInfo info = type.GetType().GetProperty(obj);
                return (Type)info.GetValue(t);
            }
            catch (Exception)
            {
                return def;
            }
        }

        public static Brush StringToBrush(string obj)
        {
            return StringToObjectConverter.StringToBrush(obj, null);
        }

        public static Brush StringToBrush(string obj, Brush def)
        {
            try
            {
                if (String.IsNullOrEmpty(obj))
                {
                    return def;
                }
                if (obj.StartsWith("#"))
                {
                    Color color = new Color();
                    if (obj.Length == 9)
                    {
                        color.A = (byte)Convert.ToInt32(obj.Substring(1, 2), 16);
                        color.R = (byte)Convert.ToInt32(obj.Substring(3, 2), 16);
                        color.G = (byte)Convert.ToInt32(obj.Substring(5, 2), 16);
                        color.B = (byte)Convert.ToInt32(obj.Substring(7, 2), 16);
                    }
                    else if (obj.Length == 7)
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
                    return new SolidColorBrush(color);
                }
                Brush brush = null;
                PropertyInfo info = typeof(Brushes).GetProperty(obj);
                return (Brush)info.GetValue(brush);
            }
            catch (Exception)
            {
            }
            return def;
        }
    }
}
