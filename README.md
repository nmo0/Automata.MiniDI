# Automata.MiniDI

标签（空格分隔）： .NET Develop

---

## 简介
Mini Dependency Injection

## Example
```
var service = new ServiceCollection();

service.AddTransient<IMyService, MyService>();

var builder = service.BuildServiceProvider();

var myService = builder.GetService<IMyService>();
```
