using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.IO;
using Triangle;

namespace TriangleTest
{
    class Program
    {
        static void Main()
        {
            int testCounter = 1;
            using var outputFile = new StreamWriter("../../../output.txt", false);

            using (var inputFile = new StreamReader("../../../input.txt"))
            {
                string testCaseArguments;
                string expectedOutput;
                string receivedOutput;

                while ((testCaseArguments = inputFile.ReadLine()) != null)
                {
                    string[] args = testCaseArguments.Split(' ');
                    expectedOutput = inputFile.ReadLine();

                    var output = new StringWriter();
                    Console.SetOut(output);
                    Console.SetError(output);

                    Triangle.Program.Main(args);
                    receivedOutput = output.ToString();
                    receivedOutput = receivedOutput.Replace("\n", "").Replace("\r", "");

                    if (receivedOutput == expectedOutput)
                    {
                        outputFile.WriteLine("Test passed " + testCounter + ": successfully");
                    }
                    else
                    {
                        outputFile.WriteLine("Test " + testCounter + ": failed. Expected: " + expectedOutput + " Recieved: " + receivedOutput);
                    }

                    testCounter++;
                }
            }
        }
    }
}
