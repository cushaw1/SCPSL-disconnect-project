# SCPSL-disconnect-project
# What is it? | 这是啥？
This is a plugin for SCP: Secret Laboratory | 这是SCP:秘密实验室的插件
# plugin content | 插件内容
If player connects after disconnecting (within the same round), he recovers the role he was before（recovery player role,hp,item,ammo,position）

如果玩家在掉线保护结束前加入掉线服务器，将会恢复掉线前的职业血量物品子弹和地点：

PS：only protcet once,do not recovery player when disconncet again

PS：只保护一次,再次掉线不保护

# How do I use it? | 我如何使用它？
download the dll from [here](https://github.com/cushaw1/SCPSL-disconnect-project/releases)
| 点击下载 [插件](https://github.com/cushaw1/SCPSL-disconnect-project/releases)

put it in the folder titled sm_plugins.
| 把插件放入sm_plugins文件夹内

set online_mode: true,because it use steamID
| 设置online_mode: true，因为插件使用了steamID

# Config Options （English）
Config Option | Value Type | Default Value | Description
--- | :---: | :---: | ---
dp_enable | Boolean | True | plugin enable/disable
dp_warhead | Boolean | True | player can not recovery after warhead boom enable/disable
dp_language | Integer | 1 | sever info language,0 is english,1 is chinese
dp_time | Integer | 60 | protect time(s)

# 参数设置 （中文）
参数名 | 参数类型 | 默认值 | 注释
--- | :---: | :---: | ---
dp_enable | Boolean | True | 开启/关闭插件
dp_warhead | Boolean | True | 开启/关闭核弹炸后玩家不复活
dp_language | Integer | 1 | 服务器info信息语言,0是英文,1是中文
dp_time | Integer | 60 | project time(s) 保护时间(秒)
