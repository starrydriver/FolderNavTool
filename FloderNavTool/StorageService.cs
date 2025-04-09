using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using FloderNavTool.ViewModels;

namespace FloderNavTool
{
    public class StorageService
    {
        private readonly string _storagePath;

        public StorageService()
        {
            // 获取应用程序目录下的json文件夹路径
            var appDirectory = AppContext.BaseDirectory;
            _storagePath = Path.Combine(appDirectory, "json", "storage.json");

            // 确保目录存在
            Directory.CreateDirectory(Path.GetDirectoryName(_storagePath));
        }

        public AppState Load()
        {
            if (!File.Exists(_storagePath))
            {
                return new AppState
                {
                    IdCount = 1,
                    RowCount = 2,
                    NavItems = new List<NavItemState>()
                };
            }

            var json = File.ReadAllText(_storagePath);
            return JsonConvert.DeserializeObject<AppState>(json);
        }

        public void Save(AppState state)
        {
            var json = JsonConvert.SerializeObject(state, Formatting.Indented);
            File.WriteAllText(_storagePath, json);
        }
    }

    public class AppState
    {
        public int IdCount { get; set; }
        public int RowCount { get; set; }
        public List<NavItemState> NavItems { get; set; }
    }

    public class NavItemState
    {
        public int Id { get; set; }
        public string FolderName { get; set; }
        public string FolderText { get; set; }
    }
}