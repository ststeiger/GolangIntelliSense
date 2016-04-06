
namespace GolangIntelliSense
{



    class MainClass
    {


        public static string GetProcessOutput(string cmdLine)
        {
            System.Text.StringBuilder outputBuilder;
            System.Diagnostics.ProcessStartInfo processStartInfo;
            System.Diagnostics.Process process;

            outputBuilder = new System.Text.StringBuilder();

            processStartInfo = new System.Diagnostics.ProcessStartInfo();
            processStartInfo.CreateNoWindow = true;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.RedirectStandardInput = true;
            processStartInfo.UseShellExecute = false;
            processStartInfo.Arguments = cmdLine;
            processStartInfo.FileName = "/root/.gvm/pkgsets/go1.6/global/src/github.com/nsf/gocode/gocode";

            process = new System.Diagnostics.Process();
            process.StartInfo = processStartInfo;
            // enable raising events because Process does not raise events by default
            process.EnableRaisingEvents = true;
            // attach the event handler for OutputDataReceived before starting the process
            process.OutputDataReceived += new System.Diagnostics.DataReceivedEventHandler
                (
                    delegate(object sender, System.Diagnostics.DataReceivedEventArgs e)
                    {
                        // append the new data to the data already read-in
                        outputBuilder.Append(e.Data);
                    }
                );
            // start the process
            // then begin asynchronously reading the output
            // then wait for the process to exit
            // then cancel asynchronously reading the output
            process.Start();
            process.BeginOutputReadLine();
            process.WaitForExit();
            // process.CancelOutputRead();

            System.Console.WriteLine(outputBuilder);
            string str = outputBuilder.ToString();
            return str;
        } // End Function GetProcessOutput


        public static int GetOffset(string code, int numLine, int ch)
        {
            string[] linez = code.Split('\n');
            int offset = 0;

            for (int i = 0; i < numLine - 1; ++i)
            {
                offset += System.Text.Encoding.UTF8.GetBytes(linez[i]).Length;
            }
            offset += numLine -1;

            string thisLine = linez[numLine-1];
            offset += ch;


            // Debug info:
            System.Console.WriteLine(thisLine);
            System.Console.WriteLine(offset);

            return offset;
        }


        public class cCompletionEntry
        {
            public string @class;
            public string name;
            public string type;
        }


        public class cResult
        {
            public System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<cCompletionEntry>> ls;
        }



        public static void GetCompletion()
        {
            string fn = @"/root/.gvm/pkgsets/go1.6/global/src/github.com/b3log/wide/main.go";
            string code = System.IO.File.ReadAllText(fn, System.Text.Encoding.UTF8);

            int offset = GetOffset(code, 61, 6);
            // int pos = str.IndexOf("flag.Parse()");
            // System.Console.WriteLine(pos);


            // "./gocode 
            string cmd = "-f=json --in=\"" + fn.Replace("\"", "\\\"") + "\" autocomplete " + offset.ToString();
            System.Console.WriteLine(cmd);


            System.Text.StringBuilder sbIn = new System.Text.StringBuilder(GetProcessOutput(cmd));
            System.Text.StringBuilder sbOut = new System.Text.StringBuilder();
            JsonPrettyPrinter jpp = new JsonPrettyPrinter();
            jpp.PrettyPrint(sbIn, sbOut);

            string strIn = sbIn.ToString();
            string strOut = sbOut.ToString();

            System.Console.WriteLine(strIn);
            System.Console.WriteLine(strOut);


            int pos = strOut.IndexOf(',');
            strOut = strOut.Substring(pos + 1);
            pos = strOut.LastIndexOf(']');
            strOut = strOut.Substring(0, pos);
            System.Console.WriteLine(strOut);
            //[0,
        }


        public static void MonoMain(string[] args)
        {
            GetCompletion();

            System.Console.WriteLine("Hello World!");
        }


    } // End Class


}
