using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LY.EMIS5.Admin.Models
{
    public class Flow
    {
         public List<Node> nodes { get; set; }
         public List<Action> actions { get; set; }

        public void setDic() {
            this.nodes.ForEach(c => {
                if (c.forms != null && c.forms.Count > 0)
                {
                    var dic = new Dictionary<string, string>();
                    c.forms.ForEach(f => dic.Add(f.name, f.type));
                }
            });
        }

        public Node getNode(int id) {
            return nodes.First(c => c.id == id);
        }

        public Node src { get; set; }

        public Node dest { get; set; }

        public void setAction(Action action=null) {
            if (action == null) {
                action = actions.First();
            }
            src = getNode(action.src);
            dest = getNode(action.dest);
        }
    }

    public class Node {
        public int id { get; set; }

        public string name { get; set; }

        public string type { get; set; }

        public string role { get; set; }

        public string person { get; set; }

        public List<Form> forms { get; set; }

        public Dictionary<string,string> formsDictionary { get; set; }
    }
    public class Action {
        public int src { get; set; }
        public int dest { get; set; }
    }

    public class Form {
        public string name { get; set; }
        public string type { get; set; }

    }
}