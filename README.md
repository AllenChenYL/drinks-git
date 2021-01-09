# 飲料揪團系統

## 前言
        為了複習以前工作使用工具技術，以及練習自己開發專案從無到有的過程，
    鑒於現代人每天人手一杯飲料，每天必須樓上揪樓下、厝邊拉隔壁、阿公牽阿嬤、
    爸爸甲媽媽、兄姐帶弟妹，休揪喝涼涼，因此【飲料揪團系統】就這樣誕生了。

## 功能描述

* 使用者登入
* 新增店家頁面
* 新增飲料團購

## 開發環境
__工具__: Visual Studio 2013 for Web    
__資料庫__: MSSQL 2014  
__前端:__ kendo ui v2020.3.1118,  JQuery, Bootstrap3  
__後端:__ C#, .net Framework4.5, EntityFramework  
__第三方套件__: Nuget AutoMapper

## 專案結構簡介
![This is a alt text.](https://github.com/Chen-Yi-Lun/drinks-git/blob/main/images/projectTree.JPG?raw=true "This is a sample image.")  

1. App_Code: 放自己須要使用的Code。
1. App_Start: 一些程式啟動時綁定的套件，**如: AutoMapper**。
1. Contorllers: **Order和Store**為主要使用的Controller。
1. Filters: 放用來驗證使用者是否登入的Code。
1. Models: 放置資料表與產生class的地方，主要有**Store、Order與OrderDetail**。
    1. 分別為**店家管理、訂單管理、跟團人資料**。
1. Scripts: **Global、Order和Store**，處理View需要的互動功能。
1. ViewModels: 增加欄位給View用，如:有無超過時間，創團者姓名繼承Model。
1. Views: **Order、Store和Layout**，大多使用kendo MVVM visible做畫面切換。  

## 畫面截圖說明
![This is a alt text.](https://github.com/Chen-Yi-Lun/drinks-git/blob/main/images/image.JPG?raw=true "This is a sample image.")  
### ⬆使用者登入畫面  
使用專案內自帶的登入系統，利用UserId當作CreateId  

![This is a alt text.](https://github.com/Chen-Yi-Lun/drinks-git/blob/main/images/addImageURLView.JPG?raw=true "This is a sample image.")  
### ⬆店家新增頁面
從網路上尋找店家Menu圖片，右鍵`複製圖片位址`，貼上後按下`OK`會顯示圖片

![This is a alt text.](https://github.com/Chen-Yi-Lun/drinks-git/blob/main/images/addStoreView.JPG?raw=true "This is a sample image.")  
### ⬆店家新增頁面
輸入資料後按確認，按下`確認`即儲存**店家資訊**。  
**ex: 電話地址若不知道填"無"即可。**  

![This is a alt text.](https://github.com/Chen-Yi-Lun/drinks-git/blob/main/images/storeView.JPG?raw=true "This is a sample image.")  
### ⬆飲料店管理清單  
`新增店家`、`刪除`與`編輯`店家資訊，以上為飲料店(增)的功能。

![This is a alt text.](https://github.com/Chen-Yi-Lun/drinks-git/blob/main/images/groupView.JPG?raw=true "This is a sample image.")  
### ⬆飲料團購清單
系統登入後，首先會看到這個畫面，左上角點擊`新增團購`，選擇店家後可以自行建團。  
1. `跟團`新增/修改飲料資料、`取消`刪除飲料資料、`檢視` 查看目前有哪些人跟團 
1. **超過截止時間**無法刪除並顯示紅色只允許檢視。  
1. **截止時間內**顯示淡紫色並可以刪除飲料團。  

![This is a alt text.](https://github.com/Chen-Yi-Lun/drinks-git/blob/main/images/cardStoreView.JPG?raw=true "This is a sample image.")  
### ⬆選擇開團飲料店頁面
點擊`新增團購`後，若有店家資料，會顯示卡片式資訊，按下開團輸入截止日期，  
確認成功後自動切到**飲料團購**畫面。

![This is a alt text.](https://github.com/Chen-Yi-Lun/drinks-git/blob/main/images/followGroupView.JPG?raw=true "This is a sample image.")  
### ⬆飲料資料輸入頁面
1. 飲料團購清單中，點擊`跟團`。  
1. 再點擊`新增飲料`出現新的一列欄位供填寫。  
1. 欄位輸入完後按下`Tab鍵`跳下一格。
    1. 輸入過程可以點擊`(顯示/隱藏)飲料目錄`會出現圖片供使用者參考
1.輸入完後右上角`儲存`，即完成**跟團**動作。  
**注意: 若想修改飲料內容，再按一次`跟團`即可進入頁面修改。**  

![This is a alt text.](https://github.com/Chen-Yi-Lun/drinks-git/blob/main/images/ordererView.JPG?raw=true "This is a sample image.")  
### ⬆訂單檢視頁面
飲料團購清單中，點擊`檢視`，出現所有訂購人清單，店家資訊供揪團人參考  
，可匯出Csv提供檔案留存。  
**注意:excel開啟時會出現亂碼，要上網找一下設定UTF-8開啟，並以逗號隔開**

## 問題與處理

1. 建立SQL資料庫的出現錯誤  
    1. C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA  
    資料夾右鍵->內容->下面進階->壓縮內容，節省磁碟空間(取消打勾)，即可正常建立DB
1. Order與OrderDetail轉成Json時無限循環
    1. 可參考專案內App_Code的**BaseController**，Override原本的Json寫法  
    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore  
    要讓原本的Controller:BaseController，才能正常運行。
1. 減少AutoMapper需一直寫Mapper.CreateMap的方法
    1. App_Start內新增AutoMapperConfig，將Mapper.CreateMap寫在裡面後  
  點擊專案內Global.asax檔，註冊AutoMapperConfig.Configure();後完成。
1. 讓kendo grid 能夠使用tab加速使用者輸入資料的速度
    1. 在order.js內官方提供由上而下的輸入方式，已改成由左而右方式，  
    grid.table.on('keydown', function (e)找到這一行開始 (目前為261行)。
1. 發現每次瀏覽器Load頁面有夠慢。
    1. BroswerLink導致Load變慢，Web.config加入<add key="vs:EnableBrowserLink" value="false" />關起來。
1. 因為檔案太大關係，所以省略整個專案將Packages及bin檔上傳至github，  
但出現**Nuget基礎連結已關閉問題**，導致無法下載Packages套件。
    1. 工具->Nuget封裝管理員->套件管理員設定->套件來源->nuget.org  
    將https://www.nuget.org/api/v2/ 內改成http://www.nuget.org/api/v2/
    1. 如果上面失敗，可以自己上nuget官方抓取相關文件版本，console安裝(套件太多麻煩)
    1. 複製下方Code到txt檔案內，存成.reg檔，點擊兩下登陸成功後，重開VS進入Nuget。  
    __(因為動到系統檔不建議使用，會怕的話再自行找方法)__
```
Windows Registry Editor Version 5.00
[HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\.NETFramework\v4.0.30319]
"SchUseStrongCrypto"=dword:00000001
[HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\.NETFramework\v4.0.30319]
"SchUseStrongCrypto"=dword:00000001
```

## 可改進部分

- [x] Controller
    - [x] 將處理資料部分切成Service與Dao
    - [x] Service負責處理資料Dao負責撈資料，讓Controller只作為View與Model橋梁
- [ ] [飲料團購]旁可以標示(number)目前未截止團數數量
- [ ] 新增團購-> 卡片式畫面可以用Ajax撈取，當店家數多的時候，可以讓使用者選取部分店家
   - [ ] 在Store資料表內加個欄位，把同屬性的店家套在一起讓畫面篩出，使用者快速選取。
- [x] csv修改成匯出excel檔案
   - [ ] 加入店家資訊(店名、電話、地址)
- [x] 新增價格欄位讓使用者自己填
    - [x] 試算每個欄位金額並總和
- [ ] 待....
