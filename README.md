# EPLAN 部件包编辑助手 1.1

这是一个独立 C# WinForms 桌面程序，支持“打开现有部件包 → 显示资料 → 修改指定字段或替换文件 → 保留其余数据 → 生成 EDZ”。打包本身不调用 EPLAN，也不修改 EPLAN 部件数据库。

## 编辑现有包

1. “打开 EDZ”支持 7z / ZIP 容器；“打开目录”选择包含 `manifest.xml` 的已解开目录。
2. 包含多个零部件或变量时，选择要编辑的一项；其他部件及变量保留。
3. 部件资料、功能与端子、技术与厂家可编辑；“原包数据”页可核对全部包内引用、原始字段和连接点坐标。
4. 只更新发生变化的字段；连接点排列样式、自定义属性定义、未编辑的文件与未知字段保留。修改中文名称或说明时保留其他语言。
5. 选择文件表示替换该项，清除表示移除该项关联。未替换的外部文档网址按原引用保留，不自动下载。
6. 生成新的 EDZ，不能覆盖输入 EDZ 或输出到原包目录。原包先复制成独立快照，不直接修改原文件。

保存 `.epart.json` 后，原包快照及替换文件位于同名 `.assets` 文件夹。移动或备份时，JSON 和 `.assets` 必须一起保留。软件退出时清理本次会话的临时快照，不清理已保存的资料目录。

导入较新包时保留原版本。2024 宏重新打包后仍然是 2024 宏，软件不做降版。请在匹配原宏版本的 EPLAN 测试数据库中核对导入结果。

部件组和模块可随原包保留，暂不直接编辑。修改部件级资料会影响所选部件的各变量；技术参数与功能模板只修改所选变量。改动连接点编号后，几何排列样式仍保留，需在 EPLAN 中核对对应关系。

## 新建基础部件

1. 完整解压程序包，运行 `EplanPartBuilder.exe`。
2. 填写部件编号、制造商、名称，可填写型号、订货号、尺寸与技术参数。
3. 选择原理图 `.ema/.ems`、3D / 安装 `.ema`，可附带 STEP/STP、产品图片和 PDF。
4. 根据产品手册添加功能模板。多个连接点以 `|` 分隔，例如 `A1|A2`。PLC 插头和通道分别填写。
5. 点击“检查资料”，再“生成 EDZ”。可保存 `.epart.json` 资料，下次继续编辑。
6. EPLAN 2.9 中打开“工具 → 部件 → 管理 → 附加 → 导入”，文件类型选择 EDZ。首次用测试数据库，选择“只添加新建数据集”，核对结果。

## STEP 与 3D 宏

STEP/STP 在本版作为源文件附件，关联到“外部文档 2”。程序不会把 STEP 改后缀伪装成宏，也不转换几何模型。

要在 Pro Panel 放置三维模型，先在 EPLAN 2.9 使用“工具 → 生成宏 → 源自 3D 文件”转换成 `.ema`。按实际需求检查放置区域、基准点、安装方向和连接点，再把生成的宏选入软件的“3D / 安装宏”。相关功能取决于现有 EPLAN 模块。

## 本版范围

- 未打开原包时，生成一个电气零部件、一个变量的基础包；打开原包时保留包内其他记录。
- 提供基础部件资料、9 个常用产品组、完整的 2.9 官方功能编号查找、手动端子编辑。
- 原理图宏写入 `P_ARTICLE_GROUPSYMBOLMACRO`，安装宏写入 `P_ARTICLE_MACRO`；两种用途独立，文件同名也不会覆盖。
- STEP 与 PDF 分别作为 `externalDocument2`、`externalDocument1` 附件。图片单独关联。
- EDZ 包含 `manifest.xml`、`items/partxml` 和需要的附件目录。清单采用版本 2.0，使用兼容旧版的 LZMA 7z 容器。
- 导出前检查必填项、字段长度、功能编号、重复连接点、文件缺失及格式；导出后检查压缩完整性。
- 多个连接点使用 EPLAN XML 规定的换行分隔，写出时保留属性换行。
- 新建基础部件仍使用本机文件路径；已有包的资料保存会携带快照与替换文件。EDZ 包含所选附件。
- 可读取 XML 宏头部版本标记供核对，不验证宏几何、表达类型或放置正确性，不从宏自动提取端子或功能模板。
- 不收集宏内部的外部依赖。若宏还依赖其他文件，请先在 EPLAN 中整理为可独立使用的宏。

## 验证边界

本地构建、UI 渲染和离线检查可以证明程序可启动、文件与清单对应、字节保留、XML 字段结构及部分输入检查正确。它们不等于 EPLAN 2.9 实际导入成功，也不证明宏可放置、三维安装方向正确或可自动布线。

文件保留、编辑和导出检查位于独立的 `tests` 项目中，不编译进正式软件。检查结果保存在调用者指定的目录，不提交运行记录到仓库。

## 开发和调试

需要 .NET 8 或更新 SDK。代码采用普通方法和具名事件，中文注释集中解释 XML 映射、文件引用和保存逻辑。

```powershell
dotnet build EplanPartBuilder.csproj -c Release
dotnet publish EplanPartBuilder.csproj -c Release -r win-x64 --self-contained true -o C:\path\publish
```

- `MainForm.cs`：按钮事件和资料编辑。
- `MainForm.Designer.cs`：控件与布局。
- `PartProject.cs`：资料与功能数据。
- `ProjectStore.cs`：JSON 读取、保存与文件原子替换。
- `PartValidator.cs`：导出前检查。
- `EplanXmlWriter.cs`：EPLAN 2.9 XML 字段映射。
- `EdzExporter.cs`：文件归档、清单和 7z 打包。
- `PackageStore.cs`：安全打开目录 / 归档、读取记录、保存和恢复原包快照。
- `PackageEditor.cs`：对原始文档作字段修改，保留其他记录及文件。
- `MainForm.Package.cs`：导入、技术与厂家表单、原始数据核对。
- `tests/EplanPartBuilder.Checks`：独立的基础、部件包和界面检查，不出现在主项目的代码列表中。

正常开发只需打开根目录的 `EplanPartBuilder.sln`，在 Visual Studio 中构建或发布。运行开发检查时使用下列标准命令，无需额外脚本：

```powershell
dotnet run --project tests/EplanPartBuilder.Checks -- --self-check C:\path\checks
dotnet run --project tests/EplanPartBuilder.Checks -- --ui-check C:\path\ui-checks
dotnet run --project tests/EplanPartBuilder.Checks -- --package-check "C:\path\已解开样包目录" C:\path\package-checks
```

部件包检查针对开发时使用的 `DLX.CDB6H632B8` 样包，检查输出目录应为空；检查生成的改名、改图片部件不作为正式产品交付。

## 格式依据

- [EPLAN 2.9：XML 标签结构](https://www.eplan.help/en-us/Infoportal/Content/Plattform/2.9/Content/htm/xmlexport_k_start.htm)
- [EPLAN 2.9：标签及属性](https://www.eplan.help/en-us/Infoportal/Content/Plattform/2.9/Content/htm/xmlexport_o_tags.htm)
- [EPLAN 2.9：功能定义内部编号](https://www.eplan.help/zh-cn/Infoportal/Content/Plattform/2.9/Content/htm/xmlexport_o_funcdef.htm)
- [EPLAN 2.9：创建 3D 宏](https://eplan.help/zh-cn/Infoportal/Content/Plattform/2.9/Content/htm/cabinetgui_h_3dmakroerstellen.htm)
- [ETI 厂家 EPLAN 下载页](https://www.eplan.etigroup.eu/software/catalog/Eplan)：开发时核对公开 C.edz 样包的 7z 容器、manifest 2.0、宏与部件属性引用。厂家宏和部件资料不随本软件分发。

本软件是独立工具，与 EPLAN 官方产品没有隶属关系。
