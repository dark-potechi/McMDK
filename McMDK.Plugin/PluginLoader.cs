using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Windows;
using System.Windows.Controls;

using McMDK.Utils;
using McMDK.Utils.Log;

using McMDK.Plugin.Gui;
using McMDK.Plugin.Gui.Controls;

namespace McMDK.Plugin
{
    public class PluginLoader
    {
        private static List<Plugin> Plugins = new List<Plugin>();

        public static void Load()
        {
            //Load plugins
            string[] plugins = FileController.LoadDirectory(Define.PluginDirectory, true);
            foreach(string plugin in plugins)
            {
                Define.GetLogger().Fine("Loading Plugin from " + plugin);
                if(plugin.EndsWith("template"))
                {
                    Define.GetLogger().Fine("Skip loading plugin of " + plugin);
                    continue;
                }
                if(!FileController.Exists(plugin + "\\plugin.xml"))
                {
                    //Not exist
                    continue;
                }
                //Load root
                var a = from b in XElement.Load(plugin + "\\plugin.xml").Elements()
                        select new McMDK.Plugin.Plugin
                        {
                            Name = b.Element("Name").Value,
                            PluginID = b.Element("PluginID").Value,
                            Author = b.Element("Author").Value,
                            Version = b.Element("Version").Value,
                            Dependents = b.Element("Dependents").Value,
                            Support = b.Element("Support").Value
                        };
                Plugin p = null;

                foreach(var item in a)
                {
                    p = item;
                }

                p.Logger = new Logger(p.Name);
                p.Logger.SetParent(Define.GetLogger());
                p.Logger.Fine("Initializing...");
                p.Initialize();
                p.Logger.Fine("Initialized.");

                p.Logger.Fine("Loading UI Settings...");
                SerializeXML(p, plugin);
                p.Logger.Fine("Loaded UI Settings.");

                //Load Builder 
                p.Logger.Fine("Loading Builder Settings...");
                //
                p.Logger.Fine("Loaded Builder Settings.");

                Define.GetLogger().Fine(p.Name + " is loaded.");

                Plugins.Add(p);
            }
        }

        public static List<Plugin> GetPlugins()
        {
            return Plugins;
        }

        private static void SerializeXML(Plugin plugin, string dir)
        {
            if(!FileController.Exists(dir + "\\ui.xml"))
            {
                return;
            }

            try
            {
                XmlDocument document = new XmlDocument();
                document.Load(dir + "\\ui.xml");

                XmlNode root = document.DocumentElement;
                XmlNode node = root.ChildNodes[0];

                UIControl control = new UIControl();
                control.Component = (GuiComponents)Enum.Parse(typeof(GuiComponents), ((XmlElement)node).Name);

                //再帰的処理
                RecursiveSerializeXML(node, control);

                plugin.Controls.Add(control);
            }
            catch (Exception e)
            {
                plugin.Logger.Error("Load failed.", e);
            }
        }

        private static void RecursiveSerializeXML(XmlNode parentNode, UIControl parentControl)
        {
            foreach(XmlNode node in parentNode.ChildNodes)
            {
                UIControl control = new UIControl();
                
                switch(node.Name)
                {
                    case "TextBlock":
                    case "TextBox":
                    case "CheckBox":
                    case "ComboBox":
                        var textcontrol = new TextControl();
                        textcontrol.FontSize = StringToObjectConverter.StringTo<double>(((XmlElement)node).GetAttribute("FontSize"));
                        textcontrol.FontStretch = (FontStretch)StringToObjectConverter.StringToProperty(((XmlElement)node).GetAttribute("FontStretch"), typeof(FontStretches), FontStretches.Normal);
                        textcontrol.FontStyle = (FontStyle)StringToObjectConverter.StringToProperty(((XmlElement)node).GetAttribute("FontStyle"), typeof(FontStyles), FontStyles.Normal);
                        textcontrol.FontWeight = (FontWeight)StringToObjectConverter.StringToProperty(((XmlElement)node).GetAttribute("FontWeight"), typeof(FontWeights), FontWeights.Normal);
                        control = textcontrol;
                        break;

                    case "Image":
                        var imagecontrol = new ImageControl();
                        imagecontrol.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(((XmlElement)node).GetAttribute("ImageSource")));
                        break;

                    default:
                        control = new UIControl();
                        break;
                }
                control.Background = StringToObjectConverter.StringToBrush(((XmlElement)node).GetAttribute("Background"), new System.Windows.Media.SolidColorBrush(new System.Windows.Media.Color() { A = (byte)0xFF, R = (byte)0xFF, G = (byte)0xFF, B = (byte)0xFF }));
                control.Component = StringToObjectConverter.StringToComponents(((XmlElement)node).Name);
                control.Foreground = StringToObjectConverter.StringToBrush(((XmlElement)node).GetAttribute("Foreground"), new System.Windows.Media.SolidColorBrush(new System.Windows.Media.Color() { A = (byte)0xFF, R = (byte)0x00, G = (byte)0x00, B = (byte)0x00 }));
                control.Height = StringToObjectConverter.StringTo<double>(((XmlElement)node).GetAttribute("Height"));
                control.HorizontalAlignment = (HorizontalAlignment?)StringToObjectConverter.StringToProperty(((XmlElement)node).GetAttribute("HorizontalAlignment"), typeof(HorizontalAlignment));
                control.IsEnabled = StringToObjectConverter.StringTo<bool>(((XmlElement)node).GetAttribute("IsEnabled"));
                control.IsVisible = StringToObjectConverter.StringTo<bool>(((XmlElement)node).GetAttribute("IsVisible"));
                //control.Margin =
                control.Name = (((XmlElement)node).GetAttribute("Name"));
                control.Opacity = StringToObjectConverter.StringTo<double>(((XmlElement)node).GetAttribute("Opacity"));
                control.ToolTip = (((XmlElement)node).GetAttribute("ToolTip"));
                control.VerticalAlignment = (VerticalAlignment?)StringToObjectConverter.StringToProperty(((XmlElement)node).GetAttribute("VerticalAlignment"), typeof(VerticalAlignment));
                control.Visibility = (Visibility?)StringToObjectConverter.StringToEnum(((XmlElement)node).GetAttribute("Visibility"), typeof(Visibility));
                control.Width = StringToObjectConverter.StringTo<double>(((XmlElement)node).GetAttribute("Width"));
                parentControl.Children.Add(control);
                RecursiveSerializeXML(node, control);
            }
        }
    }
}
