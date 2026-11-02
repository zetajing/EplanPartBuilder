$ErrorActionPreference = 'Stop'

# 在脚本所在目录构建，方便从资源管理器或其他目录启动。
Push-Location $PSScriptRoot
try {
    dotnet build EplanPartBuilder.sln -c Release
    if ($LASTEXITCODE -ne 0) { throw '构建失败。' }
} finally {
    Pop-Location
}
