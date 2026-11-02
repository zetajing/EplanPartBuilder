param([string]$OutputDirectory = (Join-Path $PSScriptRoot 'checks'), [string]$PackageDirectory = '')
$ErrorActionPreference = 'Stop'

# 用控制台宿主等待程序返回，避免 PowerShell 提前结束 WinExe 子进程。
Push-Location $PSScriptRoot
try {
    dotnet build EplanPartBuilder.sln -c Release
    if ($LASTEXITCODE -ne 0) { throw '构建失败。' }
    dotnet '.\bin\Release\net8.0-windows\EplanPartBuilder.dll' --self-check $OutputDirectory
    if ($LASTEXITCODE -ne 0) { throw "离线检查失败，请查看 $OutputDirectory\report.txt" }
    if ($PackageDirectory.Length -gt 0) {
        # 真实样包检查会生成较大文件，每次使用新的检查目录，不覆盖先前证据。
        $packageOutput = Join-Path $OutputDirectory ('package-' + [Guid]::NewGuid().ToString('N'))
        dotnet '.\bin\Release\net8.0-windows\EplanPartBuilder.dll' --package-check $PackageDirectory $packageOutput
        if ($LASTEXITCODE -ne 0) { throw "真实包检查失败，请查看 $packageOutput\package-report.txt" }
        Get-Content -LiteralPath (Join-Path $packageOutput 'package-report.txt')
        dotnet '.\bin\Release\net8.0-windows\EplanPartBuilder.dll' --ui-check $OutputDirectory $PackageDirectory
    } else {
        dotnet '.\bin\Release\net8.0-windows\EplanPartBuilder.dll' --ui-check $OutputDirectory
    }
    if ($LASTEXITCODE -ne 0) { throw '界面检查失败。' }
    Get-Content -LiteralPath (Join-Path $OutputDirectory 'report.txt')
    Get-Content -LiteralPath (Join-Path $OutputDirectory 'ui-report.txt')
} finally {
    Pop-Location
}
