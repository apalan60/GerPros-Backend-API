  ```txt
   [Route53 DNS] 
         |
   (CNAME / Alias)
         |
   [ CloudFront + ACM ]  <-- SSL/TLS 
         |
      (HTTPS)
         |
   [EC2 Instance / ECS Container] <-- (API)
  ``` 
