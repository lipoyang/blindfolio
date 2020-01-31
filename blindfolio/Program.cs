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
            if(args.Length != 6){
                Console.WriteLine("Bad parameters!");
                Console.WriteLine("Usage: blindfolio inputfile outputfile foot gutter start size");
                return;
            }
            string inputFileName  = args[0];
            string outputFileName = args[1];
            float footOffset   = 0.0f;
            float gutterOffset = 0.0f;
            int startNombre = 1;
            float fontSize = 8;
            if (!System.IO.File.Exists(inputFileName))
            {
                Console.WriteLine("Input file not found!");
                Console.WriteLine("Usage: blindfolio inputfile outputfile foot gutter start size");
                return;
            }
            try{
                footOffset = float.Parse(args[2]);
                footOffset = (footOffset * 72.0f) / 25.4f; // mmからptに換算
            }catch{
                Console.WriteLine("Invalid foot offset!");
                Console.WriteLine("Usage: blindfolio inputfile outputfile foot gutter start size");
                return;
            }
            try{
                gutterOffset = float.Parse(args[3]);
                gutterOffset = (gutterOffset * 72.0f) / 25.4f; // mmからptに換算
            }catch{
                Console.WriteLine("Invalid gutter offset!");
                Console.WriteLine("Usage: blindfolio inputfile outputfile foot gutter start size");
                return;
            }
            try{
                startNombre = int.Parse(args[4]);
            }catch{
                Console.WriteLine("Invalid start nombre!");
                Console.WriteLine("Usage: blindfolio inputfile outputfile foot gutter start size");
                return;
            }
            try{
                fontSize = float.Parse(args[5]);
            }catch{
                Console.WriteLine("Invalid font size!");
                Console.WriteLine("Usage: blindfolio inputfile outputfile foot gutter start size");
                return;
            }

            // ファイルを開く
            PdfReader reader;
            try{
                reader = new PdfReader(inputFileName);
            }catch(Exception e){
                Console.WriteLine(e.Message);
                Console.WriteLine("Usage: blindfolio inputfile outputfile foot gutter start size");
                return;
            }
            FileStream fs;
            try{
                fs = new FileStream(outputFileName, FileMode.Create, FileAccess.Write);
            }catch(Exception e){
                Console.WriteLine(e.Message);
                Console.WriteLine("Usage: blindfolio inputfile outputfile foot gutter start size");
                return;
            }
            PdfStamper stamper = new PdfStamper(reader, fs);

            // 不透明度指定用のグラフィックステート
            //PdfGState gs = new PdfGState();
            //gs.FillOpacity = 1.0f;

            // 各ページについて
            int totalPageNum = reader.NumberOfPages; // 総ページ数
            for (int page = 1; page <= totalPageNum; page++)
            {
                // 隠しノンブルの文字
                string text = (startNombre + page - 1).ToString();
                Font font = FontFactory.GetFont(FontFactory.COURIER, fontSize, Font.NORMAL, BaseColor.BLACK);
                string[] chars = new string[text.Length];
                for(int i = 0; i < text.Length; i++){
                    chars[i] = new string(text[i], 1);
                }
                Phrase phrase = new Phrase();
                phrase.Add(new Chunk(chars[0], font));
                for(int i = 1; i < text.Length; i++){
                    phrase.Add(new Chunk("\n" + chars[i], font));
                }

                // ページのサイズ
                Rectangle pageSize = reader.GetPageSize(page);

                // 隠しノンブルのy座標
                float ly = footOffset;
                float uy = footOffset + fontSize * text.Length;
                // 隠しノンブルのx座標
                int align;
                float lx, rx;
                if((page % 2) == 0){
                    // 偶数ページは右側がノド
                    align = Element.ALIGN_RIGHT;
                    lx = 0;
                    rx = pageSize.Width - gutterOffset;
                }else{
                    // 奇数ページは左側がノド
                    align = Element.ALIGN_LEFT;
                    lx = gutterOffset;
                    rx = pageSize.Width;
                }

                // ノンブル記入のためにページ内容の取得
                PdfContentByte a_page = stamper.GetOverContent(page);
                //a_page.SaveState();  //グラフィックステートを退避
                //a_page.SetGState(gs);//グラフィックステートの設定
                    
                // ノンブルの記入
                ColumnText ct = new ColumnText(a_page);
                ct.SetSimpleColumn(phrase, lx, ly, rx, uy, fontSize, align);
                ct.Go();

                //a_page.RestoreState(); //グラフィックステートの復帰
            }
            // ファイルを閉じる
            stamper.Close();
            fs.Close();
            reader.Close();
        }
    }
}
