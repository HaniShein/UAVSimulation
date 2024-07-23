using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    

    public class CommandsReader
    {
        private static LinkedList<SimCommand> _commandsList = new LinkedList<SimCommand>();

        public CommandsReader()
        {
        }

        public static LinkedList<SimCommand> Read()
        {
            string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent + "\\SimCmds.txt";
            string[] fileLines = System.IO.File.ReadAllLines(path);

            foreach (string line in fileLines)
            {
                string[] a = line.Split('\t');
                SimCommand newCmd = new SimCommand()
                {
                    Time = float.Parse(line.Split('\t')[0]),
                    Num = int.Parse(line.Split('\t')[1]),
                    X = float.Parse(line.Split('\t')[2]),
                    Y = float.Parse(line.Split('\t')[3])
                };

                AddCommand(newCmd);
            }
            
            return _commandsList;
        }

        private static void AddCommand(SimCommand newCmd)
        {
            if (_commandsList.Count == 0)
            {
                _commandsList.AddFirst(newCmd);
            }
            else
            {
                foreach (SimCommand item in _commandsList)
                {
                    if(item.Time > newCmd.Time)
                    {
                        LinkedListNode<SimCommand> lastTimeNode = _commandsList.Find(item);
                        if (lastTimeNode?.Previous == null)
                        {
                            _commandsList.AddBefore(lastTimeNode, newCmd);
                        }
                        else 
                        { 
                            _commandsList.AddAfter(lastTimeNode.Previous, newCmd); 
                        }
                        break;
                    }
                }

                if(!_commandsList.Contains(newCmd))
                {
                    _commandsList.AddLast(newCmd);
                }
            }
        }
    }
}
