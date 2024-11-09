# TODO

- Complete the `ProductItem` test case.

## Note

### Local Psql Command for testing

```bash
-- 建立資料庫
CREATE DATABASE "GerPros_Backend_APITestDb";

-- 建立使用者並設定密碼
CREATE USER TestUser WITH PASSWORD 'TestUser123';

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