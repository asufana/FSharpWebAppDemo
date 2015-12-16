#!/usr/bin/env fsharpi

//cmd> nuget install Suave
#I "lib/Suave.0.33.0/lib/net40"
#r "Suave.dll"

//外部fsxファイルの読み込み
#load "Db.fsx"
#load "Template.fsx"
open Db
open Template

module App =
  open Suave.Web
  open Suave.Http
  open Suave.Http.Successful
  open Suave.Http.Applicatives

  //helpers
  let show container =
    OK (Template.index container)
    >>= Writers.setMimeType "text/html; charset=utf-8"

  //コントローラ ------------------

  //トップページ表示
  let showHome =
    Db.Conventions.findAll
    |> Template.showHome
    |> show

  //指定勉強会表示
  let showTalks conventionId =
    Db.Talks.findByConventionId conventionId
    |> Template.showTalks
    |> show

  //talkを評価する
  let rateTalk talkId action =
    //talkIdをキーにtalkレコードを取得
    match Db.Talks.findByTalkId talkId with
    //talkレコードが存在すれば
    | Some talk ->
      match action with
        | "plus" -> Db.Talks.ratePlus talk  //プラス評価なら
        | _      -> Db.Talks.rateMinus talk //マイナス評価なら
      showTalks talk.conventionId           //評価後には指定勉強会ページを表示
    //talkレコードが存在しなければトップページを表示
    | None ->
        showHome

  //ルート設定 ----------------------

  let webPart =
    choose [
      path     "/"                     >>= showHome
      pathScan "/talks/%d"            (fun (id) -> showTalks id)
      pathScan "/talks/%d/%s"         (fun (id, action) -> rateTalk id action)

      pathRegex "(.*)\.(css|png|gif)"  >>= Files.browseHome
    ]

  //HTTPサーバ実行
  startWebServer defaultConfig webPart
