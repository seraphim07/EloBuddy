using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Rendering;
using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClayAIO.ClayScripts.Tryndamere
{
    class SpellManager : SpellManagerBase
    {
        public const string R_BUFF_NAME = "Undying Rage";

        public Spell.Active Q;
        public Spell.Active W;
        public Spell.Skillshot E;
        public Spell.Active R;

        private Font Consolas;
        
        public SpellManager() : base()
        {
            Q = new Spell.Active(SpellSlot.Q);
            
            SpellData WData = Player.Instance.Spellbook.GetSpell(SpellSlot.W).SData;
            W = new Spell.Active(
                SpellSlot.W,
                Convert.ToUInt32(WData.CastRadius));
            
            SpellData EData = Player.Instance.Spellbook.GetSpell(SpellSlot.E).SData;
            E = new Spell.Skillshot(
                SpellSlot.E,
                660,
                SkillShotType.Linear,
                Convert.ToInt32(EData.CastTime * 1000f),
                Convert.ToInt32(EData.MissileSpeed),
                Convert.ToInt32(EData.LineWidth))
            {
                AllowedCollisionCount = int.MaxValue
            };

            R = new Spell.Active(SpellSlot.R);

            Consolas = new Font(Drawing.Direct3DDevice, new FontDescription()
            {
                FaceName = "Consolas",
                Height = 50
            });
        }

        public void OnDraw(EventArgs e)
        {
            Circle.Draw(indianRed, E.Range, Player.Instance);

            if (IsRActive)
            {
                Consolas.DrawText(null, "R Remaining: " + GetRRemainingTime().ToString("F2"), 10, 10, Color.Red);
            }
            
            // new Geometry.Polygon.Circle(Player.Instance.ServerPosition, W.Range).Draw(System.Drawing.Color.Yellow);
            // new Geometry.Polygon.Rectangle(Player.Instance.ServerPosition, Game.CursorPos, E.Width).Draw(System.Drawing.Color.Yellow);
        }

        public float GetQHealAmount()
        {
            float baseHealAmount = 20 + Q.Level * 10;
            float baseHealBonusApAmount = Player.Instance.TotalMagicalDamage * 0.3f;

            float healPerFuryAmount = 0.05f + Q.Level * 0.45f;
            float healPerFuryBonusApAmount = Player.Instance.TotalMagicalDamage * 0.012f;

            return baseHealAmount + baseHealBonusApAmount + (healPerFuryAmount + healPerFuryBonusApAmount) * Player.Instance.Mana;
        }

        public bool IsRActive
        {
            get
            {
                return Player.Instance.HasBuff(R_BUFF_NAME);
            }
        }
        
        public float GetRRemainingTime()
        {
            if (IsRActive)
            {
                return Player.Instance.GetBuff(R_BUFF_NAME).EndTime - Game.Time;
            }
            else
            {
                return 0;
            }
        }
        
        public void CastEToHero()
        {
            if (E.IsReady())
            {
                AIHeroClient target = TargetSelector.GetTarget(E.Range, DamageType.Physical);

                if (target != null)
                {
                    CastSkillshotToTarget(E, target);
                }
            }
        }

        public void CastEToMinion()
        {
            if (E.IsReady())
            {
                List<Obj_AI_Minion> minions = EntityManager.MinionsAndMonsters.GetLaneMinions(
                    EntityManager.UnitTeam.Enemy,
                    Player.Instance.ServerPosition,
                    E.Range).ToList();

                if (minions.Count > 0)
                {
                    Tuple<Obj_AI_Base, int> bestTarget = GetBestLinearCastTarget(E, minions);
                    
                    if (bestTarget.Item2 >= 1)
                    {
                        CastSkillshotToTarget(E, bestTarget.Item1);
                    }
                    else
                    {
                        CastSkillshotToTarget(E, minions[0]);
                    }
                }
            }
        }

        public void CastEToJungle()
        {
            if (E.IsReady())
            {
                List<Obj_AI_Minion> monsters = EntityManager.MinionsAndMonsters.GetJungleMonsters(
                    Player.Instance.ServerPosition,
                    E.Range).ToList();

                if (monsters.Count > 0)
                {
                    Tuple<Obj_AI_Base, int> bestTarget = GetBestLinearCastTarget(E, monsters);

                    if (bestTarget.Item2 >= 1)
                    {
                        CastSkillshotToTarget(E, bestTarget.Item1);
                    }
                    else
                    {
                        CastSkillshotToTarget(E, monsters[0]);
                    }
                }
            }
        }
    }
}
