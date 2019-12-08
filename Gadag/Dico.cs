using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Gaddag
{
    /*
    The MIT License (MIT)
    Copyright (c) 2013
 
    Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"),
    to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
    and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
 
    The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
 
    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
    WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

    [Serializable]
    public class Dico
    {
        public Node LoadDico(string fileName = "Gaddag.dat")
        {
            // Declare the hashtable reference.
            Node rootNode = null;
            // Open the file containing the data that you want to deserialize.
            FileStream fs = new FileStream(fileName, FileMode.Open);
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();

                // Deserialize the hashtable from the file and 
                // assign the reference to the local variable.
                rootNode = (Node)formatter.Deserialize(fs);
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }
            return rootNode;

        }
        public Node RootNode { get; set; }

        public Dico()
        {
            RootNode = new Node { Letter = Node.Root };
        }

        public void Add(string word)
        {
            var w = word.ToLower();
            var prevNode = new List<Node>();
            for (var i = 1; i <= w.Length; i++)
            {
                var prefix = w.Substring(0, i);
                var suffix = "";
                if (i != w.Length) suffix = w.Substring(i, w.Length - (i));
                var addword = _StringReverse(prefix) + Node.Break + suffix + Node.Eow;

                var currentNode = RootNode;
                var breakFound = false;
                int j = 0;
                foreach (var c in addword)
                {
                    //Long words can be joined back together after the break point, cutting the tree size in half.
                    if (breakFound && prevNode.Count > j)
                    {
                        currentNode.AddChild(c, prevNode[j]);
                        break;
                    }

                    currentNode = currentNode.AddChild(c);

                    if (prevNode.Count == j)
                        prevNode.Add(currentNode);

                    if (c == Node.Break)
                        breakFound = true;
                    j++;
                }
            }
        }


        private static string _GetWord(string str)
        {
            var word = "";

            for (var i = str.IndexOf(Node.Break) - 1; i >= 0; i--)
                word += str[i];

            for (var i = str.IndexOf(Node.Break) + 1; i < str.Length; i++)
                word += str[i];

            return word;
        }

        public List<string> ContainsHookWithRack(string hook, string rack)
        {
            hook = hook.ToLower();
            hook = _StringReverse(hook);

            var set = new HashSet<string>();

            _ContainsHookWithRackRecursive(RootNode, set, "", rack, hook);
            return set.ToList();
        }

        private static void _ContainsHookWithRackRecursive(Node node, ISet<string> rtn, string letters, string rack, string hook)
        {
            // Null nodes represent the EOW, so return the word.
            if (node == null)
            {
                var w = _GetWord(letters);
                if (!rtn.Contains(w)) rtn.Add(w);
                return;
            }

            // If the hook still contains letters, process those first.
            if (!String.IsNullOrWhiteSpace(hook))
            {
                letters += node.Letter != Node.Root ? node.Letter.ToString() : "";

                if (node.ContainsKey(hook[0]))
                {
                    var h = hook.Remove(0, 1); //Pop the letter off the hook
                    _ContainsHookWithRackRecursive(node[hook[0]], rtn, letters, rack, h);
                }
            }
            else
            {
                letters += node.Letter != Node.Root ? node.Letter.ToString() : "";

                foreach (var key in node.Keys.Cast<char>().Where(k => rack.Contains(k) || k == Node.Eow || k == Node.Break))
                {
                    var r = (key != Node.Eow && key != Node.Break) ? rack.Remove(rack.IndexOf(key), 1) : rack; //Pull the letter from the rack
                    _ContainsHookWithRackRecursive(node[key], rtn, letters, r, hook);
                }
            }
        }

        private static string _StringReverse(string str)
        {
            var charArray = str.ToCharArray();
            Array.Reverse(charArray);
            return (new String(charArray));
        }
    }
    [Serializable]
    public class Node
    {
        public const char Break = '>';
        public const char Eow = '$';
        public const char Root = ' ';

        public char Letter { get; set; }
        public HybridDictionary Children { get; private set; }

        public Node() { }

        public Node(char letter)
        {
            this.Letter = letter;
        }

        public Node this[char index]
        {
            get { return (Node)Children[index]; }
        }

        public ICollection Keys
        {
            get { return Children.Keys; }
        }

        public bool ContainsKey(char key)
        {
            return Children.Contains(key);
        }

        public Node AddChild(char letter)
        {
            if (Children == null)
                Children = new HybridDictionary();

            if (!Children.Contains(letter))
            {
                var node = letter != Eow ? new Node(letter) : null;
                Children.Add(letter, node);
                return node;
            }

            return (Node)Children[letter];
        }

        public Node AddChild(char letter, Node node)
        {
            if (Children == null)
                Children = new HybridDictionary();

            if (!Children.Contains(letter))
            {
                Children.Add(letter, node);
                return node;
            }

            return (Node)Children[letter];
        }

        public override string ToString()
        {
            return this.Letter.ToString();
        }
    }
}
