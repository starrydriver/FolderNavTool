using System;
using System.Collections.Generic;
using Eto.Forms;
using Eto.Drawing;
using Eto;
using System.IO;
namespace 文件夹定位工具;
class Program
{
    [STAThread]
    static void Main()
    {
        var app = new Application(Platforms.WinForms);
        app.Run(new MyForm());
    }
}
class MyForm : Form
{
    private readonly MyTableLayout tableLayout = new MyTableLayout();
    public MyForm()
    {
        Title = "文件夹定位工具";
        Maximizable = false;  // 禁用最大化按钮
        Minimizable = true;     // 保留最小化按钮（可选）
        Resizable = false;      // 禁止调整窗口大小
        ClientSize = new Size(600, 400);
        BackgroundColor = Color.FromArgb(230, 230, 230);// 经典灰色背景
        Content = myLayout();
        
    }
    private StackLayout myLayout()
    {
        var layout = new StackLayout();
        layout.Spacing = 15;
        //1行排列 文件夹的选取与添加
        var layout1 = new DynamicLayout();
        layout1.Padding = 10;
        layout1.Spacing = new Size(10, 10);
        layout1.BeginHorizontal();
        var folderPicker = new FolderPicker(tableLayout);
        layout1.Add(folderPicker.myTextBox());
        layout1.Add(folderPicker.mySelectButton());
        layout1.Add(folderPicker.myAddButton());
        layout1.EndHorizontal();
        layout.Items.Add(layout1);
        layout.Items.Add(tableLayout);
        return layout;
    }
    private DynamicLayout myLayout2()
    {
        var layout = new DynamicLayout();
        layout.Padding = 10;
        layout.Spacing = new Size(10, 15);
        //1行排列 文件夹的选取与添加
        layout.BeginHorizontal();
        var folderPicker = new FolderPicker(tableLayout);
        layout.Add(folderPicker.myTextBox());
        layout.Add(folderPicker.mySelectButton());
        layout.Add(folderPicker.myAddButton());
        layout.EndHorizontal();
        //2列排列 
        layout.BeginVertical();
        layout.Add(tableLayout);
        layout.EndVertical();
        return layout;
    }

}
class FolderPicker
{
    private readonly MyTableLayout tableLayout = new MyTableLayout();
    public TextBox textBox { get; set; } = new();
    public List<string> listFileBox { get; set; }= new();

    public FolderPicker(MyTableLayout tableLayout)
    {
        this.tableLayout = tableLayout;

    }
    public TextBox myTextBox()
    {
        textBox.PlaceholderText = "文件夹路径";
        textBox.Width = 400;
        textBox.Height = 25;
        textBox.BackgroundColor = Color.FromArgb(255, 255, 255);// 白色文本框
        return textBox;
    }
    public Button mySelectButton()
    {
        var button = new Button();
        button.Text = "浏览...";
        button.Size = new Size(80, 25);
        button.Click += (sender, e) =>
        {
            // 创建文件夹选择对话框
            var dialog = new SelectFolderDialog();
            dialog.Title = "选择目标文件夹";
            // 显示对话框并获取结果
            var result = dialog.ShowDialog(Application.Instance.MainForm);
            if (result == DialogResult.Ok)
            {
                // 获取选择的文件夹路径并显示在文本框中
                textBox.Text = dialog.Directory;
                Console.WriteLine($"选择的文件夹路径: {dialog.Directory}"); // 调试输出
            }
        };
        return button;
    }
    public Button myAddButton()
    {
        var button = new Button();
        button.Text = "添加+";
        button.Size = new Size(80, 25);
        button.Click += (sender, e) =>
        {
            // 获取文本框中的文件夹路径
            var path = textBox.Text;
            // 判断路径是否有效
            if (Directory.Exists(path))
            {// 添加到列表中
                    listFileBox.Add(path);
                    //添加表格
                    tableLayout.addItem(path);
                //判断是否已经存在于列表中
                if (!listFileBox.Contains(path))
                {
                    
                }
            }
        };
        return button;
    } 
}
class  MyTableLayout : DynamicLayout
{
    public int itemCount { get; set; } = 1;
    public List<TableLayoutItem> itemList { get; set; } = new();

    public MyTableLayout()
    {
        Padding = 10;
        Spacing = new Size(10, 10);
        // 初始添加一个不可见的占位控件
        BeginHorizontal();
        var item = new TableLayoutItem("111");
        Add(item.container, xscale: false, yscale: false);
        //EndHorizontal();
    }
    public void addItem(string filePath)
    {
        var item = new TableLayoutItem(filePath);
        //动态添加控件
        Application.Instance.AsyncInvoke(() =>
        {
            Add(item.container);
            Create(); //强制布局更新
            if (itemCount % 4 == 0)
            {
                EndHorizontal();
                BeginHorizontal();
            }
        });
        itemList.Add(item);
        itemCount++;
    }
}
class TableLayoutItem
{
    public StackLayout container {get; set; }=new ();
    public Label textBox { get; set; } = new();
    // 正确的构造函数声明（没有返回类型）
    public TableLayoutItem(string filePath)
    {
        container.Spacing = 2;
        container.Items.Add(myTextBox(filePath));
        container.Items.Add(myNavButton());
    }
    public Label myTextBox(string name)
    {
        textBox.Width = 137;
        textBox.Height = 100;
        textBox.Text = name;
        textBox.BackgroundColor = Color.FromArgb(255, 255, 255);// 白色文本框
        return textBox;
    }

    public Button myNavButton()
    {
        var button = new Button();
        button.Text = "浏览...";
        button.Size = new Size(137, 40);
        button.Click += (sender, e) =>
        {
            //打开文件夹
            System.Diagnostics.Process.Start("explorer.exe", textBox.Text);
        };
        return button;
    }
}
