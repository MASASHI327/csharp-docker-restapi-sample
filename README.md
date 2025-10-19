### 技術構成
●C# \
●ASP.NET Core Web API\
●EntityFrameworkCore\
●Sqlserver\
●Xunit

### API構成 

  |             | メソッド      | URI           | 
| --- | --- | --- |     
|Todoデータを全取得   |  GET     | /apo/Todolist | 
|Todoデータのidに紐づくTodoデータを取得  |  GET    | /apo/Todolist/:id | 
|Todoデータの新規作成 |  POST | /apo/Todolist| 
|Todoデータの更新 |  PUT | /apo/Todolist/:id| 
|Todoデータの削除| DELETE | /apo/Todolist/:id| 


### 目的: チーム開発で同一環境を再現するためのDocker構成

### 起動方法: `docker compose up -d`