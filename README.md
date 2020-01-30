# blindfolio
PDFに隠しノンブルを追加するコマンドラインツール

## プラットフォーム
WindowsのコマンドプロンプトまたはPowerShell用です。

## 依存ライブラリ
* iTextSharp

## 隠しノンブルとは
製本すると見えにくくなるようにノド側に入れるページ番号。

## 使い方

```
blindfolio inputfile outputfile foot gutter start
```

* inputfile : 入力ファイル名
* outputfile : 出力ファイル名
* foot : 地からのオフセット位置[mm]
* gutter : ノドからのオフセット位置[mm]
* start : ノンブルの開始番号
