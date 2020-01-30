using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO; // FileStream, FileMode
using iTextSharp.text;
using iTextSharp.text.pdf;


namespace blindfolio
{
    class Program
    {
        static void Main(string[] args)
        {
            // パラメータチェック
            if(args.Length != 5)
            {
                Console.WriteLine("Bad parameters!");
                Console.WriteLine("Usage: blindfolio inputfile outputfile foot gutter start");
                return;
            }
            string inputFileName  = args[0];
            string outputFileName = args[1];
            double footOffset   = 0.0;
            double gutterOffset = 0.0;
            int startNombre = 1;
            if (!System.IO.File.Exists(inputFileName))
            {
                Console.WriteLine("Input file not found!");
                Console.WriteLine("Usage: blindfolio inputfile outputfile foot gutter start");
            }
            try
            {
                footOffset = double.Parse(args[2]);
            }catch{
                Console.WriteLine("Invalid foot offset!");
                Console.WriteLine("Usage: blindfolio inputfile outputfile foot gutter start");
                return;
            }
            try{
                gutterOffset = double.Parse(args[3]);
            }catch{
                Console.WriteLine("Invalid gutter offset!");
                Console.WriteLine("Usage: blindfolio inputfile outputfile foot gutter start");
                return;
            }
            try{
                startNombre = int.Parse(args[4]);
            }catch{
                Console.WriteLine("Invalid start nombre!");
                Console.WriteLine("Usage: blindfolio inputfile outputfile foot gutter start");
                return;
            }

            // ファイルを開く
            PdfReader inputFile = new PdfReader(inputFileName);
            Rectangle size = inputFile.GetPageSize(1); // ページのサイズ取得
            int totalPageNum = inputFile.NumberOfPages;

            //Document document = new Document(size);
            FileStream fs = new FileStream(outputFileName, FileMode.Create, FileAccess.Write);
            PdfStamper outputFile = new PdfStamper(inputFile, fs);
            //document.Open();

            //PdfContentByte pdfContentByte = outputFile.DirectContent;
            //var page = outputFile.GetImportedPage(inputFile, 1);
            //pdfContentByte.AddTemplate(page, 0, 0);

            //pdfContentByte.BeginText();
            //pdfContentByte.ShowTextAligned(Element.ALIGN_LEFT, text, x, y, 0);
            //pdfContentByte.EndText();

            //


            // ファイルを閉じる
            //document.Close();
            //fs.Close();
            outputFile.Close();
            //inputFile.Close();
        }
    }
}
