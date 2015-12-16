
CREATE TABLE "public"."Conventions" (
    "id" int4 NOT NULL,
    "title" varchar(1000) NOT NULL COLLATE "default",
    "conventionDate" date NOT NULL
);
INSERT INTO "public"."Conventions" VALUES ('1', '第1回LT大会', '2015-12-01');
INSERT INTO "public"."Conventions" VALUES ('2', '第2回LT大会', '2015-12-08');
INSERT INTO "public"."Conventions" VALUES ('3', '第3回LT大会', '2015-12-15');
ALTER TABLE "public"."Conventions" ADD PRIMARY KEY ("id") NOT DEFERRABLE INITIALLY IMMEDIATE;
CREATE UNIQUE INDEX  "Conventions_id_key" ON "public"."Conventions" USING btree("id" ASC NULLS LAST);

CREATE TABLE "public"."Talks" (
    "id" int4 NOT NULL,
    "title" varchar(200) NOT NULL COLLATE "default",
    "presenter" varchar(50) NOT NULL COLLATE "default",
    "conventionId" int4 NOT NULL,
    "url" varchar(200) COLLATE "default",
    "rate" int4 NOT NULL
);
INSERT INTO "public"."Talks" VALUES ('1', 'F# REPL', 'foo', '1', 'http://www.google.com/', '0');
INSERT INTO "public"."Talks" VALUES ('2', 'F# 言語仕様', 'bar', '1', 'http://www.google.com/', '0');
INSERT INTO "public"."Talks" VALUES ('3', 'F# リスト操作', 'foo', '2', 'http://www.google.com/', '0');
INSERT INTO "public"."Talks" VALUES ('4', 'F# モナド？', 'bar', '2', 'http://www.google.com/', '0');
INSERT INTO "public"."Talks" VALUES ('5', 'F# コンピュテーション式', 'foo', '3', 'http://www.google.com/', '0');
INSERT INTO "public"."Talks" VALUES ('6', 'F# WebApp', 'bar', '3', 'http://www.google.com/', '0');
ALTER TABLE "public"."Talks" ADD PRIMARY KEY ("id") NOT DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE "public"."Talks" ADD CONSTRAINT "talk_convention" FOREIGN KEY ("conventionId") REFERENCES "public"."Conventions" ("id") ON UPDATE NO ACTION ON DELETE NO ACTION NOT DEFERRABLE INITIALLY IMMEDIATE;
