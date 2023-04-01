using System;
using System.Collections.Generic;
using Sculk.Sensor.Syntax.McFunction.Nodes.Commands;

namespace Sculk.Sensor.Syntax.McFunction.Nodes;

public abstract class McFunctionCommand<TArgs> : AstNode<McFunctionNodeType>
    where TArgs : ICommandArguments {
    public override McFunctionNodeType NodeType => McFunctionNodeType.Command;

    public override List<AstNode<McFunctionNodeType>> Children { get; } = new();

    public string Name { get; }

    public TArgs Arguments { get; }

    protected McFunctionCommand(string name, TArgs arguments) {
        Name = name;
        Arguments = arguments;
    }

    public static AstNode<McFunctionNodeType> Parse(string text) {
        text = text.Trim();
        var name = text[..text.IndexOf(' ')];
        var args = text[text.IndexOf(' ')..].Trim();
        return name switch {
            // ?
            // ability
            "advancement" => new AdvancementCommand(args),
            // alwaysday
            "attribute" => new AttributeCommand(args),
            "ban" => new BanCommand(args),
            "ban-ip" => new BanIpCommand(args),
            "banlist" => new BanlistCommand(args),
            "bossbar" => new BossbarCommand(args),
            // camerashake
            // changesetting
            "clear" => new ClearCommand(args),
            // clearspawnpoint
            "clone" => new CloneCommand(args),
            // connect
            // damage
            "data" => new DataCommand(args),
            "datapack" => new DatapackCommand(args),
            // daylock
            "debug" => new DebugCommand(args),
            // dedicatedwsserver
            "defaultgamemode" => new DefaultgamemodeCommand(args),
            "deop" => new DeopCommand(args),
            // dialogue
            "difficulty" => new DifficultyCommand(args),
            "effect" => new EffectCommand(args),
            "enchant" => new EnchantCommand(args),
            // event
            "execute" => new ExecuteCommand(args),
            "experience" => new ExperienceCommand(args),
            "fill" => new FillCommand(args),
            "fillbiome" => new FillbiomeCommand(args),
            // fog
            "forceload" => new ForceloadCommand(args),
            "function" => new FunctionCommand(args),
            "gamemode" => new GamemodeCommand(args),
            "gamerule" => new GameruleCommand(args),
            // gametest
            "give" => new GiveCommand(args),
            "help" => new HelpCommand(args),
            // immutableworld
            "item" => new ItemCommand(args),
            "jfr" => new JfrCommand(args),
            "kick" => new KickCommand(args),
            "kill" => new KillCommand(args),
            "list" => new ListCommand(args),
            "locate" => new LocateCommand(args),
            "loot" => new LootCommand(args),
            "me" => new MeCommand(args),
            // mobevent
            "msg" => new MsgCommand(args),
            "op" => new OpCommand(args),
            // ops
            "pardon" => new PardonCommand(args),
            "pardon-ip" => new PardonIpCommand(args),
            "particle" => new ParticleCommand(args),
            "perf" => new PerfCommand(args),
            // permission
            "place" => new PlaceCommand(args),
            // playanimation
            "playsound" => new PlaysoundCommand(args),
            "publish" => new PublishCommand(args),
            "recipe" => new RecipeCommand(args),
            "reload" => new ReloadCommand(args),
            // remove
            "replaceitem" => new ReplaceitemCommand(args),
            "ride" => new RideCommand(args),
            // save
            "save-all" => new SaveAllCommand(args),
            "save-off" => new SaveOffCommand(args),
            "save-on" => new SaveOnCommand(args),
            "say" => new SayCommand(args),
            "schedule" => new ScheduleCommand(args),
            "scoreboard" => new ScoreboardCommand(args),
            // script
            // scriptevent
            "seed" => new SeedCommand(args),
            "setblock" => new SetblockCommand(args),
            "setidletimeout" => new SetidletimeoutCommand(args),
            // setmaxplayers
            "setworldspawn" => new SetworldspawnCommand(args),
            "spawnpoint" => new SpawnpointCommand(args),
            "spectate" => new SpectateCommand(args),
            "spreadplayers" => new SpreadplayersCommand(args),
            "stop" => new StopCommand(args),
            "stopsound" => new StopsoundCommand(args),
            // structure
            "summon" => new SummonCommand(args),
            "tag" => new TagCommand(args),
            "team" => new TeamCommand(args),
            "teammsg" => new TeammsgCommand(args),
            "teleport" => new TeleportCommand(args),
            "tell" => new TellCommand(args),
            "tellraw" => new TellrawCommand(args),
            // testfor
            // testforblock
            // testforblocks
            // tickingarea
            "time" => new TimeCommand(args),
            "title" => new TitleCommand(args),
            // titleraw
            "tm" => new TmCommand(args),
            // toggledownfall
            "tp" => new TpCommand(args),
            "trigger" => new TriggerCommand(args),
            // volumearea
            "w" => new WCommand(args),
            // warden_spawn_tracker
            // wb
            "weather" => new WeatherCommand(args),
            "whitelist" => new WhitelistCommand(args),
            "worldborder" => new WorldborderCommand(args),
            // worldbuilder
            // wsserver
            "xp" => new XpCommand(args),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
