using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JSON;
using System.IO;

namespace OOP_JSON
{
    class Program
    {
        static bool printToFile = false;
        static string fileNameLocation;
        static void Main(string[] args)
        {
            TextReader aReader = null;
            if (Console.IsInputRedirected)
            {
            
                if (args.Length > 1)
                {
                    if (args[0].ToLower() == "file")
                    {
                        printToFile = true;
                        fileNameLocation = args[1];
                    }
                }
                aReader = Console.In;
            }
            else if (args.Length > 0)
            {
                if (args[0].ToLower() == "file")
                {
                    fileNameLocation = args[1];
                    printToFile = true;
                    try
                    {
                        aReader = File.OpenText(args[2]);
                    }
                    catch (Exception e)
                    {
                        Console.Write(e.Message);
                        return;
                    }
                }
                else
                {
                    try
                    {
                        aReader = File.OpenText(args[0]);
                    }
                    catch (Exception e)
                    {
                        Console.Write(e.Message);
                        return;
                    }
                }
            }
            else
            {
                Console.Write("You didn't input a valid filename nor did you pipe data for the JSON Parser!");
                return;
            }
            
            string all = aReader.ReadToEnd();

            DateTime sTime = DateTime.Now;
            DateTime eTime = DateTime.Now;
            //Compiles the JSON

            int totalLinesProcessed = 0;
            while(all[totalLinesProcessed] != '[' && all[totalLinesProcessed] != '{')
            {
                if(char.IsLetter(all[totalLinesProcessed]))
                {
                    throw new Exception("Invalid starting token at position: " + totalLinesProcessed);
                }
                totalLinesProcessed++;
            }
            Value myCompliledJSON = null;
            if (all[totalLinesProcessed] == '[')
            {
                sTime = DateTime.Now;
                myCompliledJSON = new array(all, ref totalLinesProcessed);
                eTime = DateTime.Now;
                Console.WriteLine("Time for object to process: " + (eTime - sTime).Milliseconds);   
            }
            else if(all[totalLinesProcessed] == '{')
            {
                sTime = DateTime.Now;
                myCompliledJSON = new aObject(all, ref totalLinesProcessed, true);
                eTime = DateTime.Now;
                Console.WriteLine("Time for object to process: " + (eTime - sTime).Milliseconds);
            }
            if (!printToFile)
            {
                myCompliledJSON.PrettyPrint(Console.OpenStandardOutput(), 0);
            }
            else
            {

                try
                {
                    Stream aWriter = (Stream)File.Open(fileNameLocation, FileMode.Create);
                    myCompliledJSON.PrettyPrint((Stream)aWriter, 0);
                    aWriter.Flush();
                    aWriter.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return;
                }
            }
            Console.Write("\n\nObject has weight: " + myCompliledJSON.GetWeight() + "\n\n");
            Console.Write("File Processed in: " + (eTime - sTime).Milliseconds);
            while (true) ; //Only way to pause indirection
        }
    }
}
