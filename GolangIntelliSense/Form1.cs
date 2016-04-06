using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GolangIntelliSense
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            lol();
        }


        public static void lol()
        {
            
            string fileName = @"C:\PortableApps\Go\bin\src\github.com\b3log\wide\main.go";
            string code = System.IO.File.ReadAllText(fileName, System.Text.Encoding.UTF8);
            // code = "abc\r\ndef\r\nghi";


            int offset = getCursorOffset(code, 61, 6); // 60, 6
            int foo = code.IndexOf("flag.Parse()");
            foo += "flag".Length;
            

            System.Console.WriteLine(foo);

            findOffset(code, foo);
            System.Console.WriteLine(offset);
            string cmd = "gocode -f = json--in= \"" + fileName.Replace("\"", "\\\"") + "\" autocomplete " + offset.ToString(System.Globalization.CultureInfo.InvariantCulture);
            System.Console.WriteLine(cmd);
        }




        public static int findOffset(string code, int offset)
        {
            int offsetCount = 0;
            int lineNumber = 0;

            string[] lines = code.Split(new char[] { '\n' } , System.StringSplitOptions.None);
            
            for (int i = 0; i < lines.Length; ++i)
            {
                int lineLength = System.Text.Encoding.UTF8.GetBytes(lines[i]).Length;
                ++lineLength;

                offsetCount += lineLength;
                
                if (offsetCount > offset)
                {
                    offsetCount -= lineLength;
                    lineNumber = i + 1;
                    break;
                }
            }

            int ch = offset - offsetCount; // +1?

            System.Console.WriteLine(ch);
            System.Console.WriteLine(lineNumber);
            return offset;
        }


        public static int getCursorOffset(string code, int line, int ch)
        {
            int offset = 0;
            int utfOffset = 0;
            var lines = code.Split('\n');

            line--;
            ch--;


            for (int i = 0; i < line; ++i)
            {
                offset += lines[i].Length;
                utfOffset += System.Text.Encoding.UTF8.GetBytes(lines[i]).Length;
            }

            offset += ch;
            utfOffset += ch;

            offset += line;
            utfOffset += line;


            System.Console.Write("UTF-8 Offset: ");
            System.Console.WriteLine(utfOffset);
            System.Console.Write("C# string.length Offset: ");
            System.Console.WriteLine(offset);
            return offset;
        }


    }


}
