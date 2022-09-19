# File Tag Manager

File Tag Manager 是 Windows 上的標籤管理程式，為檔案添加標籤後，通過搜索取得相對應的檔案，也可以對檔案自訂縮略圖。

> 使用 Mvvm 架構實現關注點分離，並透過依賴注入解決類別間高耦合的問題。
>
> 資料庫為 Sqlite，使用 Dapper ORM 和 Unit of Work 模式進行資料庫的存取。搜尋使用全文檢索 (fts5) 加快搜尋速度。
>
> 測試使用 NUnit 和 moq mock 框架。

<p align="center" width="100%">
    <img src="https://imgur.com/1KMI1h3.gif" alt="file-tag-manager-c" width="85%"/>
</p>


## Table of contents

- [Built With](#built-with)
- [Download](#download)
- [Structure](#structure)
- [Init](#init)
- [File list](#file-list)
- [Search](#search)
- [Import & Export](#import--export)



## Built With

- [Windows Presentation Foundation (WPF)](https://github.com/dotnet/wpf) 
- [CommunityToolkit.Mvvm](https://github.com/CommunityToolkit/WindowsCommunityToolkit) 
- [MaterialDesignInXamlToolkit](https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit)
- [Autofac](https://autofac.org/) 
- [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json)
- [Sqlite](https://www.sqlite.org/index.html) 
- [Dapper](https://github.com/DapperLib/Dapper)
- [NLog](https://github.com/NLog/NLog)
- [Nunit](https://github.com/nunit/nunit)
- [Moq](https://github.com/moq/moq)



## Download

Download win-x64：[![](https://img.shields.io/badge/Release-v1.0.0.0-blue.svg?style=for-the-badge)](https://github.com/wilson8299/file-tag-manager/releases/tag/v1.0.0.0)



## Structure

```
File Tree
├── FileTagManager.Database/        # 資料庫相關
│ ├── Data/
│ ├── Repositories/
│ └── UoW/				
├── FileTagManager.Domain/          # 可共用檔案
│ ├── Interfaces/
│ └── Models/	
├── FileTagManager.Test/            # 測試
├── FileTagManager.WPF/             # WPF 
│ ├── Assets/
│ ├── AttachedProperties/
│ ├── Dll/
│ ├── Helpers/
│ ├── Messages/
│ ├── Services/
│ ├── Styles/
│ ├── ViewModels/
│ └── Views/
```



## Init

選擇目標資料夾。

<p align="center" width="100%">
    <img src="https://imgur.com/jAztUaQ.png" alt="select-directory" width="85%"/>
</p>


## File list

Explorer：目錄結構。

List：檔案顯示。

Info：檔案的資訊，包括更改縮略圖及添加標籤。可以透過逗號一次性添加多個標籤。e.g., tag1, tag2。

> 當有新增或刪除檔案的時候，可以點擊 Explorer 和 List 上的重整按鈕，程式會自動添加或刪除變更的檔案。

<p align="center" width="100%">
    <img src="https://imgur.com/oPFSGjt.png" alt="file-list" width="85%"/>
</p>



## Search

可以同時使用檔案名稱和標籤進行搜尋。

<p align="center" width="100%">
    <img src="https://imgur.com/v2qoGAx.png" alt="search" width="85%"/>
</p>


## Import & Export

支持匯入和匯出標籤及自訂的縮略圖。

```json
[
  {
    "Name": "bird",
    "Tag": "tag1",
    "ThumbnailByte": null
  },
  {
    "Name": "dog",
    "Tag": "tag2,tag2-1,tag2-2",
    "ThumbnailByte": null
  }
]
```
