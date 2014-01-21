using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using McMDK.Utils;

namespace McMDK.Source
{
    public class Parser
    {
        private string path;
        private string save;
        private string text;
        
        private List<Method> methods;
        private List<Field> fields;

        private readonly string MethodRegex = @"\[Method\((?<from>[0-9]{3})?,(?<to>[0-9]{3})?\),(?<method>.*)\]";
        private readonly string FieldRegex =  @"\[Field\((?<from>[0-9]{3})?,(?<to>[0-9]{3})?\),(?<action>Set|Get)\((?<p1>.*),(?<p2>.*)\)\]";

        public Parser(string path, string save)
        {
            this.path = path;
            this.save = save;
            this.methods = new List<Method>();
        }

        public void Parse()
        {
            if(!FileController.Exists(path))
            {
                throw new System.IO.FileNotFoundException(path);
            }
            text = FileController.LoadFile(path);

            //Method部分をサーチ
            Regex regex = new Regex(this.MethodRegex);
            MatchCollection matches = regex.Matches(text);
            foreach(Match match in matches)
            {
                string method = "";
                int from = 0, to = 999;
                try
                {
                    method = match.Groups["method"].Value;
                    from =   int.Parse(match.Groups["from"].Value);
                    to =     int.Parse(match.Groups["to"].Value);
                } catch (Exception)
                {
                    if(method.Equals(""))
                    {
                        continue;
                    }
                }
                this.methods.Add(new Method(method, from, to, match.Groups[0].Value));
            }

            regex = new Regex(this.FieldRegex);
            matches = regex.Matches(text);
            foreach(Match match in matches)
            {
                string action = "", p1 = "", p2 = "";
                int from = 0, to = 999;
                try
                {
                    action = match.Groups["action"].Value;
                    p1 = match.Groups["p1"].Value;
                    p2 = match.Groups["p2"].Value;
                    from = int.Parse(match.Groups["from"].Value);
                    to = int.Parse(match.Groups["to"].Value);
                } catch (Exception)
                {
                    if((!action.Equals("Set") | !action.Equals("Get")) | p1.Equals("") | p2.Equals(""))
                    {
                        continue;
                    }
                }
                this.fields.Add(new Field(p1, p2, from, to, action.Equals("Set") ? Action.SET : Action.GET, match.Groups[0].Value));
            }
        }

        public void Save(int version)
        {

        }

        private class Method
        {
            public string Body { private set; get; }
            public string Method { private set; get; }
            public int From { private set; get; }
            public int To { private set; get; }

            public Method(string m, int f, int t, string b)
            {
                this.Method = m;
                this.From = f;
                this.To = t;
                this.Body = b;
            }
        }

        private class Field
        {
            public string Body { private set; get; }
            public Action Action { private set; get; }
            public string P1 { private set; get; }
            public string P2 { private set; get; }
            public int From { private set; get; }
            public int To { private set; get; }

            public Field(string p1, string p2, int f, int t, Action a, string b)
            {
                this.P1 = p1;
                this.P2 = p2;
                this.From = f;
                this.To = t;
                this.Action = a;
                this.Body = b;
            }
        }

        private enum Action
        {
            SET, GET
        }
    }
}
