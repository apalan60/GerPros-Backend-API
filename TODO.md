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

- 雲端架構調整
```txt
       Internet
          |
    (Public IP / DNS)
          |
        [ALB]
          |
  +---------------------+
  |    Public Subnets   |
  |   (裝 NAT Gateway)   |
  +---------------------+
          |
          | NAT
          |
  +---------------------+
  |    Private Subnets  |
  | (放 ECS EC2 Instances)
  +---------------------+
```

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

## 調整項目

| 類型  | 項目                                                                                           |
|-----|----------------------------------------------------------------------------------------------|
| 補資料 | 首頁 1. 輪播背景圖(直/橫各一張) 2. 數據文字 3. 客戶回饋 3. 注目產品分類 4. footer作品集 5. header gerpros駝峰? 6. footer 連結 |
| bug | 切版- 手機 右邊會空白                                                                                 |
| bug | 產品明細 麵包屑路由, 文字換中文                                                                            |
| 調整  | 產品列表 左側tab、麵包屑下滑時固定 / 頁數mb固定  / 調成3格一行                                                       |
| 調整  | 產品明細 1. 手機板 tag 字數超過會跑版，若超過某上限，呈現...                                                         |
| 調整  | 文章 新增麵包屑/ 右側篩選fixed                                                                          |
| 調整  | 文章後台 margin 調整                                                                               |
| bug | 文章後台 card寬度 https://main.d1hm4l1a6kz6r6.amplifyapp.com/manager/topic                         |
| 調整  | 文章前台 文章明細 目前背景圖在明細頁會跑版，請它們提供統一圖片                                                             |
| bug | 文章後台 更新完路由沒跳 / 內文固定寬度                                                                        |
| 調整  | 文章後台 閱讀更多固定在右側 / 刪除後重整                                                                       |
| bug | 編輯 / 刪除圖片後，掃HTML、移除fileStorage，帶最後版本的fileStorage                                             |
| 告知  | 文章後台路徑之後會拔掉                                                                                  |
| bug | 明細頁重整後會回到首頁，請改成留在原頁面                                                                         |
| bug | 立即預約 滾動                                                                                      |
| bug | Contact Us POST                                                                              |
| bug | Edit FAQ 同時編輯兩個類別會只單更到一個                                                                     |
| bug | /manager/Product/detail 路由消失                                                                 |
| 調整  | 後台註冊頁 新增密碼規則                                                                                 |
| 調整  | 關於我們 - 服務項目重複                                                                                |
| bug | 聯絡我們 RWD 說明 + 地址                                                                             |
|調整| 關於我們- 品牌故事 文字 pendding                                                                       |
|調整| 關於我們- 服務流程 圖片移掉                                                                              |
|調整| 關於我們- 品質認證 認證一排一張                                                                            |
