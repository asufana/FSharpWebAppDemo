
//cmd> nuget install FSharp.Data
//cmd> nuget install FSharp.Data.SqlClient
//cmd> nuget install SQLProvider -prerelease
//cmd> nuget install Npgsql
#I "lib/FSharp.Data.2.2.5/lib/net40"
#I "lib/FSharp.Data.SqlClient.1.7.7/lib/net40"
#I "lib/SQLProvider.0.0.9-alpha/lib/net40"
#I "lib/Npgsql.3.0.4/lib/net45"
#r "FSharp.Data.SQLProvider.dll"

[<Literal>]
let npgsqlPath = @"/Users/hana/Dropbox/Develop/FSharp/demo/lib/Npgsql.3.0.4/lib/net45"

module Db =
  open FSharp.Data.Sql

  //DB接続設定
  type Sql =
    SqlDataProvider<
      ConnectionString =
        """
        Host=127.0.0.1;
        Port=5432;
        Database=LightningTalks;
        Username=postgres;
        Password=p@ssw0rd
        """,
      DatabaseVendor = Common.DatabaseProviderTypes.POSTGRESQL,
      ResolutionPath = npgsqlPath,
      UseOptionTypes = true
      >

  //テーブル型
  type DbContext = Sql.dataContext
  type ConventionsTable = DbContext.``[public].[Conventions]Entity``
  type TalksTable = DbContext.``[public].[Talks]Entity``

  //ヘルパ関数
  let context = Sql.GetDataContext()
  let firstOrNone s = s |> Seq.tryFind (fun _ -> true)

  //クエリ ------------------------

  module Conventions =
    //convention一覧を取得
    let findAll :ConventionsTable list =
      query {
        for c in context.``[public].[Conventions]`` do
          sortByDescending c.id
          select c
      } |> Seq.toList

  module Talks =
    //conventionIdをキーにtalk一覧を取得
    let findByConventionId id :TalksTable list =
      query {
        for t in context.``[public].[Talks]`` do
          where (t.conventionId=id)
          sortBy t.id
          select t
      } |> Seq.toList

    //talkIdをキーにtalkを取得
    let findByTalkId id :TalksTable option =
      query {
        for t in context.``[public].[Talks]`` do
          where (t.id=id)
          select t
      } |> firstOrNone

    //talk更新処理
    let update (talk:TalksTable) (title, presenter, url, rate) =
      talk.title <- title
      talk.presenter <- presenter
      talk.url <- url
      talk.rate <- rate
      context.SubmitUpdates()

    //talk評価をプラスする
    let ratePlus (talk:TalksTable) =
      update talk (talk.title, talk.presenter, talk.url, talk.rate + 1)

    //talk評価をマイナスする
    let rateMinus (talk:TalksTable) =
      if talk.rate > 0 then
        update talk (talk.title, talk.presenter, talk.url, talk.rate - 1)
