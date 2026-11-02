param([string]$OutputDirectory = (Join-Path $PSScriptRoot 'publish'))
$ErrorActionPreference = 'Stop'

# 自带 .NET 运行环境；分发时保留整个发布目录，尤其是 Data 和 Tools 文件夹。
Push-Location $PSScriptRoot
try {
    dotnet publish EplanPartBuilder.csproj -c Release -r win-x64 --self-contained true -o $OutputDirectory
    if ($LASTEXITCODE -ne 0) { throw '发布失败。' }
} finally {
    Pop-Location
}
