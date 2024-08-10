using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class FileSystemItem
{
    public string Name { get; set; }

    protected FileSystemItem(string name)
    {
        Name = name;
    }

    public abstract void Display(string indent = "");
}

class File : FileSystemItem
{
    public string Content { get; set; }

    public File(string name, string content) : base(name)
    {
        Content = content;
    }

    public override void Display(string indent = "")
    {
        Debug.Log($"{indent}File: {Name}");
    }
}

class Directory : FileSystemItem
{
    private List<FileSystemItem> items;

    public Directory(string name) : base(name)
    {
        items = new List<FileSystemItem>();
    }

    public void Add(FileSystemItem item)
    {
        items.Add(item);
    }

    public override void Display(string indent = "")
    {
        Debug.Log($"{indent}Directory: {Name}");
        foreach (var item in items)
        {
            item.Display(indent + "  ");
        }
    }
}