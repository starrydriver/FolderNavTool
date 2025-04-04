
<style>
  .lang-switcher {
    text-align: right;
    margin-bottom: 20px;
  }
  .lang-btn {
    background: none;
    border: 2px solid #4285f4;
    color: #4285f4;
    padding: 5px 15px;
    border-radius: 5px;
    cursor: pointer;
    font-weight: bold;
    transition: all 0.3s;
    margin-left: 5px;
  }
  .lang-btn.active {
    background: #4285f4;
    color: white;
  }
  .lang-btn:hover {
    opacity: 0.8;
  }
</style>
```html
<div class="lang-switcher">
  <button class="lang-btn active" onclick="switchLang('zh')">中文</button>
  <button class="lang-btn" onclick="switchLang('en')">English</button>
</div>

<div id="zh-content">
# 文件夹快速导航工具

## 当前状态
🚧 待完成

## 开发历程
**4月3日-4月4日**  
🔥 拼尽全力无法隐藏横向滑动栏  

面对仅仅数百kb的轻量级，顽固不堪的滚动条问题令人身心俱疲  

🌙 连夜更换框架  

连夜转到 Avalonia 框架  
然后耗时半日陷入与 MVVM 架构"搏斗"  

🎉 最终成果  

成功完成 UI 界面重构  
交互功能依旧停滞不前😆
</div>

<div id="en-content" style="display:none">
# Folder Quick Navigation Tool

## Current Status
🚧 Work in Progress

## Development Journey
**April 3-4**  
🔥 Struggled relentlessly with the stubborn horizontal scrollbar  

The mere few hundred KB lightweight component exhausted me with its unyielding scrollbar issue  

🌙 Overnight Framework Switch  

Migrated to Avalonia framework overnight  
Then spent half a day wrestling with MVVM architecture  

🎉 Final Achievement  

Successfully completed UI redesign  
Interactive features remain at a standstill 😆
</div>
```
<script>
function switchLang(lang) {
  // Toggle content visibility
  document.getElementById('zh-content').style.display = 
    lang === 'zh' ? 'block' : 'none';
  document.getElementById('en-content').style.display = 
    lang === 'en' ? 'block' : 'none';
  
  // Update button styles
  document.querySelectorAll('.lang-btn').forEach(btn => {
    btn.classList.toggle('active', 
      (lang === 'zh' && btn.textContent === '中文') || 
      (lang === 'en' && btn.textContent === 'English')
    );
  });
  
  // Save preference
  localStorage.setItem('preferredLang', lang);
}

// Load preferred language
document.addEventListener('DOMContentLoaded', function() {
  const lang = localStorage.getItem('preferredLang') || 'zh';
  switchLang(lang);
});
</script>

