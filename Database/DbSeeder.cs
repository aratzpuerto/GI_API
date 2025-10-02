using GI_API.Models;
using GI_API.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace GI_API.Database
{
    public static class DbSeeder
    {
        public static async System.Threading.Tasks.Task Seed(GIDbContext context, DbService dbService)
        {
            // Ensure migrations are applied first
            try
            {
                context.Database.Migrate();
            }
            catch (SqlException ex) when (ex.Number == 1801)
            {
                // Database already exists, safe to ignore
            }

            // TASK TYPES
            if (!context.TaskTypes.Any())
            {
                context.TaskTypes.AddRange(
                    new TaskType { Name = "Daily" },
                    new TaskType { Name = "Weekly" },
                    new TaskType { Name = "Monthly" },
                    new TaskType { Name = "Other" }
                );
            }

            // WEAPON TYPES
            if (!context.WeaponTypes.Any())
            {
                context.WeaponTypes.AddRange(
                    new WeaponType { Name = "Sword" },
                    new WeaponType { Name = "Claymore" },
                    new WeaponType { Name = "Polearms" },
                    new WeaponType { Name = "Catalyst" },
                    new WeaponType { Name = "Bow" }
                );
            }

            // STATS
            if (!context.Stats.Any())
            {
                context.Stats.AddRange(
                    new Stat { Name = "HP" },
                    new Stat { Name = "ATK" },
                    new Stat { Name = "DEF" },
                    new Stat { Name = "Elemental Mastery" },
                    new Stat { Name = "Energy Recharge" },
                    new Stat { Name = "Crit Rate" },
                    new Stat { Name = "Crit DMG" },
                    new Stat { Name = "Stamina" },
                    new Stat { Name = "Healing Bonus" },
                    new Stat { Name = "Incoming Healing Bonus" },
                    new Stat { Name = "Cooldown Reduction" },
                    new Stat { Name = "Shield Strength" },
                    new Stat { Name = "DMG Bonus" },
                    new Stat { Name = "RES" },
                    new Stat { Name = "Movement Speed" },
                    new Stat { Name = "Attack Speed" },
                    new Stat { Name = "Stamina Consumption" },
                    new Stat { Name = "Interruption Resistance" },
                    new Stat { Name = "Damage Reduction" }
                );
            }

            // REGIONS
            if (!context.Regions.Any())
            {
                context.Regions.AddRange(
                    new Region { Name = "Mondstadt" },
                    new Region { Name = "Liyue" },
                    new Region { Name = "Inazuma" },
                    new Region { Name = "Sumeru" },
                    new Region { Name = "Fontaine" },
                    new Region { Name = "Natlan" },
                    new Region { Name = "Nod-Krai" },
                    new Region { Name = "Snezhnaya" },
                    new Region { Name = "Khaenri'ah" },
                    new Region { Name = "Other" }
                );
            }

            // ELEMENTS
            if (!context.Elements.Any())
            {
                context.Elements.AddRange(
                    new Element { Name = "Anemo" },
                    new Element { Name = "Geo" },
                    new Element { Name = "Electro" },
                    new Element { Name = "Dendro" },
                    new Element { Name = "Hydro" },
                    new Element { Name = "Pyro" },
                    new Element { Name = "Cryo" }
                );
            }

            // ASCENSION MATERIAL TYPES
            if (!context.AscensionMaterialTypes.Any())
            {
                context.AscensionMaterialTypes.AddRange(
                    new AscensionMaterialType { Name = "Talent" },
                    new AscensionMaterialType { Name = "Weapon" }
                );
            }

            // DAYS
            if (!context.Days.Any())
            {
                context.Days.AddRange(
                    new Day { Name = "Monday" },
                    new Day { Name = "Tuesday" },
                    new Day { Name = "Wednesday" },
                    new Day { Name = "Thursday" },
                    new Day { Name = "Friday" },
                    new Day { Name = "Saturday" }
                );
            }

            // DOMAIN TYPES
            if (!context.DomainTypes.Any())
            {
                context.DomainTypes.AddRange(
                    new DomainType { Name = "Talent" },
                    new DomainType { Name = "Weapon" },
                    new DomainType { Name = "Artifact" },
                    new DomainType { Name = "Trounce" }
                );
            }

            // TASKS
            if (!context.Tasks.Any())
            {
                context.Tasks.AddRange(
                    // Daily Tasks
                    new Models.Task { Name = "Daily Commission", Description = "Complete four daily Commissions and claim reward.", TypeId = 1, ShowOrder = 1, Active = true },
                    new Models.Task { Name = "Expedition", Description = "Claim and send back characters on expeditions.", TypeId = 1, ShowOrder = 2, Active = true },
                    new Models.Task { Name = "Serenitea Pot", Description = "Claim Furniture, coins, Friend EXP from Realm Depot.", TypeId = 2, ShowOrder = 3, Active = true },
                    new Models.Task { Name = "Ley Lines", Description = "Complete 3 Ley Lines for BP.", TypeId = 1, ShowOrder = 4, Active = true },
                    new Models.Task { Name = "Fortress of Meropede", Description = "Claim daily Welfare Meal from Bran.", TypeId = 1, ShowOrder = 5, Active = false },
                    new Models.Task { Name = "Easybreeze Market", Description = "Claim daily Gift Egg from Challwa.", TypeId = 1, ShowOrder = 6, Active = false },
                    new Models.Task { Name = "Weekly Boss", Description = "Burn resin by beating a weekly boss.", TypeId = 1, ShowOrder = 7, Active = true },
                    new Models.Task { Name = "World Boss", Description = "Burn resin by beating a world boss.", TypeId = 1, ShowOrder = 8, Active = true },
                    new Models.Task { Name = "Talent Domain", Description = "Obtain talent ascension materials by beating a Talent Domain.", TypeId = 1, ShowOrder = 9, Active = true },
                    new Models.Task { Name = "Weapon Domain", Description = "Obtain weapon ascension materials by beating a Weapon Domain.", TypeId = 1, ShowOrder = 10, Active = true },
                    new Models.Task { Name = "Artifact Domain", Description = "Obtain artifacts by beating an Artifact Domain.", TypeId = 1, ShowOrder = 11, Active = true },

                    // Weekly Tasks
                    new Models.Task { Name = "Parametric Transformer", Description = "Use the Parametric Transformer and claim rewards.", TypeId = 2, ShowOrder = 1001, Active = true },
                    new Models.Task { Name = "Omni-Ubiquity", Description = "Buy Omni-Ubiquity nets from Wakamurasaki in Inazuma.", TypeId = 2, ShowOrder = 1002, Active = true },
                    new Models.Task { Name = "Reputation", Description = "Complete reputation Bounty and Requests.", TypeId = 2, ShowOrder = 1003, Active = true },
                    new Models.Task { Name = "Cook", Description = "Cook 20 dishes for the BP.", TypeId = 2, ShowOrder = 1004, Active = true },
                    new Models.Task { Name = "Mystic Ores", Description = "Forge mystic ores for the BP.", TypeId = 2, ShowOrder = 1005, Active = true },
                    new Models.Task { Name = "Crystalfly Trap", Description = "Retrieve Crystal Cores and put materials.", TypeId = 2, ShowOrder = 1006, Active = true },
                    new Models.Task { Name = "Cat's Tail", Description = "Complete 4 Guest Challenges in the Cat's Tail.", TypeId = 2, ShowOrder = 1007, Active = true },
                    new Models.Task { Name = "Current Event", Description = "Complete current event.", TypeId = 2, ShowOrder = 1008, Active = false },

                    // Monthly Tasks
                    new Models.Task { Name = "Paimon's Bargains", Description = "Buy Fates from Paimon's Baragin Shop.", TypeId = 3, ShowOrder = 2001, Active = false },
                    new Models.Task { Name = "Spyral Abyss", Description = "Beat the Spiral Abyss.", TypeId = 3, ShowOrder = 2002, Active = false },
                    new Models.Task { Name = "Imaginarium Theatre", Description = "Beat the Imaginarium Theatre.", TypeId = 3, ShowOrder = 2003, Active = false },
                    new Models.Task { Name = "Stygian Onslaught", Description = "Beat the Stygian Onslaught.", TypeId = 3, ShowOrder = 2003, Active = false }
                );
            }

            await context.SaveChangesAsync();

            // Reseed all tables to avoid issues with identity insert
            await dbService.ResetSeed("TaskTypes", context.TaskTypes.Max(t => t.Id));
            await dbService.ResetSeed("WeaponTypes", context.WeaponTypes.Max(t => t.Id));
            await dbService.ResetSeed("Stats", context.Stats.Max(t => t.Id));
            await dbService.ResetSeed("Regions", context.Regions.Max(t => t.Id));
            await dbService.ResetSeed("Elements", context.Elements.Max(t => t.Id));
            await dbService.ResetSeed("AscensionMaterialTypes", context.AscensionMaterialTypes.Max(t => t.Id));
            await dbService.ResetSeed("Days", context.Days.Max(t => t.Id));
            await dbService.ResetSeed("DomainTypes", context.DomainTypes.Max(t => t.Id));
            await dbService.ResetSeed("Tasks", context.Tasks.Max(t => t.Id));

        }

    }
}