```html
<!-- 切换按钮 -->
<div style="text-align: right; margin-bottom: 20px;">
  <button onclick="switchLang('zh')">中文</button>
  <button onclick="switchLang('en')">English</button>
</div>

<!-- 中英文内容容器 -->
<div id="zh-content">
  ## 文件夹快速导航工具
  - 4月3日-4月4日：与横向滚动条搏斗未果
  - 连夜迁移至 Avalonia 框架
  - MVVM 模式初战告捷
</div>

<div id="en-content" style="display: none;">
  ## Folder Navigation Tool
  - Apr 3-4: Failed to hide horizontal scrollbar
  - Switched to Avalonia overnight
  - First victory with MVVM pattern
</div>

<script>
function switchLang(lang) {
  document.getElementById('zh-content').style.display = 
    lang === 'zh' ? 'block' : 'none';
  document.getElementById('en-content').style.display = 
    lang === 'en' ? 'block' : 'none';
}
</script>
```
