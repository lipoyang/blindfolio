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
            float footOffset   = 0.0f;
            float gutterOffset = 0.0f;
            int startNombre = 1;
            if (!System.IO.File.Exists(inputFileName))
            {
                Console.WriteLine("Input file not found!");
                Console.WriteLine("Usage: blindfolio inputfile outputfile foot gutter start");
            }
            try
            {
                footOffset = float.Parse(args[2]);
                footOffset = (footOffset * 72.0f) / 25.4f; // mmからptに換算
            }
            catch{
                Console.WriteLine("Invalid foot offset!");
                Console.WriteLine("Usage: blindfolio inputfile outputfile foot gutter start");
                return;
            }
            try{
                gutterOffset = float.Parse(args[3]);
                gutterOffset = (gutterOffset * 72.0f) / 25.4f; // mmからptに換算
            }
            catch
            {
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
            PdfReader reader = new PdfReader(inputFileName);
            FileStream fs = new FileStream(outputFileName, FileMode.Create, FileAccess.Write);
            PdfStamper stamper = new PdfStamper(reader, fs);

            // 不透明度指定用のグラフィックステート
            PdfGState gs = new PdfGState();
            gs.FillOpacity = 1.0f;

            // フォントサイズ TODO
            float FontSize = 8;

            // 各ページについて
            int totalPageNum = reader.NumberOfPages; // 総ページ数
            for (int page = 1; page <= totalPageNum; page++)
            {
                // 隠しノンブルの文字列
                string text = (startNombre + page - 1).ToString();
                Font font = FontFactory.GetFont(FontFactory.COURIER, FontSize, Font.NORMAL, BaseColor.BLACK);
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
                float uy = footOffset + FontSize * text.Length;
                // 隠しノンブルのx座標
                int align;
                float lx, rx;
                if((page % 2) == 0){
                    // 偶数ページは右側がノド
                    align = Element.ALIGN_RIGHT;
                    lx = 0;
                    rx = pageSize.Width - gutterOffset;
                }
                else
                {
                    // 奇数ページは左側がノド
                    align = Element.ALIGN_LEFT;
                    lx = gutterOffset;
                    rx = pageSize.Width;
                }
                PdfContentByte a_page = stamper.GetOverContent(page);
                a_page.SaveState();//グラフィックステートを退避
                a_page.SetGState(gs);// グラフィックステートの設定

                ColumnText ct = new ColumnText(a_page);    // テキスト欄の新規作成
                ct.SetSimpleColumn(             // テキスト欄の内容(スタンプ)と配置を設定
                    phrase, lx, ly, rx, uy, FontSize, align);
                ct.Go();                        // テキスト欄の出力

                a_page.RestoreState();
            }

            // ファイルを閉じる
            stamper.Close();
            fs.Close();
            reader.Close();
        }
    }
}
