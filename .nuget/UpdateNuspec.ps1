$path = Split-Path -Parent $MyInvocation.MyCommand.Path
$nugetOutputDir = "$path\..\generated-packages"
md -Force $nugetOutputDir

$files = Get-ChildItem -Path "$path\.." -Filter *.csproj -Recurse
foreach($f in $files){
	$name = $f.Basename
	$directory = $f.Directory
	$target = "$directory\$name.nuspec"
	Copy-Item "$path\sample.csproj.nuspec" $target
}

foreach($f in $files){
	$fullName = $f.FullName	
	& "$path\nuget.exe" pack $fullName -Build -Prop Configuration=Release -OutputDirectory $nugetOutputDir -IncludeReferencedProjects -Symbols
}