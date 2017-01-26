using System;
using System.Xml;
using Server.Mobiles;
using Server.Items;
using Server.Spells.Third;
using Server.Spells.Seventh;
using Server.Spells.Fourth;
using Server.Spells.Sixth;
using Server.Spells.Chivalry;
using Server.Engines.Quests;

namespace Server.Regions
{
    public class ExploringDeepCreaturesRegion : DungeonRegion
    {
        public ExploringDeepCreaturesRegion(XmlElement xml, Map map, Region parent)
            : base(xml, map, parent)
        {
        }

        Mobile creature;

        public override void OnEnter(Mobile m)
        {
            if (!(m is PlayerMobile) || !(m.IsPlayer()))
                return;

            PlayerMobile pm = m as PlayerMobile;            

            if (m.Region.Name == "Ice Wyrm" && pm.ExploringTheDeepQuest == ExploringTheDeepQuestChain.HeplerPaulson)
            {
                creature = IceWyrm.Spawn(new Point3D(5805 + Utility.RandomMinMax(-5, 5), 240 + Utility.RandomMinMax(-5, 5), 0), Map.Trammel);
            }
            else if (m.Region.Name == "Mercutio The Unsavory" && pm.ExploringTheDeepQuest == ExploringTheDeepQuestChain.CusteauPerron)
            {
                creature = MercutioTheUnsavory.Spawn(new Point3D(2582 + Utility.RandomMinMax(-5, 5), 1118 + Utility.RandomMinMax(-5, 5), 0), Map.Trammel);
            }
            else if (m.Region.Name == "Djinn" && pm.ExploringTheDeepQuest == ExploringTheDeepQuestChain.CusteauPerron)
            {
                creature = Djinn.Spawn(new Point3D(1732 + Utility.RandomMinMax(-5, 5), 520 + Utility.RandomMinMax(-5, 5), 0), Map.Ilshenar);
            }
            else if (m.Region.Name == "Obsidian Wyvern" && pm.ExploringTheDeepQuest == ExploringTheDeepQuestChain.CusteauPerron)
            {
                creature = ObsidianWyvern.Spawn(new Point3D(5136 + Utility.RandomMinMax(-5, 5), 966 + Utility.RandomMinMax(-5, 5), 0), Map.Trammel);
            }
            else if (m.Region.Name == "Orc Engineer" && pm.ExploringTheDeepQuest == ExploringTheDeepQuestChain.CusteauPerron)
            {
                creature = OrcEngineer.Spawn(new Point3D(5311 + Utility.RandomMinMax(-5, 5), 1968 + Utility.RandomMinMax(-5, 5), 0), Map.Trammel);                
            }

            if (creature == null)
                return;
        }
    }

    public class CusteauPerronHouseRegion : GuardedRegion
    {
        public CusteauPerronHouseRegion(XmlElement xml, Map map, Region parent)
            : base(xml, map, parent)
        {
        }

        public override bool OnBeginSpellCast(Mobile from, ISpell s)
        {
            if ((s is TeleportSpell || s is GateTravelSpell || s is RecallSpell || s is MarkSpell || s is SacredJourneySpell) && from.IsPlayer())
            {
                from.SendLocalizedMessage(500015); // You do not have that spell!
                return false;
            }
            else
            {
                return base.OnBeginSpellCast(from, s);
            }
        }
    }


    public class NoTravelSpellsAllowed : BaseRegion
    {
        public NoTravelSpellsAllowed(XmlElement xml, Map map, Region parent)
            : base(xml, map, parent)
        {
        }

        public override bool AllowHousing(Mobile from, Point3D p)
        {
            return false;
        }

        public override bool OnBeginSpellCast(Mobile from, ISpell s)
        {
            if ((s is TeleportSpell || s is GateTravelSpell || s is RecallSpell || s is MarkSpell || s is SacredJourneySpell) && from.IsPlayer())
            {
                from.SendLocalizedMessage(500015); // You do not have that spell!
                return false;
            }
            else
            {
                return base.OnBeginSpellCast(from, s);
            }
        }
    }

    public class Underwater : BaseRegion
    {
        public Underwater(XmlElement xml, Map map, Region parent)
            : base(xml, map, parent)
        {
        }

        public override bool OnMoveInto(Mobile m, Direction d, Point3D newLocation, Point3D oldLocation)
        {
            if (!base.OnMoveInto(m, d, newLocation, oldLocation))
                return false;

            if (!(m is PlayerMobile) || !(m.IsPlayer()))
                return false;

            Item robe = m.FindItemOnLayer(Layer.OuterTorso);
            Item boot = m.FindItemOnLayer(Layer.Shoes);
            Item lens = m.FindItemOnLayer(Layer.Helm);
            Item neck = m.FindItemOnLayer(Layer.Neck);

            PlayerMobile pm = m as PlayerMobile;

            if (m.AccessLevel == AccessLevel.Player)
            {
                if (m.Mounted || m.Flying)
                {
                    m.SendLocalizedMessage(1154411); // You cannot proceed while mounted or flying!
                    return false;
                }
                else if (pm.AllFollowers.Count != 0)
                {
                    pm.SendLocalizedMessage(1154412); // You cannot proceed while pets are under your control!
                    return false;
                }
                else if (m.Alive && !(robe is CanvassRobe) || !(lens is NictitatingLens) || !(boot is BootsOfBallast) || !(neck is AquaPendant))
                {
                    m.SendLocalizedMessage(1154325); // You feel as though by doing this you are missing out on an important part of your journey...
                    return false;
                }
            }

            return true;
        }

        public override bool AllowHousing(Mobile from, Point3D p)
        {
            return false;
        }

        public override bool CheckTravel(Mobile m, Point3D newLocation, Server.Spells.TravelCheckType travelType)
        {
            return false;
        }

        public override bool OnBeginSpellCast(Mobile from, ISpell s)
        {
            if ((s is GateTravelSpell || s is RecallSpell || s is MarkSpell || s is SacredJourneySpell) && from.IsPlayer())
            {
                from.SendLocalizedMessage(500015); // You do not have that spell!
                return false;
            }
            else
            {
                return base.OnBeginSpellCast(from, s);
            }
        }
    }
}