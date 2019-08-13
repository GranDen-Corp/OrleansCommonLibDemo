# Deploy via Docker Compose

1. Install [PowerShell Core](https://github.com/powershell/powershell) v6.2 and above.
2. Put a valid SSL [PKCS #12 Certificate File](https://fileinfo.com/extension/pfx#PKCS_#12_Certificate_File) inside the `ssl_cert` sub-directory, rename it as `demo-cert.pfx`.  
(note: you can create a development certificate using .NET Core 2.x tool like [this](https://www.hanselman.com/blog/DevelopingLocallyWithASPNETCoreUnderHTTPSSSLAndSelfSignedCerts.aspx) told.)
3. Run docker pull via:  
  
    ```Shell
    ./deploy.ps1 -registry [The_remote_registry] pull
    ```

4. Run whole docker compose project as:  
  
    ```Shell
    ./deploy.ps1 -registry [The_remote_registry] -https_cert_pass [SSL_certificate_file_password] --project-name [Custom_project_name] up --detach
    ```
