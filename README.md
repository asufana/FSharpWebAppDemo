
# F# (FSharp) WebApp事始め

Mac OSX El Capitan でゼロから始める F# WebApp！

新しい言語を学ぶ際には、言語リファレンスを上から読んでいくのも良いが、実際に動くものを作りながら学んでいく[遅延勉強法](http://d.hatena.ne.jp/amachang/20080204/1202104260)なアプローチが良いと思う。

REPLで一通り遊んだあと、Webアプリを作ってみたいなあとサンプルを探していたら、素晴らしいチュートリアルとその和訳記事を発見した。

[Suave Music Store Tutorial 日本語版](https://theimowski.gitbooks.io/suave-music-store/content/ja/)

以下にWebアプリが動作する最小限のコードを紹介する。F#を習得するためのとっかかりにしてください。

## 環境情報

- OSX El Capitan 10.11.1
- Xamarin Studio 5.10 (build 871)
- mono 4.2.1
- fsharpi 4.0
- PostgreSQL 9.4

## F# ランタイムインストール

Homebrewからインストールする

```bash
$ brew install mono
```

他の方法はこちらを参照
http://fsharp.org/use/mac/


## NuGet インストール

外部モジュールを利用するため、パッケージマネージャNuGetをインストールする

- https://www.nuget.org/ から latest nuget.exe をダウンロード
- 以下のシェルスクリプト nuget を作成

```bash
$ touch nuget
$ vim nuget
#!/bin/sh
script_dir="$(cd "$(dirname "${BASH_SOURCE:-${(%):-%N}}")"; pwd)"
mono --runtime=v4.0 ${script_dir}/nuget.exe $*
```

正しく実行されることを確認

```bash
$ chmod 700 nuget
$ ./nuget
```

## PostgreSQL インストール

（手元にPostgreSQLがなければ）http://postgresapp.com/ から Postgres.app をインストールする

psqlほかコマンドにPathを通しておく

```bash
$ export PATH=$PATH:/Applications/Postgres.app/Contents/Versions/9.4/bin
```

## ソースダウンロード

```bash
$ git clone https://github.com/asufana/FSharpWebAppDemo.git
```

## テーブル作成

```bash
$ cd FSharpWebAppDemo
$ createdb LightningTalks
$ psql LightningTalks < schema/table.sql
```

## ライブラリ取得

＊ソースに含まれているので取得は不要

```bash
$ mkdir lib
$ cd lib
$ nuget install Suave
$ nuget install Suave.Experimental
$ nuget install FSharp.Data
$ nuget install FSharp.Data.SqlClient
$ nuget install SQLProvider -prerelease
$ nuget install Npgsql
```

## ソース修正

##### ライブラリパスの修正

- 取得したライブラリバージョンに合わせて、各 fsx ファイル内の ```#I``` パスを修正する

##### DB接続文字列の修正

- DB環境に合わせて ```Db.fsx``` 内の ```UserName``` ```Password``` を修正する
- 同様に ```npgsqlPath``` に Npgsql.dll ファイルが配置されている絶対パスを設定する

## 実行

```bash
$ chmod 777 App.fsx
$ ./App.fsx
[I] 2015-12-14T13:49:16.1153760Z: listener started in 35.891 ms with binding 127.0.0.1:8083 [Suave.Tcp.tcpIpServer]
```

正常に起動したら http://127.0.0.1:8083 にアクセスする

