using Smod2;
using Smod2.Attributes;

namespace DisconnectProject
{
    [PluginDetails(
        author = "cushaw",
        name = "DisconnectProject",
        description = "DisconnectProject",
        id = "cushaw.DisconnectProject",
        version = "1.2",
        SmodMajor = 3,
        SmodMinor = 1,
        SmodRevision = 22
    )]
    class DisconnectDrop : Plugin
    {
        public override void OnDisable()
        {
            this.Info("断线保护没有加载成功/plugin not load");
        }

        public override void OnEnable()
        {
            this.Info("断线保护加载成功/plugin loaded");
        }

        public override void Register()
        {
            this.AddEventHandlers(new EventHandler(this));
            this.AddConfig(new Smod2.Config.ConfigSetting("dp_enable", true, Smod2.Config.SettingType.BOOL, true, "plugin enable/disable"));
            this.AddConfig(new Smod2.Config.ConfigSetting("dp_warhead", true, Smod2.Config.SettingType.BOOL, true, "enable/disable DisconnectProject after warhead used"));
            this.AddConfig(new Smod2.Config.ConfigSetting("dp_language", 1, Smod2.Config.SettingType.NUMERIC, true, "info language"));
            this.AddConfig(new Smod2.Config.ConfigSetting("dp_time", 60, Smod2.Config.SettingType.NUMERIC, true, "project time"));
        }
    }
}
