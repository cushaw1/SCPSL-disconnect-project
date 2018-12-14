using Smod2;
using Smod2.Events;
using Smod2.EventHandlers;
using System.Collections.Generic;
using UnityEngine;
using Smod2.API;

namespace DisconnectProject
{
    class EventHandler : IEventHandlerPlayerJoin, IEventHandlerFixedUpdate, IEventHandlerRoundStart, IEventHandlerRoundEnd, IEventHandlerRoundRestart, IEventHandlerWarheadDetonate
    {
        private Plugin plugin;
        private Server server;
        public Dictionary<string, List<Smod2.API.Item>> item;
        private List<string> blacklist = new List<string>();
        private List<string> playertime = new List<string>();
        public Dictionary<string, int> hp;
        public Dictionary<string, int> role;
        public Dictionary<string, List<float>> d;
        private bool en = true;
        private bool start = false;
        private bool round = false;
        private bool warhead = true;
        private bool boom = false;
        private Dictionary<string, int> dtime;
        private Dictionary<string, int> ammo5;
        private Dictionary<string, int> ammo7;
        private Dictionary<string, int> ammo9;
        private int language = 1;
        private int projcettime = 60;
        public static List<string> playerid;
        private float pTime;
        public EventHandler(Plugin plugin)
        {
            this.plugin = plugin;
            server = plugin.pluginManager.Server;
            pTime = 0;
            item = new Dictionary<string, List<Smod2.API.Item>>();
            d = new Dictionary<string, List<float>>();
            hp = new Dictionary<string, int>();
            role = new Dictionary<string, int>();
            dtime = new Dictionary<string, int>();
            ammo5 = new Dictionary<string, int>();
            ammo7 = new Dictionary<string, int>();
            ammo9 = new Dictionary<string, int>();
            blacklist = new List<string>();
            playerid = new List<string>();
            playertime = new List<string>();
            language = 1;
            projcettime = 60;
            en = true;
            warhead = true;
            boom = false;
            start = false;
            round = false;
        }

        public void OnPlayerJoin(PlayerJoinEvent ev)
        {
            if (en == true && start == true && playertime.Contains(ev.Player.SteamId))
            {
                playertime.Remove(ev.Player.SteamId);
                dtime[ev.Player.SteamId] = 0;
                playerid.Add(ev.Player.SteamId);
            }
            if (en == true && start == true && boom && warhead)
            {
                if (language == 1) plugin.Info(ev.Player.Name + " 不能复活，因为核弹炸后概不复活");
                if (language == 0) plugin.Info(ev.Player.Name + " can not spawn，because warhead boommmmmmmm!");
            } 
            if (warhead != true) boom = false;
            if (en == true && start == true && playerid.Contains(ev.Player.SteamId) && boom == false)
            {
                if (ev.Player.TeamRole.Team == Smod2.API.Team.NONE || ev.Player.TeamRole.Team == Smod2.API.Team.SPECTATOR)
                {
                    bool result = false;
                    foreach (string name in blacklist)
                    {
                        if (name == ev.Player.SteamId)
                        {
                            result = true;
                            if (language == 1) plugin.Info(ev.Player.Name + " 这个菜鸡已经掉线过或掉线时间过久，所以不提供掉线保护");
                            if (language == 0) plugin.Info(ev.Player.Name + " disconnet again or disconnect too long time，do not provide project");
                            break;
                        }
                    }
                    if (role[ev.Player.SteamId] != 2 && !result)
                    {
                        if (language == 1) plugin.Info(ev.Player.Name + " 这个菜鸡刚刚掉线了，系统把他刷回来");
                        if (language == 0) plugin.Info(ev.Player.Name + " disconncet，project start");
                        ev.Player.ChangeRole((Role)role[ev.Player.SteamId]);
                        foreach (Smod2.API.Item b in ev.Player.GetInventory())
                        {
                            b.Remove();
                        }
                        var loc = d[ev.Player.SteamId];
                        ev.Player.Teleport(new Vector(loc[0], loc[1], loc[2]));
                        ev.Player.SetHealth(hp[ev.Player.SteamId]);
                        ev.Player.SetAmmo(AmmoType.DROPPED_5, ammo5[ev.Player.SteamId]);
                        ev.Player.SetAmmo(AmmoType.DROPPED_7, ammo7[ev.Player.SteamId]);
                        ev.Player.SetAmmo(AmmoType.DROPPED_9, ammo9[ev.Player.SteamId]);
                        foreach (var item in item[ev.Player.SteamId])
                        {
                            ev.Player.GiveItem(item.ItemType);
                        }
                        blacklist.Add(ev.Player.SteamId);
                    }
                }
            }
        }
        public void OnRoundStart(RoundStartEvent ev)
        {
            en = plugin.GetConfigBool("dp_enable");
            language = plugin.GetConfigInt("dp_language");
            projcettime = plugin.GetConfigInt("dp_time");
            item = new Dictionary<string, List<Smod2.API.Item>>();
            playertime = new List<string>();
            d = new Dictionary<string, List<float>>();
            hp = new Dictionary<string, int>();
            role = new Dictionary<string, int>();
            dtime = new Dictionary<string, int>();
            blacklist = new List<string>();
            playerid = new List<string>();
            if (server.GetPlayers().Count > 0 && en)
            {
                var players = plugin.Server.GetPlayers();
                for (int i = 0; i < players.Count; i++)
                {
                    var a = players[i];
                    var obj = (GameObject)a.GetGameObject();
                    hp[a.SteamId] = a.GetHealth();
                    item[a.SteamId] = a.GetInventory();
                    ammo5[a.SteamId] = a.GetAmmo(AmmoType.DROPPED_5);
                    ammo7[a.SteamId] = a.GetAmmo(AmmoType.DROPPED_7);
                    ammo9[a.SteamId] = a.GetAmmo(AmmoType.DROPPED_9);
                    if (a.TeamRole.Role != Role.SPECTATOR) d[a.SteamId] = new List<float> { obj.transform.position.x, obj.transform.position.y, obj.transform.position.z };
                    if (a.TeamRole.Role == Role.SCP_173) role[a.SteamId] = 0;
                    if (a.TeamRole.Role == Role.CLASSD) role[a.SteamId] = 1;
                    if (a.TeamRole.Role == Role.SPECTATOR) role[a.SteamId] = 2;
                    if (a.TeamRole.Role == Role.SCP_106) role[a.SteamId] = 3;
                    if (a.TeamRole.Role == Role.NTF_SCIENTIST) role[a.SteamId] = 4;
                    if (a.TeamRole.Role == Role.SCP_049) role[a.SteamId] = 5;
                    if (a.TeamRole.Role == Role.SCIENTIST) role[a.SteamId] = 6;
                    if (a.TeamRole.Role == Role.SCP_079) role[a.SteamId] = 7;
                    if (a.TeamRole.Role == Role.CHAOS_INSUGENCY) role[a.SteamId] = 8;
                    if (a.TeamRole.Role == Role.SCP_096) role[a.SteamId] = 9;
                    if (a.TeamRole.Role == Role.SCP_049_2) role[a.SteamId] = 10;
                    if (a.TeamRole.Role == Role.NTF_LIEUTENANT) role[a.SteamId] = 11;
                    if (a.TeamRole.Role == Role.NTF_COMMANDER) role[a.SteamId] = 12;
                    if (a.TeamRole.Role == Role.NTF_CADET) role[a.SteamId] = 13;
                    if (a.TeamRole.Role == Role.TUTORIAL) role[a.SteamId] = 14;
                    if (a.TeamRole.Role == Role.FACILITY_GUARD) role[a.SteamId] = 15;
                    if (a.TeamRole.Role == Role.SCP_939_53) role[a.SteamId] = 16;
                    if (a.TeamRole.Role == Role.SCP_939_89) role[a.SteamId] = 17;
                    if (!playerid.Contains(a.SteamId)) playerid.Add(a.SteamId);
                }
            }
            start = true;
            round = true;
        }
        public void OnFixedUpdate(FixedUpdateEvent ev)
        {
            if (en == true && start == true)
            {
                pTime -= Time.fixedDeltaTime;
                if (server.GetPlayers().Count > 0 && pTime < 0)
                {
                    pTime = 1;
                    List<string> displayer = new List<string>();
                    displayer.AddRange(playerid);
                    foreach (Player player in server.GetPlayers())
                    {
                        displayer.Remove(player.SteamId);
                    }
                    if (displayer.Count > 0)
                    {
                        for (int j = 0; j < displayer.Count; j++)
                        {
                            bool ok = false;
                            foreach (string name in blacklist)
                            {
                                if (name == displayer[j])
                                {
                                    ok = true;
                                    if (language == 1) plugin.Info("玩家 " + displayer[j] + " 再次掉线了，不提供保护");
                                    if (language == 0) plugin.Info("player " + displayer[j] + " disconnect again，do not provide project");
                                    break;
                                }
                            }
                            if (ok == false)
                            {
                                if (language == 1) plugin.Info("玩家 " + displayer[j] + " 掉线了，有" + projcettime + "秒的保护时间");
                                if (language == 0) plugin.Info("player " + displayer[j] + " disconnect，" + projcettime + "second to project");
                            }
                            playertime.Add(displayer[j]);
                            playerid.Remove(displayer[j]);
                            dtime[displayer[j]] = 0;
                        }
                    }
                    displayer.Clear();
                    List<string> dis = new List<string>();
                    for (int i = 0; i < playertime.Count; i++)
                    {
                        if (dtime[playertime[i]] >= 0 && dtime[playertime[i]] < projcettime) dtime[playertime[i]]++;
                        if (dtime[playertime[i]] == projcettime)
                        {
                            if (language == 1) plugin.Info("玩家 "+ playertime[i] + " 的" + projcettime + "秒保护时间已过，将不提供断线保护");
                            if (language == 0) plugin.Info("player " + playertime[i] + " " + projcettime + "s project time out，do not provide project");
                            dis.Add(playertime[i]);
                            playerid.Remove(playertime[i]);
                            if (!blacklist.Contains(playertime[i]))blacklist.Add(playertime[i]);
                        } 
                    }
                    for (int d = 0; d < dis.Count; d++)
                    {
                        playertime.Remove(dis[d]);
                    }
                    dis.Clear();
                    for (int k = 0; k < plugin.Server.GetPlayers().Count; k++)
                    {
                        var a = plugin.Server.GetPlayers()[k];
                        var obj = (GameObject)a.GetGameObject();
                        hp[a.SteamId] = a.GetHealth();
                        item[a.SteamId] = a.GetInventory();
                        ammo5[a.SteamId] = a.GetAmmo(AmmoType.DROPPED_5);
                        ammo7[a.SteamId] = a.GetAmmo(AmmoType.DROPPED_7);
                        ammo9[a.SteamId] = a.GetAmmo(AmmoType.DROPPED_9);
                        if (a.TeamRole.Role != Role.SPECTATOR) d[a.SteamId] = new List<float> { obj.transform.position.x, obj.transform.position.y, obj.transform.position.z };
                        if (a.TeamRole.Role == Role.SCP_173) role[a.SteamId] = 0;
                        if (a.TeamRole.Role == Role.CLASSD) role[a.SteamId] = 1;
                        if (a.TeamRole.Role == Role.SPECTATOR) role[a.SteamId] = 2;
                        if (a.TeamRole.Role == Role.SCP_106) role[a.SteamId] = 3;
                        if (a.TeamRole.Role == Role.NTF_SCIENTIST) role[a.SteamId] = 4;
                        if (a.TeamRole.Role == Role.SCP_049) role[a.SteamId] = 5;
                        if (a.TeamRole.Role == Role.SCIENTIST) role[a.SteamId] = 6;
                        if (a.TeamRole.Role == Role.SCP_079) role[a.SteamId] = 7;
                        if (a.TeamRole.Role == Role.CHAOS_INSUGENCY) role[a.SteamId] = 8;
                        if (a.TeamRole.Role == Role.SCP_096) role[a.SteamId] = 9;
                        if (a.TeamRole.Role == Role.SCP_049_2) role[a.SteamId] = 10;
                        if (a.TeamRole.Role == Role.NTF_LIEUTENANT) role[a.SteamId] = 11;
                        if (a.TeamRole.Role == Role.NTF_COMMANDER) role[a.SteamId] = 12;
                        if (a.TeamRole.Role == Role.NTF_CADET) role[a.SteamId] = 13;
                        if (a.TeamRole.Role == Role.TUTORIAL) role[a.SteamId] = 14;
                        if (a.TeamRole.Role == Role.FACILITY_GUARD) role[a.SteamId] = 15;
                        if (a.TeamRole.Role == Role.SCP_939_53) role[a.SteamId] = 16;
                        if (a.TeamRole.Role == Role.SCP_939_89) role[a.SteamId] = 17;
                        if (!playerid.Contains(a.SteamId)) playerid.Add(a.SteamId);
                    }
                }
            }
        }

        public void OnRoundEnd(RoundEndEvent ev)
        {
            if (round)
            {
                playerid.Clear();
                playertime.Clear();
                blacklist.Clear();
                item = new Dictionary<string, List<Smod2.API.Item>>();
                playertime = new List<string>();
                d = new Dictionary<string, List<float>>();
                hp = new Dictionary<string, int>();
                role = new Dictionary<string, int>();
                dtime = new Dictionary<string, int>();
                ammo5 = new Dictionary<string, int>();
                ammo7 = new Dictionary<string, int>();
                ammo9 = new Dictionary<string, int>();
                blacklist = new List<string>();
                playerid = new List<string>();
                round = false;
                start = false;
                warhead = true;
                boom = false;
            }
        }

        public void OnRoundRestart(RoundRestartEvent ev)
        {
            playerid.Clear();
            playertime.Clear();
            blacklist.Clear();
            item = new Dictionary<string, List<Smod2.API.Item>>();
            playertime = new List<string>();
            d = new Dictionary<string, List<float>>();
            hp = new Dictionary<string, int>();
            role = new Dictionary<string, int>();
            dtime = new Dictionary<string, int>();
            ammo5 = new Dictionary<string, int>();
            ammo7 = new Dictionary<string, int>();
            ammo9 = new Dictionary<string, int>();
            blacklist = new List<string>();
            playerid = new List<string>();
            round = false;
            start = false;
            warhead = true;
            boom = false;
        }

        public void OnDetonate()
        {
            if (en == true && start == true && warhead)
            {
                boom = true;
            }
        }
    }
}