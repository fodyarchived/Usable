[![Chat on Gitter](https://img.shields.io/gitter/room/fody/fody.svg?style=flat)](https://gitter.im/Fody/Fody)
[![NuGet Status](http://img.shields.io/nuget/v/Usable.Fody.svg?style=flat)](https://www.nuget.org/packages/Usable.Fody/)


## Usable is an add-in for [Fody](https://github.com/Fody/Home/)

![Usable Icon - A waste paper basket.](https://raw.github.com/Fody/Usable/master/Icons/package_icon.png)

Usable adds using statements for local variables that have been created, and implement [IDisposable](http://msdn.microsoft.com/en-au/library/system.idisposable.aspx).


## NuGet installation

Install the [Usable.Fody NuGet package](https://nuget.org/packages/Usable.Fody/) and update the [Fody NuGet package](https://nuget.org/packages/Fody/):

```powershell
PM> Install-Package Fody
PM> Install-Package Usable.Fody
```

The `Install-Package Fody` is required since NuGet always defaults to the oldest, and most buggy, version of any dependency.


### Your Code

```csharp
public void Method()
{
    var w = File.CreateText("log.txt");
    w.WriteLine("I'm a lumberjack an' I'm ok.");
}
```


### What gets compiled

```csharp
public void Method()
{
    using (var w = File.CreateText("log.txt"))
    {
        w.WriteLine("I'm a lumberjack an' I'm ok.");
    }
}
```


## Contributors

  * [Cameron MacFarland](https://github.com/distantcam)
  * [Jason Woods](https://github.com/jasonwoods-7)


### Icon

<a href="http://thenounproject.com/noun/trash/#icon-No12100" target="_blank">Trash</a> designed by <a href="http://thenounproject.com/swu1381" target="_blank">Shirley Wu</a> from The Noun Project