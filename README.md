# blindfolio
PDFに隠しノンブルを追加するコマンドラインツール

## プラットフォーム
WindowsのコマンドプロンプトまたはPowerShell用です。

## 隠しノンブルとは
製本すると見えにくくなるようにノド側に入れるページ番号。製本のために必要な場合があります。

## 依存ライブラリ
* iTextSharp

## 使い方

```
blindfolio inputfile outputfile foot gutter start size
```

* inputfile : 入力ファイル名
* outputfile : 出力ファイル名
* foot : 地(下辺)からのオフセット位置 (単位はmm)
* gutter : ノド(内側の辺)からのオフセット位置 (単位はmm)
* start : ノンブルの開始番号
* size : ノンブルのフォントサイズ (単位はpt)

## 例
```
blindfolio input.pdf output.pdf 30 20 1 6
```
input.pdfに、地から30mm、ノドから20mmの位置に1から始まる隠しノンブルを6ptのフォントで記入し、output.pdfとして出力します。
