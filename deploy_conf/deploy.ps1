#!/usr/bin/env pwsh

<#
.SYNOPSIS
Scripts to deploy docker-compose configurated combination services.

.DESCRIPTION
This PowerShell script combined with the docker-compose.yml files to pull and deploy docker images from Docker Registry.

.EXAMPLE
./deploy.ps1 -registry [The_remote_registry] pull
# Docker pull all required images.

.EXAMPLE 
./deploy.ps1 -registry [The_remote_registry] -https_cert_pass [SSL_certificate_file_password] --project-name [Custom_project_name] up --detach
# Run whole docker compose project.

#>

# input parameter
[CmdletBinding(PositionalBinding = $false)]
param(
    [Parameter(HelpMessage = "docker registry")]
    [String]
    #Specify the docker registry that docker images will pull from.
    $registry,
    [Parameter(HelpMessage = "silo host version")]
    [String]
    #Silo Host version number.
    $siloHostVer,
    [Parameter(HelpMessage = "web client version")]
    [String]
    #Invoke Orleans RPC web client version number.
    $webClientVer,
    [Parameter(HelpMessage = "Helloworld grain version")]
    [String]
    #Helloworld grain version number.
    $helloworld_grain_ver,
    [Parameter(HelpMessage = "MyReminder grain version")]
    [String]
    #MyReminder grain version number.
    $myreminder_grain_ver,
    [Parameter(HelpMessage = "NumberGenerator grain version")]
    [String]
    #Number generator version number.
    $numbergenerator_grain_ver,
    [Parameter(HelpMessage = "Override all grains version")]
    [String]
    #Specify all Orleans Grain docker images version tag that docker-compose use.
    $override_grains_ver,
    [Parameter(HelpMessage = "Override docker image version")]
    [String]
    #Specify all docker images version tag that docker-compose use.
    $override_img_ver,
    [Parameter(HelpMessage = "HTTP Port")]
    [int]
    #Web client Http port.
    $http_port,
    [Parameter(HelpMessage = "HTTPS Port")]
    [int]
    #Web client Https port.
    $https_port,
    [Parameter(HelpMessage = "SSL cert password")]
    [String]
    #Web client Https SSL certificate password.
    $https_cert_pass,
    [Parameter(ValueFromRemainingArguments)]
    #Other command line arguments for docker-compose, needs to use full parameter syntax.
    [String]$other_args
)

if ($registry) {
    $env:DOCKER_REGISTRY = $registry + "/";
    Write-Output "Using Registry= `"$env:DOCKER_REGISTRY`"";
}

if ($override_img_ver) {
    $env:SILO_HOST_VER = $override_img_ver;
    Write-Output "Using Silo Host Ver= `"$env:SILO_HOST_VER`"";

    $env:WEB_CLIENT_VER = $override_img_ver;
    Write-Output "Using Web Client ver= `"$env:WEB_CLIENT_VER`"";
    
    $env:GRAIN_VER_HELLOWORLD = $override_img_ver;
    Write-Output "Using Helloworld grain ver= `"$env:GRAIN_VER_HELLOWORLD`"";

    $env:GRAIN_VER_MYREMINDER = $override_img_ver;
    Write-Output "Using MyReminder grain ver= `"$env:GRAIN_VER_MYREMINDER`"";

    $env:GRAIN_VER_NUMBERGENERATOR = $override_img_ver;
    Write-Output "Using Number Generator grain ver= `"$env:GRAIN_VER_NUMBERGENERATOR`"";
}
elseif ($override_grains_ver) {
    $env:GRAIN_VER_HELLOWORLD = $override_grains_ver;
    Write-Output "Using Helloworld grain ver= `"$env:GRAIN_VER_HELLOWORLD`"";

    $env:GRAIN_VER_MYREMINDER = $override_grains_ver;
    Write-Output "Using MyReminder grain ver= `"$env:GRAIN_VER_MYREMINDER`"";

    $env:GRAIN_VER_NUMBERGENERATOR = $override_grains_ver;
    Write-Output "Using Number Generator grain ver= `"$env:GRAIN_VER_NUMBERGENERATOR`"";
}
else {
    if ($siloHostVer) {
        $env:SILO_HOST_VER = $siloHostVer;
        Write-Output "Using Silo Host Ver= `"$env:SILO_HOST_VER`"";
    }
    
    if ($webClientVer) {
        $env:WEB_CLIENT_VER = $webClientVer;
        Write-Output "Using Web Client ver= `"$env:WEB_CLIENT_VER`"";
    }
    
    if ($helloworld_grain_ver) {
        $env:GRAIN_VER_HELLOWORLD = $helloworld_grain_ver;
        Write-Output "Using Helloworld grain ver= `"$env:GRAIN_VER_HELLOWORLD`"";    
    }
    
    if ($myreminder_grain_ver) {
        $env:GRAIN_VER_MYREMINDER = $myreminder_grain_ver;
        Write-Output "Using MyReminder grain ver= `"$env:GRAIN_VER_MYREMINDER`"";
    }
    
    if ($numbergenerator_grain_ver) {
        $env:GRAIN_VER_NUMBERGENERATOR = $numbergenerator_grain_ver;
        Write-Output "Using Number Generator grain ver= `"$env:GRAIN_VER_NUMBERGENERATOR`"";
    }
}

if ($http_port) {
    $env:HTTP_PORT = $http_port;
    Write-Output "Using HTTP Port: $env:HTTP_PORT"
}

if ($https_port) {
    $env:HTTPS_PORT = $https_port;
    Write-Output "Using HTTPS Port: $env:HTTPS_PORT"
}

if ($https_cert_pass) {
    $env:SSL_PASS = $https_cert_pass;
}
else {
    $env:SSL_PASS = "Pass1234";
}

if ($other_args) {
    Write-Output "Composer command arguments = $other_args"
}

$execStr = "docker-compose -f docker-compose.yml -f parameters.yml $other_args";

Write-Host "run:`r`n$execStr";

Invoke-Expression $execStr

if ($env:DOCKER_REGISTRY) {
    Remove-Item Env:\DOCKER_REGISTRY;
}
if ($env:SILO_HOST_VER) {
    Remove-Item Env:\SILO_HOST_VER;
}
if ($env:WEB_CLIENT_VER) {
    Remove-Item Env:\WEB_CLIENT_VER;
}
if ($env:GRAIN_VER_HELLOWORLD) {
    Remove-Item Env:\GRAIN_VER_HELLOWORLD;
}
if ($env:GRAIN_VER_MYREMINDER) {
    Remove-Item Env:\GRAIN_VER_MYREMINDER;
}
if ($env:GRAIN_VER_NUMBERGENERATOR) {
    Remove-Item Env:\GRAIN_VER_NUMBERGENERATOR;
}
if ($http_port) {
    Remove-Item Env:\HTTP_PORT;
}
if ($https_port) {
    Remove-Item Env:\HTTPS_PORT;
}
if ($https_cert_pass) {
    Remove-Item Env:\SSL_PASS;
}