# File Tag Manager

File Tag Manager 是 Windows 上的標籤管理程式，為檔案添加標籤後，通過搜索取得相對應的檔案，也可以對檔案自訂縮略圖。

> 使用 Mvvm 架構實現關注點分離，並透過依賴注入解決類別間高耦合的問題。
>
> 資料庫為 Sqlite，使用 Dapper ORM 和 Unit of Work 模式進行資料庫的存取。Tag 表用 Toxi solution 設計。搜尋使用全文檢索 (fts5) 加快搜尋速度。
>
> 測試使用 NUnit 和 moq mock 框架。

<p align="center" width="100%">
    <img src="https://uc7d814336e36b7d3ba129424a22.previews.dropboxusercontent.com/p/thumb/ABncgnqFJMbxyLCWSrr2CcfTt91y7l0kZZhVAd9RGwcxYIYhOpaHptdYY5_E4_5Y6xm170upqmF8G0uJdcvXSIc99eH2L1wHEtyRq4CuIXZXpV6jVJt5SIA0Tgg7P3iznXMYCBXHrIMCNWj8p9zXKDrO05xd3xowQc36sdkYn4PP5NahUbgfmNcposM1dfAIVxIKoZfF9BvdBFp0V2_Kr0rW9_Qv9fJKUzgk8a71uM4OmsQtCqJUEQLfq9xWvJ0QgXdECuMpVWORs794-cPIj01m7pnRe2Ngy-hyALgTvy7ek6bzcEATz_Azs0GyzQTgJi0BHEeFWkIOujP4FsiLhiV9YQbizi7Iypoty6nU0C9gdNBTMbU8A6yhNz4It7uZPiln-pyvD1NExzFXdTf1vlW1uOQZj7yfn_1tjesgoPIs9Q/p.gif" alt="file-tag-manager-c" width="85%"/>
</p>



## Table of contents

- [Built With](#built-with)
- [Usage](#usage)
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
- [Nunit](https://github.com/nunit/nunit)
- [moq](https://github.com/moq/moq)



## Usage

Download win-x64：[![](https://img.shields.io/badge/Release-v1.0.0.0-blue.svg?style=for-the-badge)](https://github.com/wilson8299/file-tag-manager/releases/tag/v1.0.0.0)



## Structure

```
File Tree
├── FileTagManager.Database/		# 資料庫相關
│ ├── Data/
│ ├── Repositories/
│ └── UoW/				
├── FileTagManager.Domain/			# 可共用檔案
│ ├── Interfaces/
│ └── Models/	
├── FileTagManager.Test/			# 測試
├── FileTagManager.WPF/				# WPF 
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

選擇目標文件夾。

<p align="center" width="100%">
    <img src="https://uc44b53fce9ca555c2fe5d468419.previews.dropboxusercontent.com/p/thumb/ABnxPi2lsHu8LffuqwYBQ6YvjBBG5_08mAkA94OhN4u97QKikjdJNMlvYYrmXDVWogE4_D40uHC2bEKarXNg8ykWtTe0-1At9f2O6sKlY_1Nafph7aeKQEALibt11Yc7KxXhk-My2cmAjq0_fCP_mbyu1ClWr460wc1EyCL850wF3dPikZUAA9Bp3-tyrs6cUKWNPqc1fnh0dHz2alHVkT8CRXeKBQaExMCvA1y8K833MjQFmkQjtXkZr1803ihB__9JldDGdNNxdOYHe04rlHzrVxNjH7QF-0xVcnpzUMhyP8KVlDjL2QGP09AZqz8Va9RgdGGJap0RFnvDO-sLvHMRyJYFSgHT1s2JDBqeHZy7JoZ3vT4NAG2SfxlJbGOJzrqDBJvroYL2U6FK6n4J9jnzIJP6uWdJ1QwsBZmUBHrESg/p.png" alt="select-directory" width="85%"/>
</p>


## File list

Explorer：目錄結構。

List：檔案顯示。

Info：檔案的資訊，包括更改縮略圖及添加標籤。可以透過逗號一次性添加多個標籤。e.g., tag1, tag2。

> 當有新增或刪除檔案的時候，可以點擊 Explorer 和 List 上的重整按鈕，程式會自動添加或刪除變更的檔案。

<p align="center" width="100%">
    <img src="https://uc3cdc642aa478069aa10caa3157.previews.dropboxusercontent.com/p/thumb/ABmOf0QhJONESD_euOkWFiUunIuV7knO8M2KsBmn1f0AQCFs7d-a7jZtFzppYq08Rw9H9o8PIIAy-ehsYYEy4loawmLdqLT69XpxwEXjTLYPd14Uxt158JFcmy3SCjHl4LYrNIwl-toCsqPj3qP1tCynmmQ3269oNh2bSYvcq3mr81dsowLZlN6Sk9its87-kWzoCQWQh273im6tLeFSOSLglrr7LjpnyTvILKTPHtVjp6XKwgCN-ue8DauKNbCnNp1l15jFs-zBT1KNJIFUTueKFQfVfb3WYlyCgjYogci648N12EVLOzj04iZ0mj0JkTqD9-sgLP5_5SI5j1L79BWmbpjYrYEqauGRMTrDkhuA1N5CbDigZ7ttxNnQWB3WooOxZFQt2Gxx2GVIzi1Y3-EZOT3FExHgwucldnAhnRg5Pw/p.png" alt="file-list" width="85%"/>
</p>



## Search

可以同時使用檔案名稱和標籤進行搜尋。

<p align="center" width="100%">
    <img src="https://uc323d3b75a38f1eb55aa24606d5.previews.dropboxusercontent.com/p/thumb/ABmc0DHd3GWr39K8Q1fFBs0issfFtT115Pvtgw7pVg_KbfsRLnMyrifKN2xODIXJagtdUydYgd15k06_SmOsk4bhOYbZ3A29CBKFpABPzb16tBvXq-AVJ9pZNhILw9IpSLWTntmHJ2GzHEKUP2CG-3sLmLy2UCgXWYIzBbQCCOuWeI7CeHlgRmQPIL2CZwuNz2FQ0XrTVJAOLBjaamckfsvL0K6hG5Eko2VoSUDbx24qhhMum87WufJSEeGhJ4CVxfrODxyds57-_0DGqaYStETttS6z5cysxmfGzm8P8UqoajRTvmzwNZThatPUvLU9KmfwS5KDo0dK5m4txeRJv3xeHjv0vtTx62Wj_j4qGLxMnqcDL0lZgKi1mED1hJgu9Wj2O-NAhNjTveKkLJ6vtu39cUck3aNfXXk4tVmlAbAB2A/p.png" alt="search" width="85%"/>
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
