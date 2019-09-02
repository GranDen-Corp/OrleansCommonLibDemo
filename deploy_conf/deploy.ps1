#!/usr/bin/env pwsh
# input parameter
[CmdletBinding(PositionalBinding=$false)]
param(
    [Parameter(HelpMessage = "docker registry")]
    [String]$registry,
    [Parameter(HelpMessage = "silo host version")]
    [String]$siloHostVer,
    [Parameter(HelpMessage = "web client version")]
    [String]$webClientVer,
    [Parameter(HelpMessage = "Helloworld grain version")]
    [String]$helloworld_grain_ver,
    [Parameter(HelpMessage = "MyReminder grain version")]
    [String]$myreminder_grain_ver,
    [Parameter(HelpMessage = "NumberGenerator grain version")]
    [String]$numbergenerator_grain_ver,
    [Parameter(HelpMessage = "HTTP Port")]
    [int]$http_port,
    [Parameter(HelpMessage = "HTTPS Port")]
    [int]$https_port,
    [Parameter(HelpMessage = "SSL cert password")]
    [String]$https_cert_pass,
    [Parameter(ValueFromRemainingArguments)]
    [String]$other_args
)

if ($registry) {
    $env:DOCKER_REGISTRY = $registry + "/";
    Write-Output "Using Registry= `"$env:DOCKER_REGISTRY`"";
}

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