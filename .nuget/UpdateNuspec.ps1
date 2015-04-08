﻿$path = Split-Path -Parent $MyInvocation.MyCommand.Path

$files = Get-ChildItem -Path "$path\.." -Filter *.csproj -Recurse
foreach($f in $files){
	$name = $f.name
	$directory = $f.Directory
	$target = "$directory\$name.nuspec"
	Copy-Item "$path\sample.csproj.nuspec" $target
}