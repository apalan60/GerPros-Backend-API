# TODO

- Email Service && API

- CORS - S3 EC2 CloudFront  (or some integration in ECS?)

- integrate with AWS RDS 

- 請GerPros整理產品資訊、圖片，存放於demo data setting, 留一個flag，有需要就起站台時塞入DB

- 上線前務必設定AWS CDN 防止DDOS攻擊使帳單暴增

- public bucket 改為CloudFront cache + S3 + WAF => implement throttle requests
  - ref 
    - [CloudFront-SignedUrl](https://stackoverflow.com/questions/55723691/what-is-the-maximum-expiration-time-for-cloudfront-signed-url/55729193#55729193)
    - [Discussion of long term storage of s3 pre-signed url](https://stackoverflow.com/questions/55827584/how-to-give-long-term-read-access-to-objects-in-a-private-s3-bucket) 

 Image Tag(automated)

- Code Pipeline

## Note

### Local Psql Command for testing

```bash
-- 建立資料庫
CREATE DATABASE "GerPros_Backend_APITestDb";

-- 建立使用者並設定密碼
CREATE USER "TestUser" WITH PASSWORD 'TestUser123';

-- 賦予資料庫連線權限
GRANT CONNECT ON DATABASE "GerPros_Backend_APITestDb" TO TestUser;

-- 賦予資料表和序列的權限
GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA public TO TestUser;
GRANT ALL PRIVILEGES ON ALL SEQUENCES IN SCHEMA public TO TestUser;

-- 設定預設權限
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT ALL ON TABLES TO TestUser;
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT ALL ON SEQUENCES TO TestUser;

-- Owner
ALTER DATABASE "GerPros_Backend_APITestDb" OWNER TO "TestUser";
GRANT ALL PRIVILEGES ON DATABASE "GerPros_Backend_APITestDb" TO "TestUser";

```