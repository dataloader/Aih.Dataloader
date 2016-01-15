#get poject name
$project = $args[0]


#TODO: Test for empty project name


# bootstrap DNVM into this session.
&{$Branch='dev';iex ((new-object net.webclient).DownloadString('https://raw.githubusercontent.com/aspnet/Home/dev/dnvminstall.ps1'))}

# load up the global.json so we can find the DNX version
$globalJson = Get-Content -Path $PSScriptRoot\global.json -Raw -ErrorAction Ignore | ConvertFrom-Json -ErrorAction Ignore

if($globalJson)
{
    $dnxVersion = $globalJson.sdk.version
}
else
{
    Write-Warning "Unable to locate global.json to determine using 'latest'"
    $dnxVersion = "latest"
}

# install DNX
# only installs the default (x86, clr) runtime of the framework.
# If you need additional architectures or runtimes you should add additional calls
  # ex: & $env:USERPROFILE\.dnx\bin\dnvm install $dnxVersion -r coreclr
& $env:USERPROFILE\.dnx\bin\dnvm install $dnxVersion -Persistent

 # run DNU restore on all project.json files in the src folder including 2>1 to redirect stderr to stdout for badly behaved tools
#Get-ChildItem -Path $PSScriptRoot\src -Filter project.json -Recurse | ForEach-Object { & dnu restore $_.FullName 2>1 }
dnu restore $PSScriptRoot\src\$project\project.json

$projectJson = Get-Content -Path $PSScriptRoot\src\$project\project.json -Raw -ErrorAction Ignore | ConvertFrom-Json -ErrorAction Ignore

#Im only interested in the first three parts of the string as it is in the project.json (at least in our setup).
$versionNumber = $projectJson.version.substring(0, 5)



# run DNU publish on all projects to create nuget packages
& dnu publish $PSScriptRoot\src\$project\project.json -o artifacts --no-source

#write-host $PSScriptRoot\artifacts\approot\packages\$project\$versionNumber\$project.$versionNumber.nupkg

#copy-item $PSScriptRoot\artifacts\approot\packages\$project\$versionNumber\$project.$versionNumber.nupkg $PSScriptRoot\artifacts

copy-item $PSScriptRoot\artifacts\approot\packages\$project\$versionNumber\$project.$versionNumber.nupkg \\sec-it-artifacts\nugetpacks

#C:\Github\Securitas\Securitas.DynamicsNav\artifacts\approot\packages\Securitas.DynamicsNav\1.0.1

#Fara í artifacts möppuna 
#kópera það sem undir packages