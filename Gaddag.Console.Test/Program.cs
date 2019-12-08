using Gadag;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Gaddag.Console.Test
{
    class Program
    {
        static Dico Dico { get; set; }
        static void Main(string[] args)
        {
            CreateDico();
            Dico = new Dico();
            Dico.RootNode = LoadDico();
        }
        static Node LoadDico(string fileName = "Gaddag.dat")
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
                //Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }
            return rootNode;

        }
        private static void CreateDico(string fileName = "Gaddag.dat")
        {
            Dico Gad = new Dico();
            var assembly = Assembly.GetExecutingAssembly();
            string resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith("ods4.txt"));
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream, true))
            {
                string content = reader.ReadToEnd();
                string[] lignes = content.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (var l in lignes)
                {
                    Gad.Add(l);
                }
                // To serialize the hashtable and its key/value pairs,  
                // you must first open a stream for writing. 
                // In this case, use a file stream.
                using (FileStream fs = new FileStream(fileName, FileMode.Create))
                {

                    // Construct a BinaryFormatter and use it to serialize the data to the stream.
                    BinaryFormatter formatter = new BinaryFormatter();
                    try
                    {
                        formatter.Serialize(fs, Gad.RootNode);
                    }
                    catch (SerializationException e)
                    {
                        Debug.WriteLine("Failed to serialize. Reason: " + e.Message);
                        throw;
                    }
                    finally
                    {
                        fs.Close();
                    }
                }
            }
        }
    }
}
