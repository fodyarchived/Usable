## Usable is an add-in for [Fody](https://github.com/Fody/Fody/) 

![Usable Icon - A waste paper basket.](https://raw.github.com/Fody/Usable/master/Icons/package_icon.png)

Usable adds using statements for local variables that have been created, and implement [IDisposable](http://msdn.microsoft.com/en-au/library/system.idisposable.aspx).

## The nuget package  [![NuGet Status](http://img.shields.io/nuget/v/Usable.Fody.svg?style=flat)](https://www.nuget.org/packages/Usable.Fody/)

https://nuget.org/packages/Usable.Fody/

    PM> Install-Package Usable.Fody

### Your Code

    public void Method()
    {
        var w = File.CreateText("log.txt");
        w.WriteLine("I'm a lumberjack an' I'm ok.");
    }

### What gets compiled

    public void Method()
    {
        using (var w = File.CreateText("log.txt"))
        {
            w.WriteLine("I'm a lumberjack an' I'm ok.");
        }
    }

## Contributors

  * [Cameron MacFarland](https://github.com/distantcam)

### Icon

<a href="http://thenounproject.com/noun/trash/#icon-No12100" target="_blank">Trash</a> designed by <a href="http://thenounproject.com/swu1381" target="_blank">Shirley Wu</a> from The Noun Project
