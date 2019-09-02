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
    [Parameter(HelpMessage = "Use timestamp for image tag")]
    [switch]$useTimeStamp = $false,
    [Parameter(HelpMessage = "Push images to repository")]
    [switch]$pushImgs = $false,
    [Parameter(ValueFromRemainingArguments)]
    [String]$other_args
)

$timeStamp = (Get-Date).ToUniversalTime().ToString("yyyyMMdd-HHmmss")

if ($registry) {
    $env:DOCKER_REGISTRY = $registry + "/";
    Write-Output "Using Registry= `"$env:DOCKER_REGISTRY`"";
}
if ($siloHostVer) {
    $env:SILO_HOST_VER = $siloHostVer;
    Write-Output "Using Silo Host Ver= `"$env:SILO_HOST_VER`"";
}
elseif($useTimeStamp){
    $env:SILO_HOST_VER = $timeStamp;
    Write-Output "Using Silo Host Ver= `"$env:SILO_HOST_VER`"";
}

if ($webClientVer) {
    $env:WEB_CLIENT_VER = $webClientVer;
    Write-Output "Using Web Client ver= `"$env:WEB_CLIENT_VER`"";
}
elseif ($useTimeStamp) {
    $env:WEB_CLIENT_VER = $timeStamp;
    Write-Output "Using Web Client ver= `"$env:WEB_CLIENT_VER`"";
}

if ($helloworld_grain_ver) {
    $env:GRAIN_VER_HELLOWORLD = $helloworld_grain_ver;
    Write-Output "Using Helloworld grain ver= `"$env:GRAIN_VER_HELLOWORLD`"";    
}
elseif ($useTimeStamp) {
    $env:GRAIN_VER_HELLOWORLD = $timeStamp;
    Write-Output "Using Helloworld grain ver= `"$env:GRAIN_VER_HELLOWORLD`"";    
}

if ($myreminder_grain_ver) {
    $env:GRAIN_VER_MYREMINDER = $myreminder_grain_ver;
    Write-Host "Using MyReminder grain ver= `"$env:GRAIN_VER_MYREMINDER`"";
}
elseif ($useTimeStamp) {
    $env:GRAIN_VER_MYREMINDER = $timeStamp;
    Write-Host "Using MyReminder grain ver= `"$env:GRAIN_VER_MYREMINDER`"";
}

if ($numbergenerator_grain_ver) {
    $env:GRAIN_VER_NUMBERGENERATOR = $numbergenerator_grain_ver;
    Write-Host "Using NumberGenerator grain ver= `"$env:GRAIN_VER_NUMBERGENERATOR`"";
}
elseif ($useTimeStamp) {
    $env:GRAIN_VER_NUMBERGENERATOR = $timeStamp;
    Write-Host "Using NumberGenerator grain ver= `"$env:GRAIN_VER_NUMBERGENERATOR`"";
}

$execStr = "docker-compose -f docker-compose.yml -f docker-compose.override.yml build $other_args";

Write-Host "run:`r`n$execStr";

Invoke-Expression $execStr

if($pushImgs -and $registry)
{
    Write-Output "`r`n===`nPush images to '$registry' registry`n===`r`n";
    $execStr = "docker-compose -f docker-compose.yml push";
    Write-Host "run:`r`n$execStr";
    Invoke-Expression $execStr
}

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