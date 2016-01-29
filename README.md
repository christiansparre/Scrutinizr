# Scrutinizr
Simple tool to inspect HTTP requests

## How to run

Clone the repo and make sure you have ASP.NET Core 1.0 (previously know as ASP.NET 5) installed. Get it here: https://get.asp.net/

Maybe you need to run
```
dnvm use 1.0.0-rc1-update1
```
Then you should be able to run
```
dnu restore
dnx -p .\src\Scrutinizr\ web
```
And open http://localhost:5000 in your browser
