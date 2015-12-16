
//cmd> nuget install Suave.Experimental
#I "lib/Suave.Experimental.0.33.0/lib/net40"
#r "Suave.Experimental.dll"

#load "Db.fsx"
open Db

module Template =
  open Suave.Html

  //helpers
  let divId id = divAttr ["id",id]
  let aHref href = tag "a" ["href",href]
  let h1 xml = tag "blockquote" [] (tag "h1" [] xml)
  let pullright x = tag "span" ["class","pull-right"] (flatten x)

  let table x = tag "table" ["class","table table-condensed"] (flatten x)
  let th x = tag "th" [] (flatten x)
  let tr x = tag "tr" [] (flatten x)
  let td x = tag "td" [] (flatten x)

  let button title = tag "button" ["type","button"; " class","btn btn-primary btn-xs"] (text title)


  //html基本構成
  let bootstrapUrl = "https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/"
  let index container =
    html [
      head [ title "Lightning Talks"
             scriptAttr [ "src","https://ajax.googleapis.com/ajax/libs/jquery/2.1.4/jquery.min.js"; " type","text/javascript"] [text ""]
             linkAttr [ "href",bootstrapUrl + "/css/bootstrap.min.css"; " rel","stylesheet"; " type","text/css" ]
             scriptAttr [ "src",bootstrapUrl + "/js/bootstrap.min.js"; " type","text/javascript"] [text ""]
             linkAttr [ "href","/site.css"; " rel","stylesheet"; " type","text/css" ]
      ]
      body [
        divId "header" [
          h1 (aHref "/" (text "Lightning Talks"))
        ]
        divId "main" container
        divId "footer" [
          pullright [
            text "built with "
            aHref "http://suave.io" (text "Suave.IO")
          ]
        ]
      ]
    ] |> xmlToString

  //pages --------------------

  //トップページ表示
  let showHome (conventions:Db.ConventionsTable list) = [
    table [
      for c in conventions ->
        tr [
          td [
            text (c.conventionDate.ToString("yyyy/MM/dd"))
            aHref ("/talks/" + (string c.id)) (text c.title)
          ]
        ]
    ]
  ]

  //指定LT表示
  let showTalks (talks:Db.TalksTable list) = [
    table [
      yield tr [
        for c in ["タイトル";"発表者";"リンク";"評価";""] -> th [ text c ]
      ]
      for t in talks ->
        let url = match t.url with
                  | Some url -> aHref url (text url)
                  | None     -> text ""
        tr [
          td [ text t.title ]
          td [ text t.presenter ]
          td [ url ]
          td [ text (string t.rate + " points") ]
          td [ aHref ("/talks/" + (string t.id) + "/plus") (button "PLUS")
               aHref ("/talks/" + (string t.id) + "/minus") (button "MINUS") ]
        ]
    ]
  ]
